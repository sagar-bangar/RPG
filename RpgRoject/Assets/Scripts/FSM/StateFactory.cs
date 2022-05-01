using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory 
{
    CharacterController _context;

    public StateFactory(CharacterController currentContext)
    {
        _context = currentContext;
    }

    public CharacterGroundedState Grounded()
    {
        return new CharacterGroundedState(_context, this);
    }

    public CharacterJumpState Jump()
    {
        return new CharacterJumpState(_context, this);
    }

    public CharacterMoveState Move()
    {
        return new CharacterMoveState(_context, this);
    }

    public CharacterIdleState Idle()
    {
        return new CharacterIdleState(_context, this);
    }

    public CharacterSlopeState Slope()
    {
        return new CharacterSlopeState(_context, this);
    }
}
