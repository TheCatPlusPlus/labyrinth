from .globals import *
from .level import spawn, move
from .zone import Zone
from .fov import update_fov

class Gauge:
    def __init__(self, label, max_value):
        self._label      = label
        self._max_value  = max_value
        self._value      = max_value
        self._prev_value = max_value

    @property
    def value(self):
        return self._value

    @value.setter
    def value(self, new_value):
        self._prev_value = self._value
        self._value      = clamp(new_value, 0, self._max_value)

    @property
    def max_value(self):
        return self._max_value

    @property
    def prev_value(self):
        return self._prev_value

    @property
    def changed(self):
        return self._prev_value != self._value

    @property
    def percent(self):
        return self._value / self._max_value

    @property
    def prev_percent(self):
        return self._prev_value / self._max_value

    def reset_prev(self):
        self._prev_value = self._value

    def __str__(self):
        return f'{self._label}: {self._value} / {self._max_value} ({self._prev_value})'

class Entity:
    def __init__(self):
        self.x     = None
        self.y     = None
        self.level = None

class Monster(Entity):
    def __init__(self, type):
        super().__init__()

        self.type = type

class Item(Entity):
    def __init__(self, type):
        super().__init__()

        self.type = type

class Player(Monster):
    def __init__(self, name):
        super().__init__(MONSTER_PLAYER)

        self._name = name
        self._hp = Gauge('HP', 100)
        self._mp = Gauge('MP', 100)
        self._stamina = Gauge('Stamina', 50)

        self.fov = []

    @property
    def name(self):
        return self._name

    @property
    def hp(self):
        return self._hp

    @property
    def mp(self):
        return self._mp

    @property
    def stamina(self):
        return self._stamina

class Game:
    def __init__(self, player_name):
        self._zone = Zone('Test Zone', 2)
        self._level = self._zone.make_level(0)
        self._player = Player(player_name)
        self._turn_count = 0
        self._last_action_cost = 0

        spawn(self._player, self._level, *self._level.find_spawn())
        update_fov(self._player, self._level)

    @property
    def player(self):
        return self._player

    @property
    def level(self):
        return self._level

    @property
    def turn_count(self):
        return self._turn_count

    @property
    def last_action_cost(self):
        return self._last_action_cost

    def on_player_action(self, action):
        if action in MOVEMENT:
            dx, dy = MOVEMENT[action]
            move(self._player, self._player.x + dx, self._player.y + dy)

        update_fov(self._player, self._level)
