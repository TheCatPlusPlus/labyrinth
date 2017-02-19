from .log import *
from .const import *
from .utils import *

_g_scene       = None
_g_game        = None
_g_is_running  = True
_g_menu_keymap = None
_g_game_keymap = None

def set_next_scene(scene_type, *args):
    global _g_scene

    if _g_scene is not None:
        log_verbose(f'Exiting scene {type(_g_scene)}')
        _g_scene.on_exit()

    log_verbose(f'Entering scene {scene_type}')
    _g_scene = scene_type(*args)
    _g_scene.on_enter()

def this_scene():
    return _g_scene

def signal_exit():
    global _g_is_running
    _g_is_running = False

def is_running():
    return _g_is_running

def get_grid_width():
    return WIDTH_TOTAL

def get_grid_height():
    return HEIGHT_TOTAL

def get_game_display_name():
    return 'The Labyrinth'

def get_game_internal_name():
    return 'the-labyrinth'

def get_game_version():
    return '0.1.0'

def get_data_path():
    from pathlib import Path

    return Path(__file__).resolve().parent / 'data'

def get_user_data_path():
    import appdirs
    from pathlib import Path

    if Path('.git').exists():
        path = Path('.').resolve() / '_userdata'
    else:
        path = appdirs.user_data_dir(get_game_internal_name(), '', roaming = True)
        path = Path(path)

    path.mkdir(parents = True, exist_ok = True)
    return path

def is_game_loaded():
    return _g_game is not None

def this_game():
    assert is_game_loaded(), 'this_game() called before game is loaded'
    return _g_game

def get_save_path():
    return get_user_data_path() / 'current.save'

def new_game(*args, **kwargs):
    from .game import Game

    if is_game_loaded():
        discard_game()

    global _g_game
    _g_game = Game(*args, **kwargs)

def load_game():
    from .saves import do_load_game

    global _g_game
    _g_game = do_load_game(get_save_path())

def save_game():
    from .saves import do_save_game

    assert is_game_loaded(), 'save_game() called before game is loaded'

    do_save_game(get_save_path(), _g_game)
    signal_exit()

def discard_game():
    assert is_game_loaded(), 'discard_game() called before game is loaded'

    global _g_game
    _g_game = None

    try:
        get_save_path().unlink()
    except (OSError, IOError):
        pass

def has_saved_game():
    return get_save_path().exists()

def load_keymaps():
    from .input import KeyMap

    global _g_menu_keymap
    global _g_game_keymap

    def add_common(keymap):
        keymap.add(ACTION_CANCEL,  'escape')
        keymap.add(ACTION_CONFIRM, 'enter',  'num-enter')

    _g_menu_keymap = KeyMap()
    _g_game_keymap = KeyMap()

    add_common(_g_menu_keymap)
    add_common(_g_game_keymap)

    _g_game_keymap.add(ACTION_QUIT,            'ctrl q')
    _g_game_keymap.add(ACTION_SAVE_QUIT,       'ctrl s')
    _g_menu_keymap.add(ACTION_PREVIOUS_ITEM,   'up',     'num-8')
    _g_menu_keymap.add(ACTION_NEXT_ITEM,       'down',   'num-2')
    _g_game_keymap.add(ACTION_MOVE_NORTH,      '8',      'num-8', 'up')
    _g_game_keymap.add(ACTION_MOVE_SOUTH,      '2',      'num-2', 'down')
    _g_game_keymap.add(ACTION_MOVE_WEST,       '4',      'num-4', 'left')
    _g_game_keymap.add(ACTION_MOVE_EAST,       '6',      'num-6', 'right')
    _g_game_keymap.add(ACTION_MOVE_NORTH_WEST, '7',      'num-7')
    _g_game_keymap.add(ACTION_MOVE_SOUTH_WEST, '1',      'num-1')
    _g_game_keymap.add(ACTION_MOVE_NORTH_EAST, '9',      'num-9')
    _g_game_keymap.add(ACTION_MOVE_SOUTH_EAST, '3',      'num-3')
    _g_game_keymap.add(ACTION_WAIT,            '5',      'num-5', '.')
    _g_game_keymap.add(ACTION_TAKE_STAIRS,     '<',      '>')
    _g_game_keymap.add(ACTION_SHOW_INVENTORY,  'i')
    _g_game_keymap.add(ACTION_SHOW_CHARACTER,  '@')
    _g_game_keymap.add(ACTION_DEBUG_MENU,      'f12')

def get_menu_keymap():
    return _g_menu_keymap

def get_game_keymap():
    return _g_game_keymap

def get_viewport_map_origin():
    def go(player_c, view_c, map_c):
        if player_c < (view_c // 2):
            return 0
        elif player_c >= map_c - (view_c // 2):
            return map_c - view_c
        else:
            return player_c - (view_c // 2)

    game   = this_game()
    player = game.player
    level  = game.level

    if level.grid.width > WIDTH_VIEWPORT:
        x0 = go(player.x, WIDTH_VIEWPORT, level.grid.width)
    else:
        x0 = 0

    if level.grid.height > HEIGHT_VIEWPORT:
        y0 = go(player.y, HEIGHT_VIEWPORT, level.grid.height)
    else:
        y0 = 0

    return x0, y0

def get_viewport_screen_origin():
    cx0 = WIDTH_SIDEBAR + 2
    cy0 = HEIGHT_BOSS_METER + 2
    return cx0, cy0

def map_to_screen(x, y):
    x0, y0   = get_viewport_map_origin()
    cx0, cy0 = get_viewport_screen_origin()

    dx = x - x0
    dy = y - y0

    cx = cx0 + dx
    cy = cy0 + dy

    this_game().level.grid[x, y] # for OOB

    if dx < 0 or dx >= WIDTH_VIEWPORT or dy < 0 or dy >= HEIGHT_VIEWPORT:
        raise OutOfBounds(cx, cy)

    return cx, cy
