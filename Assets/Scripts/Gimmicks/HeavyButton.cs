using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyButton : Button
{
    //Author: Andre
    //A button that needs heavy weight to be pressed down

    protected List<GameObject> ceilingObjs = new List<GameObject>();
    Rigidbody2D rb;

    [SerializeField] int weightActivation = 3;
    [SerializeField] float topSpeed = 10;
    [SerializeField] AnimationCurve speedAnimCurve;

    BoxCollider2D platformCol;
    BoxCollider2D springCol;
    Transform springTransform;
    float buttonHeight;

    float inactivePosY;
    float activePosY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        platformCol = GetComponent<BoxCollider2D>();
        springTransform = transform.parent.GetChild(1);
        springCol = springTransform.GetComponentInChildren<BoxCollider2D>();

        buttonHeight = platformCol.bounds.min.y - springCol.bounds.min.y;

        inactivePosY = rb.position.y;
        activePosY = inactivePosY - buttonHeight;
    }

    protected override void FixedUpdate()
    {
        float distance = rb.position.y - activePosY;

        float speed = speedAnimCurve.Evaluate((distance / buttonHeight)) * topSpeed;
        speed = Mathf.Max(speed, 1);
        int dir = 0;

        bool weighedDown = false;

        foreach (var obj in ceilingObjs)
        {
            DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

            if (pushableObj != null && pushableObj.weight >= weightActivation)
            {
                weighedDown = true;

                if (distance > 0)
                {
                    dir = -1;
                }
            }

            pushableObj.referenceFrame.y += speed * dir;
        }

        ceilingObjs.Clear();

        if(distance < buttonHeight && !weighedDown)
        {
            dir = 1;
            speed = topSpeed;
        }

        rb.linearVelocityY = speed * dir;

        float clampedYPos = Mathf.Clamp(rb.position.y, activePosY, inactivePosY);
        rb.position = new Vector2(rb.position.x, clampedYPos);

        if (weighedDown && distance == 0)
        {
            pressedDown = true;
        }
        springTransform.localScale = new Vector3(1, distance / buttonHeight, 1);

        base.FixedUpdate();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;

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
