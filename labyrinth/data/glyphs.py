from ..const import *

class Glyph:
    def __init__(self, id, glyph, fg = None, bg = None):
        self._id    = id
        self._glyph = glyph
        self._fg    = fg
        self._bg    = bg

        assert len(glyph) == 1, f'Glyph {self.id} too long: "{self.glyph}"'

    @property
    def id(self):
        return self._id

    @property
    def glyph(self):
        return self._glyph

    @property
    def fg(self):
        return self._fg

    @property
    def bg(self):
        return self._bg

_g_glyphs = {}

def _(id, *args, **kwargs):
    assert id not in _g_glyphs, f'Duplicate glyph {id}'
    _g_glyphs[id] = Glyph(id, *args, **kwargs)

#
# tile types
#
_(TILE_GROUND,      '.', '#252525')
_(TILE_WALL,        '#', '#7d7d7d')
_(TILE_WALL_DEEP,   ' ', bg = 'black')
_(TILE_DOOR_CLOSED, '+', '#c97600')
_(TILE_DOOR_OPENED, '/', '#c97600')

#
# monsters
#
_(MONSTER_PLAYER,  '@', 'white')
_(MONSTER_RAT,     'r', 'light grey')
_(MONSTER_BIG_RAT, 'r', 'dark grey')
