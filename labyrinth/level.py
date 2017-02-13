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
        self._type    = type

    @property
    def x(self):
        return self._x

    @property
    def y(self):
        return self._y

    @property
    def type(self):
        return self._type

    @type.setter
    def type(self, value):
        self._type = value

    @property
    def is_walkable(self):
        return self._monster is None and data_tile(self._type).is_walkable

    @property
    def is_wall(self):
        return self.type in (TILE_WALL, TILE_WALL_DEEP)

    @property
    def is_door(self):
        return self.type in (TILE_DOOR_OPENED, TILE_DOOR_CLOSED)

    @property
    def base_move_cost(self):
        return -1 if not self.is_walkable else data_tile(self._type).base_move_cost

    def __str__(self):
        from .data import data_glyph
        glyph = data_glyph(self.type).glyph
        return f'({self.x}, {self.y}): {self.type} ({glyph})'

class Level:
    def __init__(self, width, height):
        self._grid = Grid(width, height, Tile)

    @property
    def grid(self):
        return self._grid
