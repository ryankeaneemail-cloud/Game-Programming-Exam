using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackInterval;
    public Transform attackPos;
    public float attackRangeX;
    public float attackRangeY;
    private float timer;
    private bool isAttacking;
    public LayerMask hitMask;

    private EnemyMovement movement;
    private EnemyHealth health;
    private Animator animator;

    private Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        movement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if(health.isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > movement.attackPlayerRange) return;

        if (timer <= 0)
        {
                Collider2D[] playerCollider = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, hitMask);
                animator.SetTrigger("Attack");

                foreach (Collider2D playerObject in playerCollider)
                {
                    Debug.Log("HitPlayer");
                    Debug.Log(playerObject.name);

                    if (playerObject.tag == "Player")
                    {
                        playerObject.GetComponent<PlayerMovement>().PlayerHurt(transform.position);
                    }
                }
                timer = attackInterval;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRangeX, attackRangeY));
    }
}
