public abstract class CharacterBaseState 
{
    protected bool _isRootState=false;
    protected CharacterController _ctx;
    protected StateFactory _stateFactory;
    protected CharacterBaseState _currentSuperState;
    protected CharacterBaseState _currentSubState;

    public CharacterBaseState(CharacterController currentContext,StateFactory stateFactory)
    {
        _ctx = currentContext;
        _stateFactory = stateFactory;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubstates();

   public void UpdateStates()
    {
        UpdateState();
        if(_currentSubState!=null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void FixedUpdates()
    {
        FixedUpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdates();
        }
    }

    protected void SwitchState(CharacterBaseState newState)
    {
        ExitState();
        newState.EnterState();
        if(_isRootState)
        {
            _ctx._currentState = newState;
        }
        else if(_currentSuperState!=null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }
    
    protected void SetSuperState(CharacterBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }


    protected void SetSubState(CharacterBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
