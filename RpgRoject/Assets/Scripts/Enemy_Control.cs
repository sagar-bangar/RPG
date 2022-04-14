using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Control : MonoBehaviour
{
    private Rigidbody enemyrb;
    private GameObject player;
    private bool inSightRange;
    private bool inAttackRange;
    private bool isOnGround;
    private bool alredyAttacked;
    private bool isInAttackPosition;
    [SerializeField] float sightDistance;

    [SerializeField] float attackDistnce;
   
    [SerializeField] float enemySpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float attackResetTime;
    void Start()
    {
        enemyrb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }
 
    private void FixedUpdate()
    {
        float distanceFromPlayer = (player.transform.position - transform.position).sqrMagnitude;
        //Debug.Log("Distance: " + distanceFromPlayer);
        Vector3 playerPosition = player.transform.position - transform.position;
        if (!inSightRange && !inAttackRange)
        {
            FindTarget(distanceFromPlayer);
        }
        if (inSightRange && !inAttackRange)
        {
            ChaseTarget(distanceFromPlayer, playerPosition);
        }
        if (inSightRange && inAttackRange)
        {
            AttackTarget(distanceFromPlayer, playerPosition);
        }
    }
    private void FindTarget(float distanceFromPlayer)
    {

        if (distanceFromPlayer <= sightDistance)
        {
            Debug.Log("In Sight");
            inSightRange = true;
        }

    }
    private void ChaseTarget(float distanceFromPlayer, Vector3 playerPosition)
    {
        Debug.Log("Moving");
        enemyrb.MovePosition(transform.position + (playerPosition.normalized * enemySpeed * Time.deltaTime));
        if (distanceFromPlayer >= sightDistance)
        {
            Debug.Log("Out Of Sight");
            inSightRange = false;
        }
        if (distanceFromPlayer <= attackDistnce)
        {
            inAttackRange = true;
            Invoke("MovetoJump", attackResetTime);
        }
    }
    private void AttackTarget(float distanceFromPlayer, Vector3 playerPosition)
    {

        if (isOnGround && !alredyAttacked && isInAttackPosition )
        {
            enemyrb.AddForce(new Vector3(player.transform.position.x-transform.position.x, jumpHeight * Time.deltaTime, player.transform.position.z - transform.position.z), ForceMode.Impulse);
            isOnGround = false;
            alredyAttacked = true;
            Invoke("ResetAttack", attackResetTime);
        }
        if (distanceFromPlayer >= attackDistnce)
        {
            inAttackRange = false;
            isInAttackPosition = false;
        }
    }
    private void ResetAttack()
    {
        alredyAttacked = false;
    }
    private void MovetoJump()
    {
        isInAttackPosition = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isOnGround = true;
        }
    }
}


















