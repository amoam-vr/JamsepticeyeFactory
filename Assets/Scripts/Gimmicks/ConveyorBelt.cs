using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : ActivatableGimmicks
{
    //Author: Andre
    //A conveyor belt that moves objects around

    [SerializeField, Range(-1, 1)] int dir = 1;
    protected List<GameObject> ceilingObjs = new List<GameObject>();

    [SerializeField] float speed = 5;

    [System.Serializable]
    enum InactiveBehaviour
    {
        Stop,
        InvertDirection
    }

    [SerializeField] InactiveBehaviour inactiveBehaviour;

    List<Material> conveyorMaterials = new List<Material>();
    int materialDir;

    string materialFloatID = "Speed";

    protected override void Start()
    {
        base.Start();

        for (int i = 1; i < transform.childCount; i++)
        {
            conveyorMaterials.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ceilingObjs.Clear();
    }

    protected override void Active()
    {
        //Move other objects on top of this conveyor belt
        foreach (var obj in ceilingObjs)
        {
            DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

            if (pushableObj != null)
            {
                pushableObj.referenceFrame.x += speed * dir;
            }
        }

        if (materialDir == dir) return;

        foreach (var material in conveyorMaterials)
        {
            material.SetFloat(materialFloatID, dir);
        }

        materialDir = dir;
    }

    protected override void Inactive()
    {
        if (inactiveBehaviour == InactiveBehaviour.Stop)
        {
            if (materialDir == 0) return;

            foreach (var material in conveyorMaterials)
            {
                material.SetFloat(materialFloatID, 0);
            }

            materialDir = 0;
            return;
        }

        //Move other objects on top of this conveyor belt
        foreach (var obj in ceilingObjs)
        {
            DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

            if (pushableObj != null)
            {
                pushableObj.referenceFrame.x += speed * dir * -1;
            }
        }

        if (materialDir == dir * -1) return;

        foreach (var material in conveyorMaterials)
        {
            material.SetFloat(materialFloatID, dir * -1);
        }

        materialDir = dir * -1;
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
