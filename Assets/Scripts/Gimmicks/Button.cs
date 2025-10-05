using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : MonoBehaviour
{
    //Author: Andre
    //A button that can activate other elements while pressed down

    [SerializeField] List<ActivatableGimmicks> gimmicks;

    protected bool pressedDown;
    bool activeGimmicks;

    [SerializeField] Gradient activeLineColor;
    [SerializeField] Gradient inactiveLineColor;

    protected List<LineRenderer> connectingLines = new List<LineRenderer>();

    protected virtual void Start()
    {
        LineRenderer line = GetComponent<LineRenderer>();

        for (int i = 0; i < gimmicks.Count; i++)
        {
            GameObject lineObj = new GameObject("Button Line");
            lineObj.transform.parent = gameObject.transform;
            lineObj.transform.localPosition = Vector3.zero;

            LineRenderer gimmickLine = lineObj.AddComponent<LineRenderer>();

            gimmickLine.materials = line.materials;
            gimmickLine.shadowCastingMode = line.shadowCastingMode;
            gimmickLine.colorGradient = inactiveLineColor;
            gimmickLine.widthCurve = line.widthCurve;
            gimmickLine.numCornerVertices = 5;

            gimmickLine.SetPosition(0, transform.position);
            gimmickLine.SetPosition(1, gimmicks[i].transform.position);

            connectingLines.Add(gimmickLine);
        }

        Destroy(line);
    }

    protected virtual void FixedUpdate()
    {
        if (pressedDown)
        {
            if (!activeGimmicks)
            {
                for (int i = 0; i < gimmicks.Count; i++)
                {
                    gimmicks[i].isActive = !gimmicks[i].startingState;
                    connectingLines[i].colorGradient = activeLineColor;
                }

                activeGimmicks = true;
            }
        }
        else
        {
            if (activeGimmicks)
            {
                for (int i = 0; i < gimmicks.Count; i++)
                {
                    gimmicks[i].isActive = gimmicks[i].startingState;
                    connectingLines[i].colorGradient = inactiveLineColor;
                }

                activeGimmicks = false;
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
