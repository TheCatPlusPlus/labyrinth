from ..const import *

class Anim:
    def __init__(self, id, glyph, *colors):
        assert len(glyph) == 1, f'Glyph {id} too long: "{glyph}"'

        self._id     = id
        self._glyph  = glyph
        self._colors = colors

    @property
    def id(self):
        return self._id

    @property
    def glyph(self):
        return self._glyph

    @property
    def colors(self):
        return self._colors

_g_anim = {}

def _(id, *args, **kwargs):
    assert id not in _g_anim, f'Duplicate animation {id}'
    _g_anim[id] = Anim(id, *args, **kwargs)

_(ANIM_EXPLOSION,  '*', 'orange', 'red')
_(ANIM_PROJECTILE, '*', 'yellow')
