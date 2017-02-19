import time, itertools
from bearlibterminal import terminal
from .globals import *
from .data import data_glyph
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

        terminal.refresh()
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

def explosion(x, y, radius, *colors, tile = TILE_EXPLOSION):
    colors    = list(itertools.islice(itertools.cycle(colors), radius + 1))
    glyph     = data_glyph(tile).glyph
    base_rect = Rect(x, y, 1, 1)

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

        yield ANIM_EXPLOSION_FRAME_TIME
