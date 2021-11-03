using System.Collections;
using System.Collections.Generic;

//Container for all enumerations so as to keep them in one place

//List of Action Types. Used as an ID for Action Buttons
public enum ACTION_BUTTON_LIST
{
    WAIT,
    INVENTORY,
    ATTACK,
    UNITS,
    OBJECTIVE,
    OPTIONS,
    SUSPEND,
    END_TURN,
    ATTACK_AND_MOVE
}

//List of Damage Types for use in Damage calculations
public enum DAMAGE_TYPE
{
    PHYSICAL,
    ENERGY,
    SUPPORT
}

//Player mode used in WorldStateInfo to track whether player is in battle, prebattle menu, or in between battle free roam
public enum PlayerMode
{
    FIELD_BATTLE,
    BASE_MAP
}


//Keeps track of which "team" the current turn is for
public enum PHASE_LIST
{
    PLAYER_PHASE,
    ENEMY_PHASE,
    OTHER_PHASE
}

//Used in BattleStateMachine to keep track
public enum BattleState
{
    START_STATE,
    SELECTING_UNIT,
    MOVE_UNIT,
    POST_MOVEMENT,
    CHANGE_PHASE,
    SELECT_ACTION,
    ATTACK_AND_MOVE,
    VIEWING_ENEMY,
    CHOOSE_ITEM,
    CHOOSE_TARGET,
    COUNTER_ATTACK,
    SECOND_ATTACK,
    POST_COMBAT,
    CLEANUP,
    ANIMATING,
    UNIT_DETAILS,
    WAIT,
    BATTLE_OVER,
    DEFAULT
}

//Defines a characters "team"
public enum CHARACTER_TYPE
{
    PLAYER,
    ENEMY,
    OTHER
}

public enum ITEM_TYPE
{
    PASSIVE,
    HEALING
}

public enum ENEMY_ACTIONS
{
    MOVE,
    ATTACK,
    WAIT
}

public enum AI_TYPE
{
    SEEKER,
    STANDARD,
    STATIONARY
}

public enum TILE_TYPE
{
    DEFAULT,
    FOREST
}

public enum RESULT_TYPE
{
    VICTORY,
    DEFEAT
}



