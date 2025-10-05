using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyButton : Button
{
    //Author: Andre
    //A button that needs heavy weight to be pressed down

    protected List<GameObject> ceilingObjs = new List<GameObject>();

    void Start()
    {
        
    }

    protected override void FixedUpdate()
    {
        foreach (var obj in ceilingObjs)
        {
            DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

            if (pushableObj != null && pushableObj.pushable)
            {
                pressedDown = true;
            }
        }
        ceilingObjs.Clear();

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
