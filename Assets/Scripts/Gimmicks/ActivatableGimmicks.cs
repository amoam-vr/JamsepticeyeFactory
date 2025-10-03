using UnityEngine;

public class ActivatableGimmicks : MonoBehaviour
{
    //Author: Andre
    //Base class for all contraptions that can be activated by a button

    public bool isActive;

    void FixedUpdate()
    {
        if (isActive)
        {
            Active();
        }
    }

    protected virtual void Active() { }
}
