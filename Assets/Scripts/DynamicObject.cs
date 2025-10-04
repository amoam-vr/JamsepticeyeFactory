using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicObject : MonoBehaviour
{
    //Author: Andre
    //Code for objects that can be moved

    //Physics variables
    protected Rigidbody2D rb;

    protected Vector2 vel;
    [HideInInspector] public Vector2 referenceFrame;

    [SerializeField] protected float acceleration = 50;

    protected List<GameObject> groundObjs = new List<GameObject>();
    protected List<GameObject> ceilingObjs = new List<GameObject>();
    protected List<GameObject> leftWallObjs = new List<GameObject>();
    protected List<GameObject> rightWallObjs = new List<GameObject>();

    [SerializeField] protected float fallGravity = -70;
    [SerializeField] protected float fallSpeed = -10;



    [HideInInspector] public bool pushable = true;

    protected virtual void Start()
    {
        //Initialize
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        ColUpdate();
    }

    protected virtual void Move()
    {
        vel.x = 0;
        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -fallGravity * Time.fixedDeltaTime);
    }

    void CrushCheck()
    {

        bool staticGround = false;
        foreach (var obj in groundObjs)
        {
            if (obj.GetComponent<DynamicObject>() == null)
            {
                staticGround = true;
            }
        }

        bool staticCeil = false;
        foreach (var obj in ceilingObjs)
        {
            if (obj.GetComponent<DynamicObject>() == null)
            {
                staticCeil = true;
            }
        }

        bool staticRightWall = false;
        foreach (var obj in rightWallObjs)
        {
            if (obj.GetComponent<DynamicObject>() == null)
            {
                staticRightWall = true;
            }
        }

        bool staticLeftWall = false;
        foreach (var obj in leftWallObjs)
        {
            if (obj.GetComponent<DynamicObject>() == null)
            {
                staticLeftWall = true;
            }
        }

        if ((staticGround && staticCeil) || (staticRightWall && staticLeftWall))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void ColUpdate()
    {
        pushable = !(leftWallObjs.Count > 0 && rightWallObjs.Count > 0);

        CrushCheck();

        if (groundObjs.Count > 0)
        {
            vel.y = Mathf.Max(vel.y, 0);
        }

        if (ceilingObjs.Count > 0)
        {
            vel.y = Mathf.Min(vel.y, 0);
        }

        if (rightWallObjs.Count > 0)
        {
            vel.x = Mathf.Min(vel.x, 0);
        }

        if (leftWallObjs.Count > 0)
        {
            vel.x = Mathf.Max(vel.x, 0);
        }

        rb.linearVelocity = vel + referenceFrame;

        referenceFrame = Vector2.zero;

        leftWallObjs.Clear();
        rightWallObjs.Clear();
        ceilingObjs.Clear();
        groundObjs.Clear();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;

            if (contactNormal.x >= 0.9)
            {
                if (!leftWallObjs.Contains(obj))
                {
                    leftWallObjs.Add(obj);
                }
            }

            if (contactNormal.x <= -0.9)
            {
                if (!rightWallObjs.Contains(obj))
                {
                    rightWallObjs.Add(obj);
                }
            }

            if (contactNormal.y >= 0.9)
            {
                if (!groundObjs.Contains(obj))
                {
                    groundObjs.Add(obj);
                }
            }

            if (contactNormal.y <= -0.9)
            {
                if (!ceilingObjs.Contains(obj))
                {
                    ceilingObjs.Add(obj);
                }
            }
        }
    }
}
