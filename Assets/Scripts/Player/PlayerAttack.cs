using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPos;
    public float attackRangeX;
    public float attackRangeY;
    public float startAttackInterval;
    public KeyCode attackKey;
    private float attackInterval;
    public LayerMask hitMask;

    public AudioClip attackClip;
    AudioSource attackSource;
    // Start is called before the first frame update
    void Start()
    {
        attackSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (attackInterval <= 0)
        {
            if (Input.GetKeyDown(attackKey))
            {
                Collider2D[] enemyCollider = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, hitMask);
                GetComponent<Animator>().SetTrigger("Attack");
                attackSource.PlayOneShot(attackClip);
                foreach (Collider2D enemyObject in enemyCollider)
                {
                    Debug.Log("HitEnemy");
                    Debug.Log(enemyObject.name);
                    if (enemyObject.tag == "Animal") 
                    {
                        Debug.Log("Attack Animal");
                        enemyObject.GetComponent<Animal>().Attacked();
                    }

                    if(enemyObject.tag == "Enemy")
                    {
                        enemyObject.GetComponent<EnemyHealth>().TakeDamage(transform.position);
                    }
                }
                attackInterval = startAttackInterval;
            }
        }
        else
        {
            attackInterval -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRangeX, attackRangeY));
    }
}
