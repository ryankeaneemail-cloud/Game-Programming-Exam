using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float moveAreaLimit;

    public float detectPlayerRange;
    public float attackPlayerRange;

    public float stopDistance;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 targetPos;
    private Vector2 basePos;

    private float timer;
    private bool isMoving;

    private EnemyHealth health;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        basePos = transform.position;
        SetPatrolWaypoint();
    }
    // Update is called once per frame
    void Update()
    {
        EnemyPatrol();
    }

    private void FixedUpdate()
    {
        MovePhysics();
    }

    private void SetMovement()
    {
        timer -= Time.deltaTime;

        if (isMoving)
        {
            if (timer <= 0f)
            {
                isMoving = false;
                timer = Random.Range(0, 1);
            }
        }
        else
        {
            if (timer <= 0f)
            {
                SetPatrolWaypoint();
                isMoving = true;
                timer = Random.Range(0, 3);
            }
        }

        FlipSprite();
    }

    private void SetPatrolWaypoint()
    {
        float randomAxis = Random.Range(-moveAreaLimit, moveAreaLimit);
        targetPos = basePos + new Vector2(randomAxis, randomAxis);
    }

    private void FlipSprite()
    {
        Vector2 playerDirection = gameObject.transform.localScale;

        float xScale = Mathf.Abs(playerDirection.x);

        float flipDirection = targetPos.x - rb.position.x;

        if (flipDirection < 0)
        {
            playerDirection.x = -xScale;
        }
        else
        {
            playerDirection.x = xScale;
        }

        gameObject.transform.localScale = playerDirection;
    }

    private void MoveToWaypoint()
    {
        if (!isMoving) return;

        Vector2 dir = targetPos - rb.position;

        if (dir.magnitude <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = dir.normalized * moveSpeed;
    }
    void EnemyPatrol()
    {
        if (health.isDead) return;

        if (PlayerInChaseRange())
        {
            targetPos = player.position;
            FlipSprite();

            if (PlayerInAttackRange()) 
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }
            else
            {
                isMoving = true;
            }
        } else
        {
            SetMovement();
        }
    }

    void MovePhysics()
    {
        if (health.isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        animator.SetFloat("MoveSpeed", rb.velocity.magnitude);

        if (PlayerInChaseRange())
        {
            ChasePlayer();
        }
        else
        {
            MoveToWaypoint();
        }
    }

    bool PlayerInChaseRange()
    {
        return Vector2.Distance(rb.position, player.position) <= detectPlayerRange;
    }

    bool PlayerInAttackRange()
    {
        return Vector2.Distance(rb.position, player.position) <= attackPlayerRange;
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        if (PlayerInAttackRange())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = (Vector2)player.position - rb.position;

        if (dir.magnitude <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = dir.normalized * moveSpeed;
    }
}
