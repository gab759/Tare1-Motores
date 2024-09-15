using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovementController : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float chaseSpeedMultiplier = 2f;
    [SerializeField] private LayerMask playerLayer;

    private Transform currentPositionTarget;
    private int patrolPos = 0;
    private bool isChasingPlayer = false;

    private void Start()
    {
        currentPositionTarget = checkpointsPatrol[patrolPos];
        transform.position = currentPositionTarget.position;
    }

    private void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);
    }
    private void Patrol()
    {
        CheckNewPoint();
        CheckForPlayer();
    }
    private void CheckNewPoint()
    {
        if (Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25)
        {
            patrolPos = patrolPos + 1 == checkpointsPatrol.Length ? 0 : patrolPos + 1;
            currentPositionTarget = checkpointsPatrol[patrolPos];
            myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized * velocityModifier;
            CheckFlip(myRBD2.velocity.x);
        }

    }
    private void CheckForPlayer()
    {
        Vector2 direction = (currentPositionTarget.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasingPlayer = true;
        }
        else
        {
            hit.point = (Vector2)transform.position + direction * 5f;
        }
        Debug.DrawRay(transform.position, (hit.point - (Vector2)transform.position), Color.red);

    }
    private void ChasePlayer()
    {
        Vector2 playerDirection = (currentPositionTarget.position - transform.position).normalized;
        myRBD2.velocity = playerDirection * velocityModifier * chaseSpeedMultiplier;
        CheckFlip(myRBD2.velocity.x);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentPositionTarget.position - transform.position, 5f, playerLayer);
        if (hit.collider == null || !hit.collider.CompareTag("Player"))
        {
            isChasingPlayer = false;
        }
    }
    private void CheckFlip(float x_Position)
    {
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
