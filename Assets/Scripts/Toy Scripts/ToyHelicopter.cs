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

        animator.SetFloat("Direction", (int)flyDir.x);
    }

    protected override void Move()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            vel = Vector2.MoveTowards(vel, flyDir * walkSpeed, acceleration * Time.fixedDeltaTime);
            dropPos = rb.position.y;
            return;
        }

        base.Move();
    }

    public override void Die()
    {
        base.Die();
    }
}
