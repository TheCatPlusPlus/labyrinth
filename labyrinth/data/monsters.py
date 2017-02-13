from collections import namedtuple
from ..const import *

class Monster:
    def __init__(self, id):
        self._id = id

    @property
    def id(self):
        return self._id

_g_monsters = {}

def _(id, *args, **kwargs):
    assert id not in _g_monsters, f'Duplicate monster {id}'
    _g_monsters[id] = Monster(id, *args, **kwargs)
