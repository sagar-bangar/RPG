using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Context Parameters
    public CharacterBaseState _currentState;
    StateFactory _states;

    // Character RigidBody
    public  Rigidbody _playerrb;

    // Physics Gravity stuff
    [Header("Gravity")]
    public float _gravity = 9.86f;
    [SerializeField] public float _gravityMultiplier;
    [Header("JumpMultiplyer")]
    [SerializeField] public float _jumpMultiplier;
    [SerializeField] public float _fallMultiplier;
    [SerializeField] public LayerMask _groundMask;

    // Jump
    [Header("Jump")]
    [SerializeField] public float _playerJumpHeight;
    public bool _jumpReset;

    // Physics sphere ray
    [Header("Raycasting")]
    [SerializeField] public float _sphereCastDistance;
    [SerializeField] float _downRayCastdistance;
    public CapsuleCollider _col;
    public RaycastHit _sphereCastHit;
    public RaycastHit _hit;

    // Movemnet
    [Header("Movement")]
    public Vector3 _moveDir;
    public float _moveSpeed;
    [SerializeField] public float _walkSpeed;
    [SerializeField] public float _runSpeed;
    [SerializeField] public float _rotationSpeed;

    // Camera orientation 
    [Header("Camera Forward Direction")]
    [SerializeField] public Transform _camOrentiation;

    // Animator
    [Header("Animator Component")]
    public Animator _playerAnim;

    void Awake()
    {
        _states = new StateFactory(this);
        _currentState = _states.Grounded();
        _gravity *= _gravityMultiplier;
        _playerAnim = GetComponentInChildren<Animator>();
        _playerrb.freezeRotation = true;
    }

    void Start()
    {
        _currentState.EnterState(); 
    }

    void Update()
    {
        _currentState.UpdateStates();
        HPlayerInput();
        VPlayerInput();
        IsMoving();
        IsFalling();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdates();
        OnGround();
        OnSlope();
    }

    // Gizmos for cast
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + _col.center + (-transform.up * _sphereCastDistance), _col.radius);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position + _col.center, (Vector3.down * (_downRayCastdistance)));
    }

    // GroundChecking
    public bool OnGround()
    {
        if (Physics.SphereCast(transform.position + _col.center, _col.radius, -transform.up, out _sphereCastHit, _sphereCastDistance, _groundMask))
        {
            //Debug.Log("On Ground");
            return true;
        }
        else
        {
            //Debug.Log("Not On Ground");
            return false;
        }
    }

    // Slope Checking
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position + _col.center, Vector3.down, out _hit, _downRayCastdistance, _groundMask))
        {
            float angle = Vector3.Dot(Vector3.up, _hit.normal);
            Debug.Log("angle: "+angle);
            if (_hit.normal != Vector3.up)
            {
                //Debug.Log("On slope");
                return true;
            }
            else
            {
                //Debug.Log("!On slope");
                return false;
            }
        }
        return false;
    }

    // Inputs for the player
    public int HPlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
            return 1;
        else if (Input.GetKey(KeyCode.A))
            return -1;
        else return 0;
    }
    public int VPlayerInput()
    {
        if (Input.GetKey(KeyCode.W))
            return 1;
        else if (Input.GetKey(KeyCode.S))
            return -1;
        else return 0;
    }

    // Movement check
    public bool IsMoving()
    {
        if(HPlayerInput()==0 && VPlayerInput()==0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Falling check
    public bool IsFalling()
    {
        if(!OnGround() && _playerrb.velocity.y<0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
