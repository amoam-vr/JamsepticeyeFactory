using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toy : MonoBehaviour
{
    //Author: Andre
    //Base class for toys

    protected static Toy possessedToy;
    protected static List<Toy> aliveToys = new List<Toy>();
    public bool startingToy;
    protected bool isDead;

    //Control Variables
    protected int moveDir;

    protected bool isGrounded;
    [SerializeField] protected float jumpPermisiveness = 0.15f;
    protected float jumpPermisivenessTimer;

    //Physics variables
    protected Rigidbody2D rb;
    
    protected Vector2 vel;
    protected Vector2 referenceFrame;

    [SerializeField] protected float walkSpeed = 10;
    [SerializeField] protected float walkAcceleration = 10;

    [SerializeField] protected float lightGravity = -30;
    [SerializeField] protected float heavyGravity = -70;

    [SerializeField] protected float fallSpeed = -10;

    [SerializeField] protected float jumpHeight = 3;
    protected float jumpSpeed;

    private void OnValidate()
    {
        if (startingToy)
        {
            if (possessedToy != null)
            {
                possessedToy.startingToy = false;
            }

            possessedToy = GetComponent<Toy>();
        }
    }

    protected virtual void Start()
    {
        //Initialize
        rb = GetComponent<Rigidbody2D>();
        jumpSpeed = Mathf.Sqrt(-2 * lightGravity * jumpHeight);

        aliveToys.Add(GetComponent<Toy>());

        if (possessedToy == null)
        {
            possessedToy = GetComponent<Toy>();
        }

        aliveToys.Remove(possessedToy);
    }

    protected virtual void Update()
    {
        if (possessedToy != this) return;

        moveDir = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPermisivenessTimer = jumpPermisiveness;
        }
    }

    protected virtual void FixedUpdate()
    {
        float gravity = Input.GetKey(KeyCode.Space) && vel.y > 0 ? lightGravity : heavyGravity;

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -gravity * Time.fixedDeltaTime);
        
        if (possessedToy == this)
        {
            Possesed();
        }
        else
        {
            Unpossesed();
        }

        rb.linearVelocity = vel + referenceFrame;

        isGrounded = false; 
    }

    protected virtual void Possesed()
    {
        vel.x = Mathf.MoveTowards(vel.x, walkSpeed * moveDir, walkAcceleration * Time.fixedDeltaTime);

        if (isGrounded && jumpPermisivenessTimer > 0)
        {
            vel.y = jumpSpeed;
            jumpPermisivenessTimer = 0;
        }

        jumpPermisivenessTimer = Mathf.MoveTowards(jumpPermisivenessTimer, 0, Time.fixedDeltaTime);
    }

    protected virtual void Unpossesed()
    {

    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        aliveToys.Remove(this);

        if (possessedToy != this)
        {
            return;
        }

        //Find closest toy
        Toy closestToy = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < aliveToys.Count; i++)
        {
            if (closestToy == null)
            {
                closestToy = aliveToys[i];
            }

            float toyDistance = Vector3.Distance(transform.position, aliveToys[i].transform.position);
            
            if (toyDistance < closestDistance)
            {
                closestDistance = toyDistance;
                closestToy = aliveToys[i];
            }
        }

        possessedToy = closestToy;
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //Stop player from accelerationg when hitting a ground or wall

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;


            if (contactNormal.x >= 0.9)
            {
                vel.x = Mathf.Max(vel.x, 0);
            }

            if (contactNormal.x <= -0.9)
            {
                vel.x = Mathf.Min(vel.x, 0);
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
