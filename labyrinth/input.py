import collections
from bearlibterminal import terminal
from .keydb import IGNORED_KEYS, parse_key, unparse_key, Key
from .const import *

class KeyMap:
    def __init__(self):
        self._default_keys = {}
        self._user_keys    = {}
        self._actions      = set()
        self._action_keys  = collections.defaultdict(set)

    def _bind_to(self, action, key_spec, mapping):
        key = parse_key(key_spec)

        if key in mapping:
            raise ValueError(f'Duplicated binding: {action} and {mapping[key]}')

        mapping[key] = action
        self._action_keys[action].add(key)

    def add(self, action, *default_keys):
        self._actions.add(action)

        for key in default_keys:
            self._bind_to(action, key, self._default_keys)

    def bind(self, action, *keys):
        if action not in self._actions:
            raise ValueError(f'Unknown action: {action}')

        if not keys:
            raise ValueError('At least one key must be specified')

        for key in keys:
            self._bind_to(action, key, self._user_keys)

    def query(self, key):
        return self._user_keys.get(key, self._default_keys.get(key, None))

def read_event():
    event = terminal.read()

    if event == terminal.TK_CLOSE:
        return (EVENT_CLOSE,)
    elif event == terminal.TK_RESIZED:
        return (EVENT_RESIZE,)
    elif event == terminal.TK_MOUSE_MOVE:
        x = terminal.state(terminal.TK_MOUSE_X)
        y = terminal.state(terminal.TK_MOUSE_Y)
        return (EVENT_MOUSE_MOVE, x, y)
    elif event == terminal.TK_MOUSE_SCROLL:
        wheel = terminal.state(terminal.TK_MOUSE_WHEEL)
        return (EVENT_MOUSE_SCROLL, wheel)
    elif event in IGNORED_KEYS or (event & terminal.TK_KEY_RELEASED) == terminal.TK_KEY_RELEASED:
        return None
    else:
        shift = terminal.check(terminal.TK_SHIFT)
        ctrl  = terminal.check(terminal.TK_CONTROL)
        alt   = terminal.check(terminal.TK_ALT)
        key   = Key(event, ctrl, shift, alt)
        return (EVENT_KEY, key)

def has_event():
    return terminal.peek() != 0

def run_modal(on_key):
    terminal.refresh()

    while True:
        event = read_event()

        if event is None or event[0] != EVENT_KEY:
            continue

        result = on_key(event[1])
        if result is not None:
            return result
