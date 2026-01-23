using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float moveSpeed;
    public float moveAreaLimit;

    public float stopDistance;

    public AudioClip animalSound;
    public float soundIntervalRange;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private Vector2 basePos;

    private float timer;
    private bool isMoving;

    public GameObject heartObject;
    SpriteRenderer heartRenderer;

    AudioSource animalAudioSource;

    public bool isAttacked;

    float soundInterval;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        heartRenderer = heartObject.GetComponent<SpriteRenderer>();
        animalAudioSource = GetComponent<AudioSource>();
        basePos = transform.position;
        SetPatrolWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        SetMovement();
        RandomSoundLoop();
    }

    private void FixedUpdate()
    {
        MoveToWaypoint();
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

    }

    void RandomSoundLoop()
    {
        if (GameManager.instance.isPaused) return;

        soundInterval -= Time.deltaTime;

        if (soundInterval <= 0f)
        {
            Debug.Log("Play Sound");
            animalAudioSource.PlayOneShot(animalSound);
            soundInterval = Random.Range(soundIntervalRange / 2, soundIntervalRange);
        }
    }

    private void SetPatrolWaypoint()
    {
        float randomAxis = Random.Range(-moveAreaLimit, moveAreaLimit);
        targetPos = basePos + new Vector2(randomAxis, randomAxis);
        FlipSprite();
    }

    private void FlipSprite()
    {
        Vector2 playerDirection = gameObject.transform.localScale;

        float xScale = Mathf.Abs(playerDirection.x);

        float flipDirection = targetPos.x - rb.position.x;

        if (flipDirection < 0) {
            playerDirection.x = xScale;
        } 
        else
        {
            playerDirection.x = -xScale;
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

    public void Attacked()
    {
        heartRenderer.color = Color.black;
        StartCoroutine(HeartEmote());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Collide With Player");
            StartCoroutine(HeartEmote());
        }
    }

    IEnumerator HeartEmote()
    {
        heartObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        heartObject.SetActive(false);
        heartRenderer.color = Color.white;
    }
}