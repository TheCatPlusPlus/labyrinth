_g_logger       = None
_g_show_verbose = True

def setup_log():
    import logging, logging.config
    import colorlog
    import colorama

    global _g_logger

    colorama.init()
    logging.config.dictConfig({
        'version': 1,
        'formatters': {
            'colors': {
                '()': 'colorlog.ColoredFormatter',
                'format': '%(log_color)s[%(asctime)s] [%(levelname)s] [%(name)s] %(message)s',
                'datefmt': '%H:%M:%S',
                'log_colors': {
                    'DEBUG':    'bold_black',
                    'INFO':     'white',
                    'WARNING':  'yellow',
                    'ERROR':    'bold_red',
                    'CRITICAL': 'bold_red',
                }
            }
        },
        'handlers': {
            'console': {
                'class': 'colorlog.StreamHandler',
                'formatter': 'colors',
            }
        },
        'root': {
            'level': 'DEBUG',
            'handlers': ['console'],
        },
    })

    _g_logger = logging.getLogger('game')

def log_verbose(msg, *args, **kwargs):
    if not _g_show_verbose:
        return

    log_debug(msg, *args, **kwargs)

def log_debug(msg, *args, **kwargs):
    _g_logger.debug(msg, *args, **kwargs)

def log_info(msg, *args, **kwargs):
    _g_logger.info(msg, *args, **kwargs)

def log_warning(msg, *args, **kwargs):
    _g_logger.warning(msg, *args, **kwargs)

def log_error(msg, *args, **kwargs):
    _g_logger.error(msg, *args, **kwargs)

__all__ = ['log_verbose', 'log_debug', 'log_info', 'log_warning', 'log_error']
