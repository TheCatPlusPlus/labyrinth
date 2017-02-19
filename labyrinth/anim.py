import time, itertools, math
from bearlibterminal import terminal
from .globals import *
from .data import data_anim
from . import ui

def animate(frames):
    while True:
        scene = this_scene()

        terminal.clear()
        scene.draw()

        try:
            next_time = next(frames)
        except StopIteration:
            break

        if next_time is None:
            next_time = ANIM_FRAME_TIME

        terminal.refresh()

        if not terminal.peek():
            time.sleep(next_time / 1000)

        if terminal.peek():
            return

def _explosion_layers(radius):
    assert radius >= 0

    if radius == 0:
        yield [0]
        return

    for i in irange(0, radius):
        yield irange(0, i)

    for i in irange(1, radius):
        yield irange(i, radius)

def _explosion_layer_points(x, y, layer):
    r = layer // 2

    for dx in irange(-r, r):
        yield (x + dx, y - r)
        yield (x + dx, y + r)

    for dy in irange(-r, r):
        yield (x - r, y + dy)
        yield (x + r, y + dy)

def explosion(x, y, radius, id = ANIM_EXPLOSION):
    log_debug(f'anim.explosion({x}, {y}, {radius}, {id})')

    anim      = data_anim(id)
    colors    = list(itertools.islice(itertools.cycle(anim.colors), radius + 1))
    glyph     = anim.glyph
    base_rect = Rect(x, y, 1, 1)

    # TODO: clip at walls
    for frame in _explosion_layers(radius):
        for layer in frame:
            color  = colors[layer]
            points = []
            rect   = base_rect.inflate(layer) if layer > 0 else base_rect

            for x, y in rect.edge_points:
                try:
                    cx, cy = map_to_screen(x, y)
                except OutOfBounds:
                    continue

                with ui.foreground(color):
                    terminal.put(cx, cy, glyph)

        yield

def projectile(x0, y0, x1, y1, id = ANIM_PROJECTILE):
    log_debug(f'anim.projectile({x0}, {y0}, {x1}, {y1}, {id})')

    anim  = data_anim(id)
    color = anim.colors[0]
    glyph = anim.glyph

    x, y = x0, y0

    while x != x1 or y != y1:
        dx = x1 - x
        dy = y1 - y

        dist = math.sqrt(dx * dx + dy * dy)

        x = int(round(x + dx / dist))
        y = int(round(y + dy / dist))

        try:
            if this_game().level.grid[x, y].is_wall:
                return
            cx, cy = map_to_screen(x, y)
        except OutOfBounds:
            continue

        with ui.foreground(color):
            terminal.put(cx, cy, glyph)

        yield
