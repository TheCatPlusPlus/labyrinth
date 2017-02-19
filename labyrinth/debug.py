import pdb
from bearlibterminal import terminal
from .globals import *
from .input import read_event, KeyMap, run_modal
from .fov import update_fov
from . import ui

def _reveal_map():
    log_debug('Revealing the map')
    game = this_game()

    for tile in game.level.grid.cells:
        tile.is_lit = True
        tile.is_lit = False

    update_fov(game.player, game.level)

def _run_pdb():
    game   = this_game()
    player = game.player
    level  = game.level

    log_debug('Breaking into debugger')
    pdb.set_trace()

_g_debug_menu = {
    'dbg-reveal-map': ('r', 'reveal map', _reveal_map),
    'dbg-run-pdb':    ('p', 'run pdb', _run_pdb),
}

_g_debug_keymap = KeyMap()
_g_debug_keymap.add(ACTION_CANCEL, 'escape', 'q')
for name, entry in _g_debug_menu.items():
    _g_debug_keymap.add(name, entry[0])

_g_debug_labels = [
    f'[[[color=green]{e[0]}[/color]]] {e[1]}'
    for e in _g_debug_menu.values()
]
_g_debug_labels.append('[[[color=green]ESC[/color]]] cancel')

def debug_menu():
    log_debug('Showing debug menu')

    with ui.colors('black', 'black'):
        terminal.print(0, 0, ' ' * get_grid_width())

    terminal.print(0, 0, ' | '.join(_g_debug_labels), align = terminal.TK_ALIGN_CENTER, width = get_grid_width())

    def on_key(key):
        action = _g_debug_keymap.query(key)

        if action is None:
            return None
        elif action == ACTION_CANCEL:
            return False
        else:
            _g_debug_menu[action][2]()
            return True

    run_modal(on_key)
    log_debug('Hiding debug menu')
