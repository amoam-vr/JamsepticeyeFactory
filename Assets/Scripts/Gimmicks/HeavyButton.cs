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

    Collider2D platformCol;
    Collider2D springCol;
    float buttonHeight;

    float inactivePosY;
    float activePosY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        platformCol = GetComponent<Collider2D>();
        springCol = transform.parent.GetChild(1).GetComponent<Collider2D>();

        buttonHeight = platformCol.bounds.min.y - springCol.bounds.min.y;

        inactivePosY = rb.position.y;
        activePosY = inactivePosY - buttonHeight;
    }

    protected override void FixedUpdate()
    {
        float distance = rb.position.y - activePosY;

        float speed = speedAnimCurve.Evaluate((distance / buttonHeight) - 1) * topSpeed;
        speed = Mathf.Max(speed, 0.1f);
        int dir = 0;

        foreach (var obj in ceilingObjs)
        {
            DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

            if (pushableObj != null && pushableObj.weight >= weightActivation)
            {
                pressedDown = true;

                if (distance > 0)
                {
                    dir = -1;
                }
            }

            pushableObj.referenceFrame.y += speed * dir;
        }

        ceilingObjs.Clear();

        if(distance < buttonHeight && !pressedDown)
        {
            dir = 1;
        }

        rb.linearVelocityY = speed * dir;

        float clampedYPos = Mathf.Clamp(rb.position.y, activePosY, inactivePosY);
        rb.position = new Vector2(rb.position.x, clampedYPos);

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
