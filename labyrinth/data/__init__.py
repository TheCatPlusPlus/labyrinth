__all__ = [
    'data_glyph',
    'data_monster',
    'data_tile',
    'data_anim',
]

def data_glyph(id):
    from .glyphs import _g_glyphs
    return _g_glyphs[id]

def data_monster(id):
    from .monsters import _g_monsters
    return _g_monsters[id]

def data_tile(id):
    from .tiles import _g_tiles
    return _g_tiles[id]

def data_anim(id):
    from .anim import _g_anim
    return _g_anim[id]
