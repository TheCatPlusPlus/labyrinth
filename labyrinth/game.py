from .globals import *

class PlayerGauge:
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

class Player:
    def __init__(self, name):
        self._name = name
        self._hp = PlayerGauge('HP', 100)
        self._mp = PlayerGauge('MP', 100)
        self._stamina = PlayerGauge('Stamina', 50)

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
        self._player = Player(player_name)

    @property
    def player(self):
        return self._player

    def on_player_action(self, action):
        pass
