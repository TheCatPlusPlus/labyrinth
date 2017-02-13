import random
from .globals import *

class Zone:
    def __init__(self, name, total_depth):
        self._name        = name
        self._total_depth = total_depth

    @property
    def name(self):
        return self._name

    @property
    def total_depth(self):
        return self._total_depth

    def get_generator(self, level, depth):
        return ZoneGenerator(self, level, depth)

class ZoneGenerator:
    def __init__(self, zone, level, depth):
        self._level = level
        self._depth = depth
        self._zone  = zone

    def __call__(self):
        self._place_stairs()
        yield 'placed stairs'

        self._place_vaults()
        yield 'placed vaults'

        self._spawn_items()
        yield 'spawned items'

        self._spawn_monsters()
        yield 'spawned monsters'

    def _place_stairs(self):
        candidates = self._level.walkable_tiles
        random.shuffle(candidates)

        up_count = down_count = 3

        if self._depth == 0:
            up_count = 1
        if self._depth == self._zone.total_depth - 1:
            down_count = 0

        def add(idx, type):
            tile      = candidates.pop(-1)
            tile.type = type
            tile.tag  = f'stairs:{idx}'

        for idx in range(up_count):
            add(idx, TILE_STAIRS_UP)

        for idx in range(down_count):
            add(idx, TILE_STAIRS_DOWN)

    def _place_vaults(self):
        pass

    def _spawn_items(self):
        pass

    def _spawn_monsters(self):
        pass
