from ..const import *

class Glyph:
    def __init__(self, id, glyph, fg = None, bg = None, unlit_fg = None, unlit_bg = None):
        assert len(glyph) == 1, f'Glyph {id} too long: "{glyph}"'

        self._id    = id
        self._glyph = glyph
        self._fg    = fg
        self._bg    = bg

        if unlit_fg is None:
            unlit_fg = DEFAULT_UNLIT_FG

        if bg is not None and bg != 'black':
            unlit_bg = DEFAULT_UNLIT_BG
        else:
            unlit_bg = 'black'

        self._unlit_fg = unlit_fg
        self._unlit_bg = unlit_bg

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

    @property
    def unlit_fg(self):
        return self._unlit_fg

    @property
    def unlit_bg(self):
        return self._unlit_bg

_g_glyphs = {}

def _(id, *args, **kwargs):
    assert id not in _g_glyphs, f'Duplicate glyph {id}'
    _g_glyphs[id] = Glyph(id, *args, **kwargs)

DEFAULT_UNLIT_FG = DEFAULT_UNLIT_BG = '#202020'

#
# tile types
#
_(TILE_EXPLOSION,   '*')
_(TILE_GROUND,      '.', '#555555')
_(TILE_WALL,        ' ', bg = '#505050')
_(TILE_WALL_DEEP,   ' ', bg = 'black')
_(TILE_DOOR_CLOSED, '+', '#c97600')
_(TILE_DOOR_OPEN,   '/', '#c97600')
_(TILE_STAIRS_UP,   '<')
_(TILE_STAIRS_DOWN, '>')

#
# monsters
#
_(MONSTER_PLAYER,  '@', 'white')
_(MONSTER_RAT,     'r', 'light grey')
_(MONSTER_BIG_RAT, 'r', 'dark grey')

#
# items
#
_(ITEM_MULTIPLE, '%', 'white')
