using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
    //components
    private Rigidbody enemyrb;
    private GameObject player;


    //enemy parameters
    [Header("Gravity")]
    [SerializeField] float gravityMultiplier;
    private float gravity = 9.81f;
    [Header("Range")]
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    [Header("Stats")]
    [SerializeField] float enemySpeed;
    [SerializeField] float jumpAttackH;
    private Vector3 moveDir;
    private Vector3 playerPosition;
    private float playerDistance;
    private Vector3 z;
    private Vector3 x;


    //wait parameters
    [Header("Wait Conditions")]


    //condition check parameters
    private RaycastHit hit;
    private CapsuleCollider col;
    [SerializeField] LayerMask groundMask;
    

    //condition parameters
    private bool alreadyJumpped;
    private bool jumpReset;
    private bool inAttackRange;
    private bool inSightRange;
    private bool waitBeforeAttack;
    private bool waitBeforeMove;
   


    void Start()
    {
        enemyrb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        col = GetComponent<CapsuleCollider>();
        gravity *= gravityMultiplier;
    }



    void Update()
    {
        playerDistance= (player.transform.position-transform.position).magnitude;
        MoveDirection();
    }



    void FixedUpdate()
    {
        onGround();
        if (!inSightRange&&!inAttackRange)
        {
            Debug.Log("Out of Sight");
            FindTarget();
        }
        if(inSightRange&&!inAttackRange)
        {
            LookAtTarget();
            Move();
        }
        if(inSightRange&&inAttackRange)
        {
            LookAtTarget();
            JumpAttack();
        }
    }



    private Vector3 MoveDirection()
    {
        x = Vector3.Cross(hit.normal, transform.forward).normalized;
        z = Vector3.Cross(hit.normal, transform.right).normalized;
        playerPosition = (player.transform.position - transform.position).normalized;
        moveDir = new Vector3(x.magnitude * playerPosition.x, 0, z.magnitude * playerPosition.z).normalized;
        moveDir.y = -hit.normal.y * 0.1f;
        return moveDir;
    }



    private void FindTarget()
    {
        
        if (playerDistance<=sightRange)
        {
            inSightRange = true;
        }
    }
 


    //look towards target
    private void LookAtTarget()
    {
        if (moveDir.x != 0 || moveDir.z != 0)
        {
            Quaternion rotateToTarget = Quaternion.LookRotation(moveDir, Vector3.up);
            enemyrb.rotation = Quaternion.RotateTowards(transform.rotation, rotateToTarget, 360 * Time.deltaTime);
        }
    }



    //Movement conditions
    private void Move()
    {
        waitBeforeAttack = true;
        if(onGround()&&!waitBeforeMove)
        {
            enemyrb.MovePosition(transform.position + (moveDir.normalized  * enemySpeed * Time.deltaTime));
        }
        else
        {
            enemyrb.AddForce(Vector3.down * gravity  * Time.deltaTime, ForceMode.Acceleration);
        }
        if(playerDistance>=sightRange)
        {
            inSightRange = false;
        }
        if(playerDistance<=attackRange)
        {
            inAttackRange = true;
            Invoke("WaitBeforeAttack", 1f);
        }
    }



    //Attack condition
    private void JumpAttack()
    {
        waitBeforeMove = true;
        if (onGround()&&!alreadyJumpped&&!jumpReset&&!waitBeforeAttack)
        {
            //scale x and z for precise jump
            enemyrb.AddForce(new Vector3((player.transform.position.x - transform.position.x)*1.5f, Mathf.Sqrt(2f * gravity * jumpAttackH)*Time.deltaTime,(player.transform.position.z - transform.position.z)*1.5f), ForceMode.Impulse);
            //enemyrb.AddForce(transform.up * Mathf.Sqrt(2f * gravity * jumpAttackH) * Time.deltaTime, ForceMode.Impulse);
            alreadyJumpped = true;
            jumpReset = true;
        }  
        else if(alreadyJumpped)
        {
            if(onGround())
            {
                // enemyrb.AddForce(-transform.up * Mathf.Sqrt(2f * gravity * jumpAttackH) * Time.deltaTime, ForceMode.Impulse);slides a bit
                enemyrb.velocity = Vector3.zero;//completely stops sliding
                alreadyJumpped = false;
                Invoke("JumpAttackReset", 2f);
            }
        }
        if(!onGround())
        {
            enemyrb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Acceleration);  
        }
        if(playerDistance>=attackRange)
        {
            inAttackRange = false;
            Invoke("WaitBeforeMove", 2f);
        }
    }



    //Wait Conditions
    private void JumpAttackReset()
    {
        jumpReset = false;
    }
    private void WaitBeforeAttack()
    {
        waitBeforeAttack = false;
    }
    private void WaitBeforeMove()
    {
        waitBeforeMove = false;
    }



    //Ground Check
    private bool onGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, col.height / 2 * 1.2f, groundMask))
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



    //Gizmos
    private void OnDrawGizmosSelected()
    {
        if (playerDistance < sightRange)
        {
            Gizmos.color=Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color=Color.white;
        Gizmos.DrawRay(transform.position, Vector3.down * (col.height/ 2) * 1.2f);
    }
}
