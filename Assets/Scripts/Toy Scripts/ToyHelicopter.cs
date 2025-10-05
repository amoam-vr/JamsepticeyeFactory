using UnityEngine;

public class ToyHelicopter : Toy
{
    //Author: Andre
    //A flying toy

    Vector2 flyDir;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (possessedToy != toyScript) return;

        flyDir.x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        flyDir.y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
    }

    protected override void Move()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            vel = Vector2.MoveTowards(vel, flyDir * walkSpeed, acceleration * Time.fixedDeltaTime);
            return;
        }

        base.Move();
    }

    public override void Die()
    {
        base.Die();
    }
}
