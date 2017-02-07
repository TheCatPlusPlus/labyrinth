from bearlibterminal import terminal
from contextlib import contextmanager
from .globals import *
from .input import read_event

# single line box
BOX_UL_CORNER      = '\u250C'
BOX_UR_CORNER      = '\u2510'
BOX_DL_CORNER      = '\u2514'
BOX_DR_CORNER      = '\u2518'
BOX_UL_CORNER_SOFT = '\u256C'
BOX_UR_CORNER_SOFT = '\u256D'
BOX_DL_CORNER_SOFT = '\u2570'
BOX_DR_CORNER_SOFT = '\u256F'
BOX_VLINE          = '\u2502'
BOX_HLINE          = '\u2500'
BOX_VLINE_L_SPLIT  = '\u251C'
BOX_VLINE_R_SPLIT  = '\u2524'
BOX_HLINE_D_SPLIT  = '\u252C'
BOX_HLINE_U_SPLIT  = '\u2534'
BOX_CROSS_SPLIT    = '\u253C'

class Menu:
    def __init__(self, *choices):
        self.choices = choices

    @property
    def choices(self):
        return self._choices

    @choices.setter
    def choices(self, value):
        self._choices      = list(value)
        self._current      = 0
        self._cursor_shape = '->'
        self._width        = max(len(c[0]) for c in self._choices) + len(self._cursor_shape) + 1

    @property
    def width(self):
        return self._width

    @property
    def height(self):
        return len(self._choices)

    @property
    def current(self):
        return self._choices[self._current][1]

    @property
    def can_confirm(self):
        return self._is_enabled(self._current)

    def _is_enabled(self, idx):
        choice = self._choices[idx]
        try:
            return choice[2]() if callable(choice[2]) else bool(choice[2])
        except IndexError:
            return True

    def _move_cursor(self, delta):
        self._current = (self._current + delta) % len(self._choices)

    def react(self, event, *args):
        if event != EVENT_KEY:
            return None

        action = get_menu_keymap().query(args[0])

        if action == ACTION_CONFIRM and self.can_confirm:
            return (ACTION_CONFIRM, self.current)
        elif action == ACTION_CANCEL:
            return (ACTION_CANCEL,)
        elif action == ACTION_PREVIOUS_ITEM:
            self._move_cursor(-1)
        elif action == ACTION_NEXT_ITEM:
            self._move_cursor(+1)

        return None

    def draw(self, x, y):
        x = int(x)
        y = int(y)

        for idx, choice in enumerate(self._choices):
            label = choice[0]
            is_current = idx == self._current

            if not self._is_enabled(idx):
                color = 'dark grey'
            elif is_current:
                color = 'green'
            else:
                color = 'white'

            if is_current:
                cursor = self._cursor_shape
            else:
                cursor = ' ' * len(self._cursor_shape)

            terminal.print(x, y + idx, f'[color={color}]{cursor} {label}[/color]')

class Input:
    def __init__(self, x, y, max_width, default = ''):
        assert len(default) < max_width

        self._value     = default
        self._max_width = max_width
        self._x         = x
        self._y         = y

    @property
    def value(self):
        return self._value

    def focus(self):
        self._clear()

        result, new_value = terminal.read_str(self._x, self._y, self._value, self._max_width)
        new_value = new_value.strip()

        if result != terminal.TK_INPUT_CANCELLED and len(new_value) > 0:
            self._value = new_value

    def _clear(self):
        with background('dark grey'):
            for x in range(self._x, self._x + self._max_width + 1):
                terminal.put(x, self._y, ' ')

    def draw(self):
        self._clear()
        with background('dark grey'):
            terminal.print(self._x, self._y, self._value)

def box(x, y, w, h):
    vline(x,     y, h)
    vline(x + w, y, h)

    hline(x, y,     w)
    hline(x, y + h, w)

    terminal.put(x,     y,     BOX_UL_CORNER)
    terminal.put(x + w, y,     BOX_UR_CORNER)
    terminal.put(x,     y + h, BOX_DL_CORNER)
    terminal.put(x + w, y + h, BOX_DR_CORNER)

def vline(x, y, h, ch = BOX_VLINE):
    for i in range(0, h):
        terminal.put(x, y + i, ch)

def hline(x, y, w, ch = BOX_HLINE):
    for i in range(0, w):
        terminal.put(x + i, y, ch)

def modal_confirm(message):
    # TODO multiline maybe
    x = get_grid_width() // 2
    y = get_grid_height() // 2 - 1

    yes_no = f'[[[color=green]Y[/color]]]es / [[[color=green]N[/color]]]o'

    yes_no_len, _  = terminal.measure(yes_no)
    message_len, _ = terminal.measure(message)

    width = max(message_len, yes_no_len) + 3
    x -= width // 2

    terminal.print(x + 1, y + 1, ' ' * width)
    terminal.print(x + 1, y + 2, ' ' * width)
    box(x, y, width, 2)
    terminal.print(x + 1, y + 1, message, align = terminal.TK_ALIGN_CENTER, width = width - 2)
    terminal.print(x + 1, y + 2, yes_no, align = terminal.TK_ALIGN_CENTER, width = width - 2)
    terminal.refresh()

    while True:
        event = read_event()

        if event is None:
            continue
        elif event[0] != EVENT_KEY:
            return False

        key = event[1].id
        if key == terminal.TK_Y:
            return True
        elif key in (terminal.TK_N, terminal.TK_ESCAPE):
            return False

@contextmanager
def background(bg):
    previous = terminal.state(terminal.TK_BKCOLOR)
    terminal.bkcolor(bg)
    yield
    terminal.bkcolor(previous)

@contextmanager
def foreground(fg):
    previous = terminal.state(terminal.TK_COLOR)
    terminal.color(fg)
    yield
    terminal.color(previous)

@contextmanager
def colors(fg, bg):
    with foreground(fg):
        with background(bg):
            yield

def hbar(x, y, w, bg):
    with background(bg):
        hline(x, y, w, ' ')

def gauge(y, label, g, color, loss_color, gain_color):
    if g.percent < 0.25:
        value_fg = 'red'
    elif g.percent < 0.5:
        value_fg = 'yellow'
    else:
        value_fg = 'white'

    if g.prev_value > g.value:
        change_color = loss_color
    else:
        change_color = gain_color

    last_width = int(round(g.prev_percent * WIDTH_GAUGES))
    current_width = int(round(g.percent * WIDTH_GAUGES))
    x = WIDTH_SIDEBAR - WIDTH_GAUGES - 1

    value = f'{label}: [color={value_fg}]{g.value}[/color] / {g.max_value}'
    terminal.print(1, y, value)

    hbar(x, y, WIDTH_GAUGES, 'dark grey')

    if last_width < current_width:
        # if we gained value, we want to render the gained part differently
        # so swap current with last
        current_width, last_width = last_width, current_width

    hbar(x, y, last_width, change_color)
    hbar(x, y, current_width, color)
