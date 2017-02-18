from bearlibterminal import terminal
from . import ui
from .data import data_glyph, data_tile
from .level import OutOfBounds
from .globals import *

class Scene:
    def on_enter(self):
        pass

    def on_exit(self):
        pass

    def react(self, event, *args):
        pass

    def draw(self):
        pass

class MainMenuScene(Scene):
    def __init__(self):
        self._menu = ui.Menu(
            ('New game', self._on_new_game),
            ('Continue', self._on_load_game, has_saved_game),
            ('Quit',     signal_exit),
        )

    def _on_new_game(self):
        set_next_scene(NewGameScene)

    def _on_load_game(self):
        load_game()
        set_next_scene(GameScene)

    def react(self, event, *args):
        action = self._menu.react(event, *args)
        if action is None:
            return

        if action[0] == ACTION_CANCEL:
            signal_exit()
        elif action[0] == ACTION_CONFIRM:
            action[1]()

    def draw(self):
        terminal.print(0, 6, f'{get_game_display_name()}', width = get_grid_width(), align = terminal.TK_ALIGN_CENTER)
        terminal.print(0, 7, f'[color=dark grey]{get_game_version()}', width = get_grid_width(), align = terminal.TK_ALIGN_CENTER)
        menu_x = get_grid_width() // 2 - self._menu.width // 2 - 1
        self._menu.draw(menu_x, 12)

class NewGameScene(Scene):
    def __init__(self):
        self._player_name = ui.Input(x = 25, y = 1, default = 'Player', max_width = WIDTH_PLAYER_NAME)
        self._menu = ui.Menu(
           ('Start the game', self._on_start_game),
           ('Change name',    self._player_name.focus),
           ('Randomise name', self._on_random_name, False),
           ('Go back',        self._on_back),
        )

    def _on_random_name(self):
        pass

    def _on_start_game(self):
        if has_saved_game():
            if not ui.modal_confirm('[color=red]WARNING:[/color] This will overwrite the existing save. Are you sure?'):
                return

        new_game(self._player_name.value)
        set_next_scene(GameScene)

    def _on_back(self):
        set_next_scene(MainMenuScene)

    def react(self, event, *args):
        action = self._menu.react(event, *args)
        if action is None:
            return

        if action[0] == ACTION_CANCEL:
            self._on_back()
        elif action[0] == ACTION_CONFIRM:
            action[1]()

    def draw(self):
        terminal.print(4, 1, 'Character name:')
        self._player_name.draw()
        self._menu.draw(1, 10)

class GameScene(Scene):
    def __init__(self):
        pass

    def react(self, event, *args):
        if event != EVENT_KEY:
            return

        action = get_game_keymap().query(args[0])

        if action == ACTION_QUIT and ui.modal_confirm('Quit without saving? [color=red]NOTE[/color]: this will discard existing save!'):
            discard_game()
            signal_exit()
        elif action == ACTION_SAVE_QUIT and ui.modal_confirm('Save and quit?'):
            signal_exit()
        else:
            this_game().on_player_action(action)

    def _draw_sidebar(self, player, level):
        ui.vline(WIDTH_SIDEBAR, 0, get_grid_height())
        ui.hline(WIDTH_SIDEBAR, get_grid_height() - HEIGHT_MESSAGES, get_grid_width() - WIDTH_SIDEBAR)
        terminal.put(WIDTH_SIDEBAR, get_grid_height() - HEIGHT_MESSAGES, ui.BOX_VLINE_L_SPLIT)

        gauge_x = WIDTH_SIDEBAR - WIDTH_SIDEBAR_GAUGES - 1

        with ui.foreground('grey'):
            terminal.print(2, 1, f'{player.name}', align = terminal.TK_ALIGN_CENTER, width = WIDTH_PLAYER_NAME)
            ui.sidebar_gauge(gauge_x, 3, 'HP', player.hp, 'dark green', 'red', 'light green')
            ui.sidebar_gauge(gauge_x, 4, 'MP', player.mp, 'blue', 'light blue', 'light blue')
            ui.sidebar_gauge(gauge_x, 5, 'ST', player.stamina, 'dark yellow', 'light yellow', 'light yellow')

    def _draw_boss_health(self):
        # TODO
        #terminal.print(WIDTH_SIDEBAR + 2, 1, 'Boss Name', align = terminal.TK_ALIGN_CENTER, width = WIDTH_VIEWPORT)
        #terminal.print(WIDTH_SIDEBAR + 2, 2, '[color=dark red]Boss Tagline[/color]', align = terminal.TK_ALIGN_CENTER, width = WIDTH_VIEWPORT)
        #from .game import Gauge
        #ui.gauge(WIDTH_SIDEBAR + 2, 3, WIDTH_VIEWPORT, Gauge('Boss', 1234), 'red', 'light red', 'light red')
        pass

    def _draw_viewport(self, player, level):
        def go(player_c, view_c, map_c):
            if player_c < (view_c // 2):
                return 0
            elif player_c >= map_c - (view_c // 2):
                return map_c - view_c
            else:
                return player_c - (view_c // 2)

        if level.grid.width > WIDTH_VIEWPORT:
            x0 = go(player.x, WIDTH_VIEWPORT, level.grid.width)
        else:
            x0 = 0

        if level.grid.height > HEIGHT_VIEWPORT:
            y0 = go(player.y, HEIGHT_VIEWPORT, level.grid.height)
        else:
            y0 = 0

        for y in range(0, HEIGHT_VIEWPORT):
            for x in range(0, WIDTH_VIEWPORT):
                cx = WIDTH_SIDEBAR + 2 + x
                cy = HEIGHT_BOSS_METER + 2 + y

                try:
                    tile = level.grid[x0 + x, y0 + y]
                except OutOfBounds:
                    ui.put_glyph(cx, cy, TILE_WALL_DEEP)
                    continue

                glyph  = data_glyph(tile.type)
                fg, bg = glyph.fg, glyph.bg

                if not tile.is_lit:
                    unlit = data_glyph(TILE_UNLIT)

                    if fg is not None:
                        fg = unlit.fg
                    if bg is not None and bg != 'black':
                        bg = unlit.bg

                if tile.was_seen:
                    ui.put_glyph(cx, cy, tile.type, fg, bg)

                if tile.is_lit:
                    if tile.monster is not None:
                        ui.put_glyph(cx, cy, tile.monster.type)
                    elif tile.items:
                        if len(tile.items) == 1:
                            ui.put_glyph(cx, cy, tile.items[0])
                        else:
                            ui.put_glyph(cx, cy, ITEM_MULTIPLE)

    def draw(self):
        game   = this_game()
        player = game.player
        level  = game.level

        self._draw_sidebar(player, level)
        self._draw_boss_health()
        self._draw_viewport(player, level)

        #for tile in level.grid.cells:
        #    glyph = data_glyph(tile.type)
        #    with ui.colors(glyph.fg, glyph.bg):
        #        terminal.put(tile.x, tile.y, glyph.glyph)
