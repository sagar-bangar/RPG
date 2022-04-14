using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class Player_Behaviour : MonoBehaviour
{
    private Rigidbody playerrb;
    public GameObject playerc;
    private Animator playerAnim;
    private Transform child;

    //movement
    private Vector3 moveDir;
    private bool addingCounterJump;
    private bool jumpReset;
    [Header("Character Parameters")]
    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float playerJumpHeight;
    [SerializeField] float rotationSpeed;


    //cam orientation 
    [Header("Camera Forward Direction")]
    [SerializeField] Transform camOrentiation;


    //Physics stuff
    [Header("Gravity")]
    private float gravity = 9.86f;
    [SerializeField] float gravityMultiplier;
    [SerializeField] LayerMask groundMask;


    //slope ray
    [Header("Slope Parameters")]
    private RaycastHit hit;


    //sphere ray
    [SerializeField] float downrayCastdistance;
    [SerializeField] float sphereCastdistance;
    public CapsuleCollider col;
    private RaycastHit sphereCastHit;


    private void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        child = transform.GetChild(0).GetComponent<Transform>();
        gravity *= gravityMultiplier;
        addingCounterJump = false;
    }


    private void Update()
    {
        playerc.GetComponent<Renderer>().material.SetColor("_Color", OnGround() ? Color.black : Color.white);
        VPlayerInput();
        HPlayerInput();
        MoveDirection();
        child.localPosition = Vector3.zero;
    }


    private void FixedUpdate()
    {
        OnSlope();
        OnGround();
        Move();
        Jump();
    }


    //Slope Checking
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position+col.center, Vector3.down, out hit, downrayCastdistance, groundMask))
        {
            if (hit.normal != Vector3.up)
            {
                Debug.Log("On slope");
                return true;
            }
            else
            {
                Debug.Log("!On slope");
                return false;
            }
        }
        return false;
    }



    //Groundchecking  
    private bool OnGround()
    {
        if (Physics.SphereCast(transform.position+col.center, col.radius, -transform.up, out sphereCastHit, sphereCastdistance, groundMask))
        {
            Debug.Log("On Ground");
            return true;
        }
        else
        {
            Debug.Log("Not On Ground");
            return false;
        }
    }


    //Gizmos for cast
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position+col.center + (-transform.up * sphereCastdistance), col.radius);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position+col.center, (Vector3.down * (downrayCastdistance)));
    }


    //Inputs for the player
    private int HPlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
            return 1;
        if (Input.GetKey(KeyCode.A))
            return -1;
        else return 0;
    }
    private int VPlayerInput()
    {
        if (Input.GetKey(KeyCode.W))
            return 1;
        if (Input.GetKey(KeyCode.S))
            return -1;
        else return 0;
    }


    //move direction for player in this case moves in forward direction of camera using transform component
    private Vector3 MoveDirection()
    {
        if(OnGround())
        {
            Vector3 normal = (hit.normal);
            Vector3 z = Vector3.Cross(normal, transform.right).normalized;
            Vector3 x = Vector3.Cross(normal, transform.forward).normalized;
            moveDir = new Vector3(x.magnitude * HPlayerInput(), 0, z.magnitude * VPlayerInput());
            moveDir = moveDir.z * camOrentiation.forward + moveDir.x * camOrentiation.right;
            if (hit.normal.y > 0.5)//60deg wit transfor.up cos60=0.5
            {
                moveDir.y = -hit.normal.y * 0.00001f;//dot for lesser roataion than hit.normal
            }
            else//hit.normal=mgcos0 
            {
                moveDir.y = hit.normal.y; //hit.normal=1 which is 90deg with transform.up cos90=1
            }
            return moveDir;
        }  
        else
        {
            moveDir = new Vector3(transform.forward.magnitude * HPlayerInput(), 0 ,transform.right.magnitude * VPlayerInput());
            moveDir = moveDir.z * camOrentiation.forward + moveDir.x * camOrentiation.right;
            return moveDir;
        }
   
    }


    //movement 
    private void Move()
    {
        if (OnGround())
        {
            if (moveDir.x != 0 || moveDir.z != 0)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = runSpeed;
                    playerAnim.SetFloat("Speed", 1f,0.1f,Time.deltaTime);
                }
                else
                {
                    moveSpeed = walkSpeed;
                    playerAnim.SetFloat("Speed", 0.75f,0.1f, Time.deltaTime);
                }
                playerrb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                playerAnim.SetFloat("Speed",0.5f, 0.1f, Time.deltaTime);
                playerrb.velocity = Vector3.zero;
            }
            if (moveDir.x != 0 || moveDir.z != 0)//rotation on slope
            {
                Quaternion rotateTowards = Quaternion.LookRotation(moveDir, Vector3.up);
                //transform.rotation = Quaternion.Euler(0, rotateTowards.eulerAngles.y, 0);
                transform.localRotation = Quaternion.Slerp(transform.rotation, rotateTowards, rotationSpeed*Time.deltaTime);
            }
        }
        if(!OnGround())
        {
            playerrb.AddForce(-transform.up * gravity * Time.deltaTime, ForceMode.Acceleration);
        }
    }


    //Jump force
    private async void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && OnGround() && !addingCounterJump && !jumpReset)
        {
            playerrb.AddForce(transform.up * Mathf.Sqrt(2f * gravity * playerJumpHeight) * Time.deltaTime, ForceMode.Impulse);//jump
            playerAnim.SetTrigger("Jump");
            addingCounterJump = true;
            jumpReset = true;
        }
        else if (addingCounterJump)//prevent from sliding
        {
            if (OnGround())
            {
                //playerrb.AddForce(-transform.up * Mathf.Sqrt(2f * gravity * playerJumpHeight) * 0.8f * Time.deltaTime, ForceMode.Impulse);//counterJump
                playerrb.velocity = Vector3.zero;
                addingCounterJump = false;
                //Invoke("JumpReset", 0.5f);//wait and jump after 0.5sec
                await Task.Delay(500);//mili sec
                jumpReset = false;
            }
        }
    }
    
    private  void JumpReset()
    {
        jumpReset = false;
    }
}