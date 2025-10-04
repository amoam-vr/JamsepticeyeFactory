using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    //Author: Andre
    //Code for objects that can be moved

    //Physics variables
    protected Rigidbody2D rb;

    protected Vector2 vel;
    [HideInInspector] public Vector2 referenceFrame;

    [SerializeField] protected float acceleration = 50;

    protected bool isGrounded;
    [SerializeField] protected float fallGravity = -70;
    [SerializeField] protected float fallSpeed = -10;


    bool leftWall;
    bool rightWall;
    [HideInInspector] public bool pushable = true;

    protected virtual void Start()
    {
        //Initialize
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        pushable = !(leftWall && rightWall);
        leftWall = false;
        rightWall = false;

        vel.x = Mathf.MoveTowards(vel.x, 0, acceleration * Time.fixedDeltaTime);

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -fallGravity * Time.fixedDeltaTime);

        rb.linearVelocity = vel + referenceFrame;

        referenceFrame = Vector2.zero;

        isGrounded = false;
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;

            if (contactNormal.x >= 0.9)
            {
                vel.x = Mathf.Max(vel.x, 0);
                leftWall = true;
            }

            if (contactNormal.x <= -0.9)
            {
                vel.x = Mathf.Min(vel.x, 0);
                rightWall = true;
            }

            if (contactNormal.y >= 0.9)
            {
                vel.y = Mathf.Max(vel.y, 0);
                isGrounded = true;
            }

            if (contactNormal.y <= -0.9)
            {
                vel.y = Mathf.Min(vel.y, 0);
            }
        }
    }
}
