using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    //Author: Andre
    //Code for objects that can be moved

    //Physics variables
    protected Rigidbody2D rb;

    protected Vector2 vel;
    protected Vector2 referenceFrame;

    [SerializeField] protected float acceleration = 50;

    [SerializeField] protected float fallGravity = -70;
    [SerializeField] protected float fallSpeed = -10;

    protected virtual void Start()
    {
        //Initialize
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        vel.x = Mathf.MoveTowards(vel.x, 0, acceleration * Time.fixedDeltaTime);

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -fallGravity * Time.fixedDeltaTime);

        rb.linearVelocity = vel + referenceFrame;
    }

}
