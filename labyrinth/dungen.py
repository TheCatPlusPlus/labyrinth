# derived from https://github.com/munificent/hauberk/blob/master/lib/src/content/dungeon.dart
#
# Copyright (c) 2000-2014 Bob Nystrom
# Copyright (c) 2017 Cat Plus Plus
#
# Permission is hereby granted, free of charge, to any person
# obtaining a copy of this software and associated
# documentation files (the "Software"), to deal in the
# Software without restriction, including without limitation
# the rights to use, copy, modify, merge, publish, distribute,
# sublicense, and/or sell copies of the Software, and to
# permit persons to whom the Software is furnished to do so,
# subject to the following conditions:
#
# The above copyright notice and this permission notice shall
# be included in all copies or substantial portions of the
# Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY
# KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
# WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
# PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
# OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
# OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
# OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
# SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

import collections, random
from .globals import *
from .level import OutOfBounds, Grid

class RectangleRoom:
    def __init__(self, width, height):
        self.width  = width
        self.height = height

    def place(self, generator, rect):
        for x in range(rect.x, rect.x2):
            generator.add_connector(x, rect.y - 1)
            generator.add_connector(x, rect.y2)

        for y in range(rect.y, rect.y2):
            generator.add_connector(rect.x - 1, y)
            generator.add_connector(rect.x2,    y)

class OctagonRoom:
    def __init__(self, width, height, slope):
        self.width  = width
        self.height = height
        self.slope  = slope

    def place(self, generator, rect):
        top_left     = (rect.x, rect.y)
        top_right    = (rect.x2, rect.y)
        bottom_left  = (rect.x, rect.y2)
        bottom_right = (rect.x2, rect.y2)
        slope        = self.slope

        for pos in rect:
            distance_tl = norm_l1(vec_sub(top_left, pos))
            distance_tr = norm_l1(vec_sub(top_right, pos))
            distance_bl = norm_l1(vec_sub(bottom_left, pos))
            distance_br = norm_l1(vec_sub(bottom_right, pos))

            if distance_tl < slope or distance_tr < slope + 1 or distance_bl < slope + 1 or distance_br < slope + 2:
                generator._level.grid[pos].type = TILE_WALL_DEEP

        center = (rect.x + (rect.width // 2), rect.y + (rect.height // 2))
        generator.add_connector(center[0], rect.y - 1)
        generator.add_connector(center[0], rect.y2)
        generator.add_connector(rect.x - 1, center[1])
        generator.add_connector(rect.x2, center[1])

class RoomPresets:
    def __init__(self):
        self._presets = []
        self._weights = []

    def add_rectangle(self, width, height, rarity = 1):
        self.add(RectangleRoom(width, height), rarity)
        if width != height:
            self.add(RectangleRoom(height, width), rarity)

    def add_octagon(self, width, height, slope, rarity = 1):
        rarity *= 2
        self.add(OctagonRoom(width, height, slope), rarity)
        if width != height:
            self.add(OctagonRoom(height, width, slope), rarity)

    def add(self, preset, rarity):
        self._presets.append(preset)
        self._weights.append(1 / rarity)

    def pick(self):
        return random.choices(self._presets, self._weights)[0]

_g_presets = RoomPresets()
_g_presets.add_rectangle(3, 3, rarity = 2)
_g_presets.add_rectangle(3, 5, rarity = 2)
_g_presets.add_rectangle(5, 5)
_g_presets.add_rectangle(5, 7)
_g_presets.add_rectangle(7, 7)
_g_presets.add_rectangle(5, 9)
_g_presets.add_rectangle(7, 9)
_g_presets.add_rectangle(9, 9)
_g_presets.add_rectangle(7, 11)
_g_presets.add_rectangle(9, 11)
_g_presets.add_rectangle(11, 11, rarity = 2)
_g_presets.add_rectangle(7, 13)
_g_presets.add_rectangle(9, 13, rarity = 2)
_g_presets.add_octagon(5, 5, 1)
_g_presets.add_octagon(5, 7, 1, rarity = 2)
_g_presets.add_octagon(7, 7, 2)
_g_presets.add_octagon(7, 9, 2, rarity = 2)
_g_presets.add_octagon(9, 9, 2)
_g_presets.add_octagon(9, 9, 3)
_g_presets.add_octagon(9, 11, 2, rarity = 2)
_g_presets.add_octagon(9, 11, 3, rarity = 2)
_g_presets.add_octagon(11, 11, 3, rarity = 2)

class Generator:
    def __init__(self, level):
        self._width  = level.grid.width
        self._height = level.grid.height

        assert self._width % 2 != 0 and self._height % 2 != 0, f'Level must be odd-sized, {self._width}x{self._height} given'

        self._level      = level
        self._regions    = Grid(self._width, self._height, lambda x, y: 0)
        self._rooms      = {}
        self._region_num = -1
        self._maze       = []
        self._connectors = []

    def _carve(self, x, y):
        self._level.grid[x, y].type = TILE_GROUND
        self._regions[x, y] = self._region_num

    def _can_carve(self, x, y, dx, dy):
        try:
            self._level.grid[x + dx * 3, y + dy * 3]
        except OutOfBounds:
            return False

        return self._level.grid[x + dx * 2, y + dy * 2].is_wall

    def _start_region(self):
        self._region_num += 1

    def __call__(self):
        self._add_rooms()
        yield 'added rooms'

        self._add_mazes()
        yield 'added mazes'

        self._place_rooms()
        yield 'placed rooms'

        self._connect_regions()
        yield 'connected regions'

        self._remove_dead_ends()
        yield 'removed dead ends'

        self._assign_wall_types()
        yield 'assigned wall types'

    def _add_rooms(self):
        for _ in range(0, DUNGEN_ROOM_TRIES):
            room_type = _g_presets.pick()

            rect = self._find_space(room_type.width, room_type.height)
            if rect is None:
                continue

            self._rooms[rect] = room_type
            self._start_region()

            for x, y in rect:
                self._carve(x, y)

    def _find_space(self, width, height):
        for _ in range(0, DUNGEN_ROOM_POSITION_TRIES):
            x = random.randrange(0, (self._width - width) // 2) * 2 + 1
            y = random.randrange(0, (self._height - height) // 2) * 2 + 1

            rect = Rect(x, y, width, height)
            if any(rect.overlaps(other) for other in self._rooms.keys()):
                return None

            return rect

    def _add_mazes(self):
        for y in range(1, self._height, 2):
            for x in range(1, self._width, 2):
                if not self._level.grid[x, y].is_wall:
                    continue

                self._grow_maze(x, y)

    def _grow_maze(self, x, y):
        cells    = []
        last_dir = None

        self._start_region()
        self._carve(x, y)
        self._maze.append((x, y))

        cells.append((x, y))
        while cells:
            cell = cells[-1]

            unmade_cells = []
            for dx, dy in CARDINALS:
                if self._can_carve(cell[0], cell[1], dx, dy):
                    unmade_cells.append((dx, dy))

            if unmade_cells:
                if last_dir in unmade_cells and random.random() > DUNGEN_WINDING_PERCENT:
                    dir_ = last_dir
                else:
                    dir_ = random.choice(unmade_cells)

                next_cells = [
                    (cell[0] + dir_[0],     cell[1] + dir_[1]),
                    (cell[0] + dir_[0] * 2, cell[1] + dir_[1] * 2),
                ]

                self._carve(*next_cells[0])
                self._carve(*next_cells[1])
                self._maze.append(next_cells[0])
                self._maze.append(next_cells[1])
                cells.append(next_cells[1])

                last_dir = dir_
            else:
                cells.pop(-1)
                last_dir = None

    def _place_rooms(self):
        for rect, room_type in self._rooms.items():
            room_type.place(self, rect)

    def add_connector(self, x, y):
        self._connectors.append((x, y))

    def _connect_regions(self):
        merged_regions = {i: i for i in range(0, self._region_num + 1)}
        allowed_bounds = self._level.grid.rect.inflate(-1)

        random.shuffle(self._connectors)
        for x, y in self._connectors:
            if (x, y) not in allowed_bounds:
                continue

            touching_junction = False
            regions = set()

            for dx, dy in CARDINALS:
                try:
                    region = self._regions[x + dx, y + dy]
                except KeyError:
                    continue

                if region == -1:
                    touching_junction = True
                    break

                region = merged_regions[region]
                regions.add(region)

            if touching_junction:
                continue

            if len(regions) >= 2:
                self._add_junction(x, y)

                regions_lst = list(regions)
                merged_to   = regions_lst[0]
                merged_from = set(regions_lst[1:])

                for idx in range(0, self._region_num + 1):
                    if merged_regions[idx] in merged_from:
                        merged_regions[idx] = merged_to
            elif random.random() < DUNGEN_EXTRA_CONNECTOR_CHANCE:
                self._add_junction(x, y)

    def _add_junction(self, x, y):
        if random.random() < DUNGEN_DOOR_CHANCE:
            tile_type = TILE_DOOR_OPEN if random.random() < DUNGEN_DOOR_OPEN_CHANCE else TILE_DOOR_CLOSED
        else:
            tile_type = TILE_GROUND

        self._level.grid[x, y].type = tile_type
        self._regions[x, y] = -1

    def _remove_dead_ends(self):
        to_check = collections.deque(self._maze)
        while to_check:
            x, y = to_check.pop()

            if self._level.grid[x, y].is_wall:
                continue

            exits = 0
            for dx, dy in CARDINALS:
                tile = self._level.grid[x + dx, y + dy]
                if tile.is_walkable or tile.is_door:
                    exits += 1

            if exits != 1:
                continue

            self._level.grid[x, y].type = TILE_WALL_DEEP
            for dx, dy in CARDINALS:
                to_check.appendleft((x + dx, y + dy))

    def _assign_wall_types(self):
        # transform walls that aren't surrounded on every side
        # into normal TILE_WALL
        for tile in self._level.grid.cells:
            if tile.type != TILE_WALL_DEEP:
                continue

            x, y = tile.x, tile.y

            for dy in irange(-1, 1):
                for dx in irange(-1, 1):
                    try:
                        other = self._level.grid[x + dx, y + dy]
                    except OutOfBounds:
                        continue

                    if not other.is_wall:
                        tile.type = TILE_WALL
