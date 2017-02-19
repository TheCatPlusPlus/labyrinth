# derived from http://www.roguebasin.com/index.php?title=Permissive_Field_of_View_in_Javascript
# TODO square los would be nice at some point

import math, copy
from .globals import *

class Line:
    def __init__(self, p, q):
        self.p = p
        self.q = q

    def cw(self, x, y):
        return self.dtheta(x, y) > 0

    def ccw(self, x, y):
        return self.dtheta(x, y) < 0

    def dtheta(self, x, y):
        theta = math.atan2(self.q[1] - self.p[1], self.q[0] - self.p[0])
        other = math.atan2(y - self.p[1], x - self.p[0])
        dt = other - theta

        if dt > -math.pi:
            return dt
        else:
            return dt + 2 * math.pi

class Arc:
    def __init__(self, steep, shallow):
        self.steep   = steep
        self.shallow = shallow

        self.steep_bumps   = []
        self.shallow_bumps = []

    def hits(self, x, y):
        return self.steep.ccw(x + 1, y) and self.shallow.cw(x, y + 1)

    def bump_cw(self, x, y):
        bump = (x + 1, y)
        self.steep_bumps.append(bump)
        self.steep.q = bump

        for bump in self.shallow_bumps:
            if self.steep.cw(*bump):
                self.steep.p = bump

    def bump_ccw(self, x, y):
        bump = (x, y + 1)
        self.shallow_bumps.append(bump)
        self.shallow.q = bump

        for bump in self.steep_bumps:
            if self.shallow.ccw(*bump):
                self.shallow.p = bump

    def shade(self, x, y):
        steep_block = self.steep.cw(x, y + 1)
        shallow_block = self.shallow.ccw(x + 1, y)

        if steep_block and shallow_block:
            return []
        elif steep_block:
            self.bump_cw(x, y)
            return [self]
        elif shallow_block:
            self.bump_ccw(x, y)
            return [self]
        else:
            a = copy.deepcopy(self)
            b = copy.deepcopy(self)
            a.bump_cw(x, y)
            b.bump_ccw(x, y)
            return [a, b]

class Light:
    def __init__(self, r):
        wide = Arc(
            Line((1, 0), (0, r)),
            Line((0, 1), (r, 0)),
        )
        self.arcs = [wide]

    def hits(self, x, y):
        for idx, arc in enumerate(self.arcs):
            if arc.hits(x, y):
                return idx
        return None

    def shade(self, idx, x, y):
        arc = self.arcs[idx]
        new_arcs = arc.shade(x, y)

        self.arcs = self.arcs[:idx] + new_arcs + self.arcs[idx + 1:]
        return len(self.arcs) > 0

def _fov(ox, oy, r, visit, blocked):
    def quadrant(dx, dy):
        light = Light(r)

        for dr in irange(1, r):
            for i in irange(0, dr):
                x = dr - i
                y = i

                arc = light.hits(x, y)
                if arc is None:
                    continue

                ax = ox + x * dx
                ay = oy + y * dy
                visit(ax, ay)

                if not blocked(ax, ay):
                    continue

                if not light.shade(arc, x, y):
                    return

    visit(ox, oy)
    quadrant(-1, +1)
    quadrant(+1, +1)
    quadrant(-1, -1)
    quadrant(+1, -1)

def update_fov(player, level):
    def visit(x, y):
        tile = level.grid[x, y]
        tile.is_lit = True
        player.fov.append(tile)

    def blocked(x, y):
        tile = level.grid[x, y]
        return not tile.is_transparent

    for tile in player.fov:
        tile.is_lit = False

    player.fov = []
    _fov(player.x, player.y, FOV_RADIUS, visit, blocked)
