import random
from .globals import *
from .dungen import Generator
from .level import Level

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

    @property
    def _level_width(self):
        return WIDTH_VIEWPORT

    @property
    def _level_height(self):
        return HEIGHT_VIEWPORT

    def make_level(self, depth):
        level = Level(self._level_width | 1, self._level_height | 1, f'{self.name}:{depth + 1}')

        level_gen = Generator(level)
        for progress in level_gen():
            log_info(f'Generating {level.name}: level structure: {progress}')

        zone_gen = ZoneGenerator(self, level, depth)
        for progress in zone_gen():
            log_info(f'Generating {level.name}: zone specifics: {progress}')

        return level

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
