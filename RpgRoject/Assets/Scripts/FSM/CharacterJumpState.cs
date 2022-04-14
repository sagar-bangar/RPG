using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{
    public CharacterJumpState(CharacterController currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubstates();
        _isRootState = true;
    }

    public override void EnterState()
    {
        _ctx._playerAnim.SetBool("isJumping", true); // jump animation transition
    }

    public override void ExitState()
    {
        _ctx._playerAnim.SetFloat("Jump", 1f, 0.1f, Time.deltaTime); // jump down animation
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
        Debug.Log("Jump State");
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        Jump();
        ApplyGravity();
    }

    public override void CheckSwitchState()
    {
        if(_ctx.OnGround())
        {
            SwitchState(_stateFactory.Grounded());
        }
    }
  
    private void Jump()
    {
        if (!_ctx._jumpReset && _ctx.OnGround())
        {
            _ctx._jumpReset = true;
            ResetJump();
            _ctx._playerAnim.SetFloat("Jump", 0, 0.1f, Time.deltaTime); // jump up animation
            _ctx._playerrb.AddForce(_ctx.transform.up * Mathf.Sqrt(2f * _ctx._gravity * _ctx._playerJumpHeight) * Time.deltaTime, ForceMode.Impulse); // jump
        }
    }

    private void ApplyGravity()
    {
        if(!_ctx.OnGround())
        {
            _ctx._playerAnim.SetFloat("Jump", 0.5f, 0.1f, Time.deltaTime); // idle animation
            if (_ctx._playerrb.velocity.y > 0) // while jumping
            {
                _ctx._playerrb.AddForce(-_ctx.transform.up * (_ctx._gravity * _ctx._jumpMultiplier) * Time.deltaTime, ForceMode.Acceleration); // apply while jumping Gravity (jumpmultiplier=0-1)
            }
            else if (_ctx._playerrb.velocity.y < 0) // while falling
            {
                _ctx._playerrb.AddForce(-_ctx.transform.up * (_ctx._gravity * _ctx._fallMultiplier) * Time.deltaTime, ForceMode.Acceleration); // apply while falling Gravity (fallmultipier=1-2)
            }
        }
    }

    private async void ResetJump()
    {
        await Task.Delay(1000); // mili sec 1000s^-3 = 1sec
        _ctx._jumpReset = false;
    }
}


//_ctx._playerrb.AddForce(_ctx._moveDir * Mathf.Sqrt(2f * _ctx._gravity * _ctx._playerJumpHeight) * Time.deltaTime, ForceMode.Impulse);//dash
