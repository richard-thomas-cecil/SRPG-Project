using System.Collections;
using System.Collections.Generic;

//Container for all enumerations so as to keep them in one place

//List of Action Types. Used as an ID for Action Buttons
public enum ACTION_BUTTON_LIST
{
    WAIT,
    INVENTORY,
    ATTACK
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
    FIELD_BATTLE
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
    SELECTING_UNIT,
    MOVE_UNIT,
    CHANGE_PHASE,
    SELECT_ACTION,
    VIEWING_ENEMY,
    WAIT
}

//Defines a characters "team"
public enum CHARACTER_TYPE
{
    PLAYER,
    ENEMY,
    OTHER
}
