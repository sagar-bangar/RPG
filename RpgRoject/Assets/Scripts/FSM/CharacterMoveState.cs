using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{
    public CharacterMoveState(CharacterController currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
        Debug.Log("MoveState");
        CheckSwitchState();
        MoveDirection();
    }

    public override void FixedUpdateState()
    {
        Move();
        Rotate();
    }

    public override void CheckSwitchState()
    {
        if (_ctx.HPlayerInput() == 0 && _ctx.VPlayerInput() == 0)
        {
            SwitchState(_stateFactory.Idle());
        }
    }

    // Move direction for player in this case moves in forward direction of camera using transform component
    private Vector3 MoveDirection()
    {
        if (_ctx.OnGround() && _ctx.OnSlope())
        {
            Vector3 z = Vector3.Cross((_ctx._hit.normal.y *_ctx.transform.forward), _ctx.transform.right).normalized;
            Vector3 x = Vector3.Cross((_ctx._hit.normal.y * _ctx.transform.right), _ctx.transform.forward).normalized;
            _ctx._moveDir = new Vector3(x.magnitude * _ctx.HPlayerInput(), 0, z.magnitude * _ctx.VPlayerInput());
            _ctx._moveDir = _ctx._moveDir.z * _ctx._camOrentiation.forward + _ctx._moveDir.x * _ctx._camOrentiation.right;
            if (_ctx._hit.normal.y > 0.7f) // 60deg wit transfor.up cos60=0.5 >=60 && <90 _ctx._hit.normal.y >= 0.5f && _ctx._hit.normal.y < 1f
            {
                Debug.Log("Slope:60>");
                _ctx._moveDir.y = _ctx._hit.normal.y * 0.00001f; // Dot for lesser roataion than hit.normal
            }
            else if(_ctx._hit.normal.y<0.7f)
            {
                Debug.Log("Slope:60<");
                Vector3 stepSlopeMove = Vector3.up - _ctx._hit.normal * (Vector3.Dot(Vector3.up, _ctx._hit.normal));
                _ctx._moveDir = stepSlopeMove * -_ctx._runSpeed ;
                _ctx._moveDir.y = _ctx._hit.normal.y * 0.00001f;

            }
            else // Hit.normal=mgcos0 
            {
                Debug.Log("Slope:0");
                _ctx._moveDir.y = _ctx.transform.position.y * 0.00001f; // Hit.normal=1 which is 90deg with transform.up cos90=1
            }
            return _ctx._moveDir;
        }
        else
        {
            _ctx._moveDir = new Vector3(_ctx.transform.forward.magnitude * _ctx.HPlayerInput(), 0, _ctx.transform.right.magnitude * _ctx.VPlayerInput());
            _ctx._moveDir = _ctx._moveDir.z * _ctx._camOrentiation.forward + _ctx._moveDir.x * _ctx._camOrentiation.right;
            return _ctx._moveDir;
        }
    }
    
    // Apply force for Movement
    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _ctx._moveSpeed = _ctx._runSpeed;
            _ctx._playerAnim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            _ctx._moveSpeed = _ctx._walkSpeed;
            _ctx._playerAnim.SetFloat("Speed", 0.75f, 0.1f, Time.deltaTime);
        }
        if(_ctx.OnGround())
        {
            _ctx._playerrb.AddForce(_ctx._moveDir.normalized * _ctx._moveSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            _ctx._playerrb.AddForce(_ctx._moveDir.normalized * _ctx._runSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    // Apply RotationForce
    private void Rotate()
    {
        if (_ctx._moveDir.x != 0 || _ctx._moveDir.z != 0) // so that lookrotation is not set to zero
        {
            Quaternion rotateTowards = Quaternion.LookRotation(_ctx._moveDir, Vector3.up);
            //transform.rotation = Quaternion.Euler(0, rotateTowards.eulerAngles.y, 0);
            _ctx.transform.localRotation = Quaternion.Slerp(_ctx.transform.rotation, rotateTowards, _ctx._rotationSpeed * Time.deltaTime);
        }
    }
}
