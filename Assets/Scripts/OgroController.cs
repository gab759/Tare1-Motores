using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgroController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 7f;
    [SerializeField] private Animator myAnimator;


    private SpriteRenderer _compSpriteRenderer;
    private Vector2 initialPosition;
    private bool isPlayerInRange = false;
    private float timer = 0f;
    private float shootInterval = 2f;
    void Awake()
    {
        _compSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            MoveTowardsPlayer();
            FlipSprite();
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                ShootBullet();
                timer = 0f;
            }
        }
        else
        {
            ReturnToInitialPosition();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void ReturnToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }
    private void FlipSprite()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        if (direction.x < 0)
        {
            _compSpriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            _compSpriteRenderer.flipX = false;
        }
    }
    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = direction * bulletSpeed;
            myAnimator.SetTrigger("GoAttack");
        }
    }
}
