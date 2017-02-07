from bearlibterminal import terminal
from . import ui
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

    def draw(self):
        ui.vline(WIDTH_SIDEBAR, 0, get_grid_height())
        ui.hline(WIDTH_SIDEBAR, get_grid_height() - HEIGHT_MESSAGES, get_grid_width() - WIDTH_SIDEBAR)
        terminal.put(WIDTH_SIDEBAR, get_grid_height() - HEIGHT_MESSAGES, ui.BOX_VLINE_L_SPLIT)

        player = this_game().player

        with ui.foreground('grey'):
            terminal.print(3, 1, f'{player.name}', align = terminal.TK_ALIGN_CENTER, width = WIDTH_PLAYER_NAME)
            ui.gauge(3, 'HP', player.hp, 'dark green', 'red', 'light green')
            ui.gauge(4, 'MP', player.mp, 'blue', 'light blue', 'light blue')
            ui.gauge(5, 'ST', player.stamina, 'dark yellow', 'light yellow', 'light yellow')
