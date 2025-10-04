using UnityEngine;

public class BouncyBall : Toy
{
    //Author: Andre
    //Base class for toys

    [SerializeField] float minBounceHeight = 1;
    [SerializeField] float minBounceHorizontal = 5;

    protected override void ColUpdate()
    {
        if (groundObjs.Count > 0)
        {
            float bounceHeight = Mathf.Floor(dropPos - rb.position.y) + 0.5f;
            float bounceSpeed = Mathf.Sqrt(-2 * jumpGravity * bounceHeight);

            if (bounceHeight >= minBounceHeight)
            {
                vel.y = bounceSpeed;
            }

            dropPos = rb.position.y;
        }

        if (Mathf.Abs(vel.x) >= 5 && (leftWallObjs.Count > 0 || rightWallObjs.Count > 0))
        {
            vel.x = -1 * vel.x;
        }

        base.ColUpdate();
    }
}
