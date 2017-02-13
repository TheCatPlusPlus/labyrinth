__all__ = [
    'data_glyph',
    'data_monster',
    'data_tile',
]

def data_glyph(id_):
    from .glyphs import _g_glyphs
    return _g_glyphs[id_]

def data_monster(id_):
    from .monsters import _g_monsters
    return _g_monsters[id_]

def data_tile(id_):
    from .tiles import _g_tiles
    return _g_tiles[id_]
