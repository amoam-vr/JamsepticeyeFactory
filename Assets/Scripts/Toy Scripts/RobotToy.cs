using UnityEngine;

public class RobotToy : Toy
{
    //Author: Andre
    //A robot that moves by itself when not possesed
    [SerializeField, Range(-1, 1)] int initialDir;
    [SerializeField] float turnCooldown = 0.5f;
    float turnCooldownTimer;

    protected override void Start()
    {
        base.Start();
        canPush = true;
        moveDir = initialDir;
    }

    protected override void Unpossessed()
    {
        if (isDead)
        {
            moveDir = 0;
        }

        vel.x = Mathf.MoveTowards(vel.x, walkSpeed * moveDir, acceleration * Time.fixedDeltaTime);

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -fallGravity * Time.fixedDeltaTime);
    }

    protected override void ColUpdate()
    {
        bool turn = (leftWallObjs.Count > 0 && vel.x < 0) || (rightWallObjs.Count > 0 && vel.x > 0);

        if (turnCooldownTimer <= 0 && turn)
        {
            moveDir *= -1;
            turnCooldownTimer = turnCooldown;
        }

        if (turnCooldownTimer > 0)
        {
            turnCooldownTimer -= Time.fixedDeltaTime;
        }

        base.ColUpdate();

        pushable = moveDir == 0;
    }
}
