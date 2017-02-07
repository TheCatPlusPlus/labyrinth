#!/usr/bin/env python3.6
import time
from bearlibterminal import terminal

from .globals import *
from .log import setup_log
from .input import read_event, has_event, parse_key
from .scenes import *

def main():
    setup_log()
    log_info(f'User data path: {get_user_data_path()}')

    load_keymaps()

    config = f'''
        window: title={get_game_display_name()}, size={get_grid_width()}x{get_grid_height()};
        input: precise-mouse=false, filter=[keyboard, system];
        log: level=fatal;
    '''

    terminal.open()
    terminal.set(config)
    terminal.refresh()

    set_next_scene(MainMenuScene)

    while is_running():
        scene = this_scene()

        if not has_event():
            time.sleep(0.05)
        else:
            event = read_event()

            if event is not None:
                event_type, event_args = event[0], event[1:]

                #if event_args:
                #    event_args = ', '.join(str(x) for x in event_args)
                #    log_debug(f'Event: {event_type}: {event_args}')
                #else:
                #    log_debug(f'Event: {event_type}')

                if event[0] == EVENT_CLOSE or event[0] == EVENT_KEY and event[1] == parse_key('alt f4'):
                    signal_exit()
                else:
                    scene.react(event[0], *event[1:])

        terminal.clear()
        scene.draw()
        terminal.refresh()

    if is_game_loaded():
        save_game()

main()
