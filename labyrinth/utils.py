class Rect:
    def __init__(self, x, y, w, h):
        self.x      = x
        self.y      = y
        self.width  = w
        self.height = h

    @property
    def x2(self):
        return self.x + self.width

    @property
    def y2(self):
        return self.y + self.height

    def as_tuple(self):
        return (self.x, self.y, self.width, self.height)

    def inflate(self, distance):
        return Rect(self.x - distance, self.y - distance, self.width + distance * 2, self.height + distance * 2)

    def overlaps(self, other):
        return self.distance_to(other) <= 0

    def distance_to(self, other):
        if self.y >= other.y2:
            vertical = self.y - other.y2
        elif self.y2 <= other.y:
            vertical = other.y - self.y2
        else:
            vertical = -1

        if self.x >= other.x2:
            horizontal = self.x - other.x2
        elif self.x2 <= other.x:
            horizontal = other.x2 - self.x
        else:
            horizontal = -1

        if vertical == -1 and horizontal == -1:
            return -1
        if vertical == -1:
            return horizontal
        if horizontal == -1:
            return vertical
        return horizontal + vertical

    def __str__(self):
        return f'Rect(pos = ({self.x}, {self.y}), w = {self.width}, h = {self.height})'

    def __repr__(self):
        return str(self)

    def __eq__(self, other):
        return self.as_tuple() == other.as_tuple()

    def __hash__(self):
        return hash(self.as_tuple())

    def __iter__(self):
        for y in range(self.y, self.y + self.height):
            for x in range(self.x, self.x + self.width):
                yield (x, y)

    def __contains__(self, pos):
        x, y = pos
        return x >= self.x and x < self.x2 and y >= self.y and y < self.y2

def clamp(value, min_value, max_value):
    return min(max_value, max(min_value, value))

def irange(min, max, step = None):
    if step is not None:
        return range(min, max + 1, step)
    else:
        return range(min, max + 1)
