using UnityEngine;

public class BouncyBall : Toy
{
    //Author: Andre
    //A bouncy ball

    [SerializeField] float minBounceHeight = 1;
    [SerializeField] float minBounceHorizontal = 5;
    float bounceCoolDown;

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

        if (Mathf.Abs(vel.x) >= minBounceHorizontal && bounceCoolDown <= 0 && (leftWallObjs.Count > 0 || rightWallObjs.Count > 0))
        {
            vel.x *= -1;
            bounceCoolDown = Time.fixedDeltaTime * 2;
        }

        if (bounceCoolDown > 0)
        {
            bounceCoolDown -= Time.fixedDeltaTime;
        }

        base.ColUpdate();
    }
}
