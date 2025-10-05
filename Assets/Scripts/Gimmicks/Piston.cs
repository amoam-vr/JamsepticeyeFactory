using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piston : ActivatableGimmicks
{
    //Author: Andre
    //A psiton that moves back and forth

    [SerializeField] bool cycle = true;

    Rigidbody2D rb;

    [SerializeField] float topSpeed = 10;
    [SerializeField] float acceleration = 20;
    float speed;

    [SerializeField] float stopDuration = 1;
    float stopTimer;

    [SerializeField] float distance = 5;

    Vector2[] positions;


    int targetPosIndex;

    protected override void Start()
    {
        //Initialize
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        positions = new Vector2[2];

        positions[0] = transform.position;

        positions[1] = transform.position + (transform.up * distance);
    }

    protected override void Active()
    {
        if (!cycle)
        {
            targetPosIndex = 1;
            speed = Mathf.MoveTowards(speed, topSpeed, acceleration * Time.fixedDeltaTime);
            rb.MovePosition(Vector2.MoveTowards(rb.position, positions[targetPosIndex], speed * Time.fixedDeltaTime));
            return;
        }

        if (stopTimer <= 0)
        {
            speed = Mathf.MoveTowards(speed, topSpeed, acceleration * Time.fixedDeltaTime);

            rb.MovePosition(Vector2.MoveTowards(rb.position, positions[targetPosIndex], speed * Time.fixedDeltaTime));
            
            if (rb.position == positions[targetPosIndex])
            {
                targetPosIndex++;
                targetPosIndex = targetPosIndex % 2;
                speed = 0;
                stopTimer = stopDuration;
            }
        }
        else
        {
            stopTimer -= Time.fixedDeltaTime;
        }
    }

    protected override void Inactive()
    {
        if (!cycle)
        {
            targetPosIndex = 0;
            speed = Mathf.MoveTowards(speed, topSpeed, acceleration * Time.fixedDeltaTime);
            rb.MovePosition(Vector2.MoveTowards(rb.position, positions[targetPosIndex], speed * Time.fixedDeltaTime));
            return;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isActive) return;

        DynamicObject pushableObj = collision.gameObject.GetComponent<DynamicObject>();

        if (pushableObj != null && stopTimer <= 0)
        {
            Vector2 dir = (positions[targetPosIndex] - (Vector2)transform.position).normalized;

            pushableObj.referenceFrame += dir * (Mathf.Clamp(speed + Time.fixedDeltaTime * 20, 0, topSpeed));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.up * distance, 0.5f);
    }
}
