using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : MonoBehaviour
{
    //Author: Andre
    //A button that can activate other elements while pressed down

    [SerializeField] List<ActivatableGimmicks> gimmicks;

    bool pressedDown;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (pressedDown)
        {
            for (int i = 0; i < gimmicks.Count; i++)
            {
                gimmicks[i].isActive = !gimmicks[i].startingState;
            }
        }
        else
        {
            for (int i = 0; i < gimmicks.Count; i++)
            {
                gimmicks[i].isActive = gimmicks[i].startingState;
            }
        }

        pressedDown = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<DynamicObject>() != null)
        {
            pressedDown = true;
        }
    }
}
