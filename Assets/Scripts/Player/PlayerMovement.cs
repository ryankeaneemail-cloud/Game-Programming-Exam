using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;

    public float moveSpeed;
    public float knockbackForce;

    Rigidbody2D rb;
    Vector2 movement;

    [HideInInspector]
    public bool isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        MoveInput();
        FaceInput();
    }

    void FixedUpdate()
    {
        MovementPhysics();
    }

    void MovementPhysics()
    {
        rb.velocity = movement * moveSpeed;
        playerAnim.SetFloat("MoveSpeed", rb.velocity.magnitude);
    }

    void MoveInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FaceInput()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput < 0 && !isFacingRight)
        {
            Flip();
        }
        if (moveInput > 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector2 playerDirection = gameObject.transform.localScale;
        playerDirection.x *= -1;
        gameObject.transform.localScale = playerDirection;
        isFacingRight = !isFacingRight;
    }

    public void PlayerHurt(Vector2 skeletonPos)
    {
        playerAnim.SetTrigger("Hurt");

        Vector2 dir = (rb.position - skeletonPos).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }
}
