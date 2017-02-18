import random
from .globals import *
from .data import data_tile

class OutOfBounds(Exception):
    def __init__(self, x, y):
        super().__init__(f'Point ({x}, {y}) is out of bounds')
        self.x = x
        self.y = y

class Grid:
    def __init__(self, width, height, make_cell):
        self._rect      = Rect(0, 0, width, height)
        self._make_cell = make_cell

        self._cells = [
            self._make_cell(x, y)
            for y in range(self.height)
            for x in range(self.width)
        ]

    @property
    def width(self):
        return self._rect.width

    @property
    def height(self):
        return self._rect.height

    @property
    def cells(self):
        for pos in self._rect:
            yield self[pos]

    @property
    def rect(self):
        return self._rect

    def __getitem__(self, pos):
        idx = self._index(pos)
        return self._cells[idx]

    def __setitem__(self, pos, cell):
        idx = self._index(pos)
        self._cells[idx] = cell

    def remake_cell(self, x, y, *args, **kwargs):
        self[x, y] = self._make_cell(x, y, *args, **kwargs)

    def _index(self, pos):
        x, y = pos

        if x < 0 or x >= self.width or y < 0 or y >= self.height:
            raise OutOfBounds(x, y)

        idx = x + y * self.width
        return idx

class Tile:
    def __init__(self, x, y, type = TILE_WALL_DEEP):
        self._x       = x
        self._y       = y
        self._items   = []
        self._monster = None

        self.type = type
        self.tag  = None

    @property
    def x(self):
        return self._x

    @property
    def y(self):
        return self._y

    @property
    def is_walkable(self):
        return self._monster is None and data_tile(self.type).is_walkable

    @property
    def is_wall(self):
        return self.type in (TILE_WALL, TILE_WALL_DEEP)

    @property
    def is_door(self):
        return self.type in (TILE_DOOR_OPEN, TILE_DOOR_CLOSED)

    @property
    def is_exit(self):
        return self.type in (TILE_STARS_UP, TILE_STAIRS_DOWN)

    @property
    def base_move_cost(self):
        return -1 if not self.is_walkable else data_tile(self.type).base_move_cost

    @property
    def monster(self):
        return self._monster

    @property
    def items(self):
        return self._items

    def add_entity(self, entity):
        from .game import Monster, Item

        if not self.is_walkable:
            return False

        if isinstance(entity, Monster):
            self._monster = entity
        elif isinstance(entity, Item):
            self._items.append(entity)

        entity.x = self.x
        entity.y = self.y
        return True

    def remove_entity(self, entity):
        from .game import Monster, Item

        assert entity.x == self.x and entity.y == self.y, 'Given entity is not on this tile'
        entity.x = None
        entity.y = None

        if isinstance(entity, Monster):
            self._monster = None
        elif isinstance(entity, Item):
            self._items.remove(entity)

    def __str__(self):
        from .data import data_glyph
        glyph = data_glyph(self.type).glyph
        return f'({self.x}, {self.y}): {self.type} ({glyph})'

class Level:
    def __init__(self, width, height, name):
        self._grid = Grid(width, height, Tile)
        self._name = name

    @property
    def grid(self):
        return self._grid

    @property
    def name(self):
        return self._name

    @property
    def walkable_tiles(self):
        # TODO maybe cache this
        return [
            tile
            for tile in self.grid.cells
            if tile.is_walkable
        ]

    def find_spawn(self):
        tile = random.choice(self.walkable_tiles)
        return (tile.x, tile.y)

def move(entity, x, y):
    current_tile = entity.level.grid[entity.x, entity.y]

    try:
        new_tile = entity.level.grid[x, y]
    except OutOfBounds:
        return False

    if not new_tile.is_walkable:
        if new_tile.is_door:
            new_tile.type = TILE_DOOR_OPEN
        return False

    current_tile.remove_entity(entity)
    new_tile.add_entity(entity)

def despawn(entity):
    assert entity.level is not None, 'Trying to despawn unspawned entity'
    level.grid[entity.x, entity.y].remove_entity(entity)
    entity.level = None

def spawn(entity, level, x, y):
    if entity.level is not None:
        despawn(entity)

    assert level.grid[x, y].add_entity(entity), 'Failed to spawn entity (taken spot)'
    entity.level = level
