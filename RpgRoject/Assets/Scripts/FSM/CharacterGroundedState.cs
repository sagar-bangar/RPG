using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedState : CharacterBaseState
{
    public CharacterGroundedState(CharacterController currentContext,StateFactory stateFactory) : base(currentContext, stateFactory) 
    {
        InitializeSubstates();
        _isRootState = true;
    }

    public override void EnterState()
    {
        _ctx._playerAnim.SetBool("isJumping", false);
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubstates()
    {
        if (_ctx.IsMoving())
        {
            SetSubState(_stateFactory.Move());
        }
        else if (!_ctx.IsMoving())
        {
            SetSubState(_stateFactory.Idle());
        }
    }

    public override void UpdateState()
    {
        Debug.Log("gorundedState"); 
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        ApplyGravity();
        NegateSliding();
    }

    public override void CheckSwitchState()
    {
        if(Input.GetKey(KeyCode.Space) && !_ctx._jumpReset && _ctx.OnGround())
        {
            SwitchState(_stateFactory.Jump());
        }
    }

    private void ApplyGravity()
    {
        if(!_ctx.OnGround())
        {
            _ctx._playerrb.AddForce(-_ctx.transform.up * _ctx._gravity * Time.deltaTime, ForceMode.Acceleration); // Apply while on ground Gravity
        }
    }

    private void NegateSliding()
    {
        if(_ctx.IsFalling())
        {
            if(_ctx.OnGround())
            {
                _ctx._playerrb.velocity = Vector3.zero;
            }
        }
    }
}
// Make something such that whenever player falls on ground he should not slip
// Slipping counter not working
