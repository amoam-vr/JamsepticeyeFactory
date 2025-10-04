using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toy : DynamicObject
{
    //Author: Andre
    //Base class for toys

    public static Toy possessedToy;
    protected Toy toyScript;
    protected static List<Toy> aliveToys = new List<Toy>();
    public bool startingToy;
    protected bool isDead;

    protected bool canPush;

    //Control Variables
    protected int moveDir;

    [SerializeField] protected float jumpPermisiveness = 0.15f;
    protected float jumpPermisivenessTimer;

    [SerializeField] protected float walkSpeed = 10;

    [SerializeField] protected float jumpGravity = -30;

    [SerializeField] protected float jumpHeight = 3.5f;
    protected float jumpSpeed;

    [SerializeField] float fallDamageHeight = 10;
    float dropPos;

    public bool isMetalic;

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

    protected override void Start()
    {
        //Initialize
        base.Start();

        jumpSpeed = Mathf.Sqrt(-2 * jumpGravity * jumpHeight);

        toyScript = GetComponent<Toy>();

        aliveToys.Add(toyScript);

        if (possessedToy == null)
        {
            possessedToy = toyScript;
        }

        aliveToys.Remove(possessedToy);

        if (possessedToy == toyScript)
        {
            canPush = true;
        }
    }

    protected virtual void Update()
    {
        if (possessedToy != toyScript) return;

        moveDir = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPermisivenessTimer = jumpPermisiveness;
        }
    }

    protected override void FixedUpdate()
    {
        if (isGrounded)
        {
            if(dropPos - rb.position.y >= fallDamageHeight)
            {
                Die();
            }

            dropPos = rb.position.y;
        }

        if (!isDead && vel.y >= 0)
        {
            dropPos = rb.position.y;
        }

        if (possessedToy == toyScript)
        {
            Possesed();
        }
        else
        {
            Unpossessed();
        }
    }

    protected virtual void Possesed()
    {
        vel.x = Mathf.MoveTowards(vel.x, walkSpeed * moveDir, acceleration * Time.fixedDeltaTime);

        float gravity = Input.GetKey(KeyCode.Space) && vel.y > 0 ? jumpGravity : fallGravity;

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -gravity * Time.fixedDeltaTime);

        if (isGrounded && jumpPermisivenessTimer > 0)
        {
            vel.y = jumpSpeed;
            jumpPermisivenessTimer = 0;
        }

        jumpPermisivenessTimer = Mathf.MoveTowards(jumpPermisivenessTimer, 0, Time.fixedDeltaTime);

        rb.linearVelocity = vel + referenceFrame;

        referenceFrame = Vector2.zero;

        isGrounded = false;
    }

    protected virtual void Unpossessed()
    {
        base.FixedUpdate();
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        canPush = false;

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

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        //Stop player from accelerationg when hitting a ground or wall

        base.OnCollisionStay2D(collision);

        DynamicObject pushableObj = collision.gameObject.GetComponent<DynamicObject>();

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;

            if (contactNormal.x >= 0.9)
            {
                if (!canPush || pushableObj == null || !pushableObj.pushable)
                {
                    vel.x = Mathf.Max(vel.x, 0);
                }
                else
                {
                    pushableObj.referenceFrame.x += vel.x;
                }
            }

            if (contactNormal.x <= -0.9)
            {
                if (!canPush || pushableObj == null || !pushableObj.pushable)
                {
                    vel.x = Mathf.Min(vel.x, 0);
                }
                else
                {
                    pushableObj.referenceFrame.x += vel.x;
                }
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
