from .log import *

import pickle, pickletools

# TODO versioning, compatibility, etc

def do_save_game(save_path, game):
    temp_path = save_path.with_suffix('.tmp')
    log_info(f'Saving the game to {save_path}')

    with temp_path.open('wb') as fp:
        stream = pickle.dumps(game, protocol = 4)
        stream = pickletools.optimize(stream)
        fp.write(stream)

    try:
        save_path.unlink()
    except (OSError, IOError):
        pass

    temp_path.rename(save_path)

def do_load_game(save_path):
    log_info(f'Loading the game from {save_path}')
    with save_path.open('rb') as fp:
        return pickle.load(fp)
