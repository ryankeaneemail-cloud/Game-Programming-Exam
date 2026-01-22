using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public bool isDead;

    public int maxHP;
    public int currentHP;

    public float knockbackForce;

    public float respawnTime;
    private float currentRespawnTime;

    Collider2D enemyCollider;
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        currentHP = maxHP;
        currentRespawnTime = respawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHP();
    }

    void CheckHP()
    {
        if (!isDead && currentHP == 0)
        {
            isDead = true;
            enemyCollider.enabled = false;
            animator.SetTrigger("Dead");
            currentRespawnTime = respawnTime;
        }

        if (isDead)
        {
            currentRespawnTime -= Time.deltaTime;

            if (currentRespawnTime <= 0) 
            {
                isDead = false;
                currentHP = maxHP;
                enemyCollider.enabled = true;
                animator.SetTrigger("Revived");
            }

        }
    }

    public void TakeDamage(Vector2 playerPos)
    {
        if (isDead) return;

        currentHP -= 1;

        animator.SetTrigger("Hurt");

        Vector2 dir = (rb.position - playerPos).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }
}
