using UnityEngine;

public class ActivatableGimmicks : MonoBehaviour
{
    //Author: Andre
    //Base class for all contraptions that can be activated by a button

    public bool isActive;
    [HideInInspector] public bool startingState;

    protected virtual void Start()
    {
        startingState = isActive;
    }

    protected virtual void FixedUpdate()
    {
        if (isActive)
        {
            Active();
        }
        else
        {
            Inactive();
        }
    }

    protected virtual void Active() { }
    protected virtual void Inactive() { }
}
