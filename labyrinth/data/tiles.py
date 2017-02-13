import collections
from ..const import *

class Tile:
    def __init__(self, id, is_walkable = True, base_move_cost = MOVE_COST_BASE):
        self._id             = id
        self._is_walkable    = is_walkable
        self._base_move_cost = base_move_cost

    @property
    def id(self):
        return self._id

    @property
    def is_walkable(self):
        return self._is_walkable

    @property
    def base_move_cost(self):
        return self._base_move_cost

class TileDict(collections.defaultdict):
    def __missing__(self, key):
        self[key] = Tile(key)
        return self[key]

_g_tiles = TileDict()

def _(id, *args, **kwargs):
    assert id not in _g_tiles, f'Duplicate tile {id}'
    _g_tiles[id] = Tile(id, *args, **kwargs)

_(TILE_WALL, is_walkable = False)
_(TILE_WALL_DEEP, is_walkable = False)
_(TILE_DOOR_CLOSED, is_walkable = False)
