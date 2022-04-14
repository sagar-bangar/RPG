using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{
    public CharacterIdleState(CharacterController currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {

    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void InitializeSubstates()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("IdleState");
        CheckSwitchState();
        Idle();
    }

    public override void FixedUpdateState()
    {
       
    }

    public override void CheckSwitchState()
    {
        if (_ctx.HPlayerInput() != 0 || _ctx.VPlayerInput() != 0)
        {
            SwitchState(_stateFactory.Move());
        }
    }

    private void Idle()
    {
        _ctx._playerAnim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        if(_ctx.OnGround())
        {
            _ctx._playerrb.velocity = Vector3.zero;
        }
    }
}
