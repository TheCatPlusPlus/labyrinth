from bearlibterminal import terminal

class KnownKey:
    def __init__(self, id, name, shifted = False):
        self.id      = id
        self.name    = name
        self.shifted = shifted

class Key:
    def __init__(self, id, ctrl, shift, alt):
        self._id    = id
        self._ctrl  = ctrl
        self._shift = shift
        self._alt   = alt

    @property
    def key(self):
        return (self.id, self.ctrl, self.shift, self.alt)

    @property
    def id(self):
        return self._id

    @property
    def ctrl(self):
        return self._ctrl

    @property
    def alt(self):
        return self._alt

    @property
    def shift(self):
        return self._shift

    @property
    def without_mods(self):
        return Key(self.id, False, False, False)

    def __repr__(self):
        parts = ['0x{:04x}'.format(self.id)]
        if self.ctrl:
            parts.append('ctrl = True')
        if self.shift:
            parts.append('shift = True')
        if self.alt:
            parts.append('alt = True')
        key = ', '.join(parts)
        return f'Key({key})'

    def __str__(self):
        spec = unparse_key(self)
        return f'Key({spec})'

    def __eq__(self, other):
        if not isinstance(other, Key):
            raise TypeError('Key cannot be compared to other types')

        return self.key == other.key

    def __hash__(self):
        return hash(self.key)

IGNORED_KEYS = {
    # bearlib takes care of state tracking
    terminal.TK_SHIFT,
    terminal.TK_CONTROL,
    terminal.TK_ALT,

    # they get enqueued twice for some reason
    #terminal.TK_KP_MULTIPLY,
    #terminal.TK_KP_PLUS,
    #terminal.TK_KP_MINUS,
}

KNOWN_KEYS = [
    # unshifted letters
    KnownKey(terminal.TK_A, 'a'),
    KnownKey(terminal.TK_B, 'b'),
    KnownKey(terminal.TK_C, 'c'),
    KnownKey(terminal.TK_D, 'd'),
    KnownKey(terminal.TK_E, 'e'),
    KnownKey(terminal.TK_F, 'f'),
    KnownKey(terminal.TK_G, 'g'),
    KnownKey(terminal.TK_H, 'h'),
    KnownKey(terminal.TK_I, 'i'),
    KnownKey(terminal.TK_J, 'j'),
    KnownKey(terminal.TK_K, 'k'),
    KnownKey(terminal.TK_L, 'l'),
    KnownKey(terminal.TK_M, 'm'),
    KnownKey(terminal.TK_N, 'n'),
    KnownKey(terminal.TK_O, 'o'),
    KnownKey(terminal.TK_P, 'p'),
    KnownKey(terminal.TK_Q, 'q'),
    KnownKey(terminal.TK_R, 'r'),
    KnownKey(terminal.TK_S, 's'),
    KnownKey(terminal.TK_T, 't'),
    KnownKey(terminal.TK_U, 'u'),
    KnownKey(terminal.TK_V, 'v'),
    KnownKey(terminal.TK_W, 'w'),
    KnownKey(terminal.TK_X, 'x'),
    KnownKey(terminal.TK_Y, 'y'),
    KnownKey(terminal.TK_Z, 'z'),

    # shifted letters
    KnownKey(terminal.TK_A, 'A', shifted = True),
    KnownKey(terminal.TK_B, 'B', shifted = True),
    KnownKey(terminal.TK_C, 'C', shifted = True),
    KnownKey(terminal.TK_D, 'D', shifted = True),
    KnownKey(terminal.TK_E, 'E', shifted = True),
    KnownKey(terminal.TK_F, 'F', shifted = True),
    KnownKey(terminal.TK_G, 'G', shifted = True),
    KnownKey(terminal.TK_H, 'H', shifted = True),
    KnownKey(terminal.TK_I, 'I', shifted = True),
    KnownKey(terminal.TK_J, 'J', shifted = True),
    KnownKey(terminal.TK_K, 'K', shifted = True),
    KnownKey(terminal.TK_L, 'L', shifted = True),
    KnownKey(terminal.TK_M, 'M', shifted = True),
    KnownKey(terminal.TK_N, 'N', shifted = True),
    KnownKey(terminal.TK_O, 'O', shifted = True),
    KnownKey(terminal.TK_P, 'P', shifted = True),
    KnownKey(terminal.TK_Q, 'Q', shifted = True),
    KnownKey(terminal.TK_R, 'R', shifted = True),
    KnownKey(terminal.TK_S, 'S', shifted = True),
    KnownKey(terminal.TK_T, 'T', shifted = True),
    KnownKey(terminal.TK_U, 'U', shifted = True),
    KnownKey(terminal.TK_V, 'V', shifted = True),
    KnownKey(terminal.TK_W, 'W', shifted = True),
    KnownKey(terminal.TK_X, 'X', shifted = True),
    KnownKey(terminal.TK_Y, 'Y', shifted = True),
    KnownKey(terminal.TK_Z, 'Z', shifted = True),

    # unshifted numbers
    KnownKey(terminal.TK_1, '1'),
    KnownKey(terminal.TK_2, '2'),
    KnownKey(terminal.TK_3, '3'),
    KnownKey(terminal.TK_4, '4'),
    KnownKey(terminal.TK_5, '5'),
    KnownKey(terminal.TK_6, '6'),
    KnownKey(terminal.TK_7, '7'),
    KnownKey(terminal.TK_8, '8'),
    KnownKey(terminal.TK_9, '9'),
    KnownKey(terminal.TK_0, '0'),

    # shifted numbers
    KnownKey(terminal.TK_1, '!', shifted = True),
    KnownKey(terminal.TK_2, '@', shifted = True),
    KnownKey(terminal.TK_3, '#', shifted = True),
    KnownKey(terminal.TK_4, '$', shifted = True),
    KnownKey(terminal.TK_5, '%', shifted = True),
    KnownKey(terminal.TK_6, '^', shifted = True),
    KnownKey(terminal.TK_7, '&', shifted = True),
    KnownKey(terminal.TK_8, '*', shifted = True),
    KnownKey(terminal.TK_9, '(', shifted = True),
    KnownKey(terminal.TK_0, ')', shifted = True),

    # unshifted punctuation
    KnownKey(terminal.TK_GRAVE,      '`'),
    KnownKey(terminal.TK_MINUS,      '-'),
    KnownKey(terminal.TK_EQUALS,     '='),
    KnownKey(terminal.TK_LBRACKET,   '['),
    KnownKey(terminal.TK_RBRACKET,   ']'),
    KnownKey(terminal.TK_BACKSLASH,  '\\'),
    KnownKey(terminal.TK_SEMICOLON,  ';'),
    KnownKey(terminal.TK_APOSTROPHE, "'"),
    KnownKey(terminal.TK_COMMA,      ','),
    KnownKey(terminal.TK_PERIOD,     '.'),
    KnownKey(terminal.TK_SLASH,      '/'),

    # shifted punctuation
    KnownKey(terminal.TK_MINUS,      '_', shifted = True),
    KnownKey(terminal.TK_EQUALS,     '+', shifted = True),
    KnownKey(terminal.TK_LBRACKET,   '{', shifted = True),
    KnownKey(terminal.TK_RBRACKET,   '}', shifted = True),
    KnownKey(terminal.TK_BACKSLASH,  '|', shifted = True),
    KnownKey(terminal.TK_SEMICOLON,  ':', shifted = True),
    KnownKey(terminal.TK_APOSTROPHE, '"', shifted = True),
    KnownKey(terminal.TK_GRAVE,      '~', shifted = True),
    KnownKey(terminal.TK_COMMA,      '<', shifted = True),
    KnownKey(terminal.TK_PERIOD,     '>', shifted = True),
    KnownKey(terminal.TK_SLASH,      '?', shifted = True),

    # fn
    KnownKey(terminal.TK_F1,  'f1'),
    KnownKey(terminal.TK_F2,  'f2'),
    KnownKey(terminal.TK_F3,  'f3'),
    KnownKey(terminal.TK_F4,  'f4'),
    KnownKey(terminal.TK_F5,  'f5'),
    KnownKey(terminal.TK_F6,  'f6'),
    KnownKey(terminal.TK_F7,  'f7'),
    KnownKey(terminal.TK_F8,  'f8'),
    KnownKey(terminal.TK_F9,  'f9'),
    KnownKey(terminal.TK_F10, 'f10'),
    KnownKey(terminal.TK_F11, 'f11'),
    KnownKey(terminal.TK_F12, 'f12'),

    # named
    KnownKey(terminal.TK_RETURN,    'return'),
    KnownKey(terminal.TK_ENTER,     'enter'),
    KnownKey(terminal.TK_ESCAPE,    'escape'),
    KnownKey(terminal.TK_BACKSPACE, 'backspace'),
    KnownKey(terminal.TK_TAB,       'tab'),
    KnownKey(terminal.TK_SPACE,     'space'),
    KnownKey(terminal.TK_PAUSE,     'pause'),
    KnownKey(terminal.TK_INSERT,    'insert'),
    KnownKey(terminal.TK_HOME,      'home'),
    KnownKey(terminal.TK_PAGEUP,    'page-up'),
    KnownKey(terminal.TK_DELETE,    'delete'),
    KnownKey(terminal.TK_END,       'end'),
    KnownKey(terminal.TK_PAGEDOWN,  'page-down'),
    KnownKey(terminal.TK_RIGHT,     'right'),
    KnownKey(terminal.TK_LEFT,      'left'),
    KnownKey(terminal.TK_DOWN,      'down'),
    KnownKey(terminal.TK_UP,        'up'),

    # numpad
    KnownKey(terminal.TK_KP_DIVIDE,   'num-/'),
    KnownKey(terminal.TK_KP_MULTIPLY, 'num-*'),
    KnownKey(terminal.TK_KP_MINUS,    'num--'),
    KnownKey(terminal.TK_KP_PLUS,     'num-+'),
    KnownKey(terminal.TK_KP_ENTER,    'num-enter'),
    KnownKey(terminal.TK_KP_1,        'num-1'),
    KnownKey(terminal.TK_KP_2,        'num-2'),
    KnownKey(terminal.TK_KP_3,        'num-3'),
    KnownKey(terminal.TK_KP_4,        'num-4'),
    KnownKey(terminal.TK_KP_5,        'num-5'),
    KnownKey(terminal.TK_KP_6,        'num-6'),
    KnownKey(terminal.TK_KP_7,        'num-7'),
    KnownKey(terminal.TK_KP_8,        'num-8'),
    KnownKey(terminal.TK_KP_9,        'num-9'),
    KnownKey(terminal.TK_KP_0,        'num-0'),
    KnownKey(terminal.TK_KP_PERIOD,   'num-.'),

    # mouse
    KnownKey(terminal.TK_MOUSE_LEFT,   'mouse-left'),
    KnownKey(terminal.TK_MOUSE_RIGHT,  'mouse-right'),
    KnownKey(terminal.TK_MOUSE_MIDDLE, 'mouse-middle'),
    KnownKey(terminal.TK_MOUSE_X1,     'mouse-4'),
    KnownKey(terminal.TK_MOUSE_X2,     'mouse-5'),
]

KEY_BY_NAME     = {key.name: key for key in KNOWN_KEYS}
KEY_BY_ID       = {key.id: key for key in KNOWN_KEYS if not key.shifted}
KEY_BY_SHIFT_ID = {key.id: key for key in KNOWN_KEYS if key.shifted}

def parse_key(spec):
    # key
    # ctrl key
    # shift key
    # ctrl shift key
    # alt shift key
    # ctrl alt shift key
    parts = spec.strip().split()

    if not (1 <= len(parts) <= 4):
        raise ValueError(f'Invalid key spec syntax: {spec}')

    key_name = parts[-1]
    mods     = parts[0:-1]

    shift = ctrl = alt = False

    for mod in mods:
        mod = mod.lower()

        if mod == 'shift':
            shift = True
        elif mod == 'ctrl':
            ctrl = True
        elif mod == 'alt':
            alt = True
        else:
            raise ValueError(f'Invalid modifier {mod} in spec {spec}')

    try:
        key = KEY_BY_NAME[key_name]
    except KeyError:
        try:
            key = KEY_BY_NAME[key_name.lower()]
        except KeyError:
            raise ValueError(f'Unknown key {key_name} in spec {spec}')

    shift = shift or key.shifted
    return Key(key.id, ctrl, shift, alt)

def unparse_key(key):
    if key.shift and key.id in KEY_BY_SHIFT_ID:
        map = KEY_BY_SHIFT_ID
    elif key.id in KEY_BY_ID:
        map = KEY_BY_ID
    else:
        return None

    key_name = map[key.id].name
    mods     = []

    if key.ctrl:
        mods.append('ctrl')
    if key.alt:
        mods.append('alt')
    if key.shift and not key.id in KEY_BY_SHIFT_ID:
        mods.append('shift')

    spec = mods + [key_name]
    return ' '.join(spec).strip()

__all__ = ('IGNORED_KEYS', 'KEY_BY_NAME', 'KEY_BY_ID', 'KEY_BY_SHIFT_ID')
