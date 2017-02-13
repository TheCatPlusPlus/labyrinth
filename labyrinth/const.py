EVENT_CLOSE  = 'window-closing'
EVENT_RESIZE = 'window-resized'
EVENT_MOUSE  = 'mouse-event'
EVENT_SCROLL = 'scroll-event'
EVENT_KEY    = 'key-event'

ACTION_PREVIOUS_ITEM = 'previous-item'
ACTION_NEXT_ITEM     = 'next-item'
ACTION_CANCEL        = 'cancel'
ACTION_CONFIRM       = 'confirm'
ACTION_QUIT          = 'quit'
ACTION_SAVE_QUIT     = 'save-quit'

ACTION_TAKE_STAIRS     = 'take-stairs'
ACTION_MOVE_NORTH      = 'move-north'
ACTION_MOVE_SOUTH      = 'move-south'
ACTION_MOVE_WEST       = 'move-west'
ACTION_MOVE_EAST       = 'move-east'
ACTION_MOVE_NORTH_WEST = 'move-north-west'
ACTION_MOVE_SOUTH_WEST = 'move-south-west'
ACTION_MOVE_NORTH_EAST = 'move-north-east'
ACTION_MOVE_SOUTH_EAST = 'move-south-east'
ACTION_WAIT            = 'wait'

ACTION_SHOW_INVENTORY = 'show-inventory'
ACTION_SHOW_CHARACTER = 'show-character'

WIDTH_SIDEBAR     = 30
WIDTH_PLAYER_NAME = WIDTH_SIDEBAR - 6
WIDTH_GAUGES      = 10
HEIGHT_MESSAGES   = 6

SPEED_BASE     = 100 # baseline: one turn
MOVE_COST_BASE = SPEED_BASE

TILE_GROUND      = 't:ground'
TILE_WALL        = 't:wall'
TILE_WALL_DEEP   = 't:wall-deep' # wall that's surrounded by other walls on all sides
TILE_DOOR_CLOSED = 't:door-closed'
TILE_DOOR_OPENED = 't:door-opened'
TILE_STAIRS_UP   = 't:stairs-up'
TILE_STAIRS_DOWN = 't:stairs-down'

MONSTER_PLAYER  = 'm:player'
MONSTER_RAT     = 'm:rat'
MONSTER_BIG_RAT = 'm:big-rat'

DUNGEN_ROOM_TRIES             = 60
DUNGEN_ROOM_POSITION_TRIES    = 30
DUNGEN_EXTRA_CONNECTOR_CHANCE = 0.25
DUNGEN_WINDING_PERCENT        = 0.3
DUNGEN_DOOR_CHANCE            = 0.75
DUNGEN_DOOR_OPEN_CHANCE       = 0.2

NORTH = (0, -1)
SOUTH = (0, +1)
WEST  = (-1, 0)
EAST  = (+1, 0)

CARDINALS = (NORTH, SOUTH, WEST, EAST)
