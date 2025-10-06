using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toy : DynamicObject
{
    //Author: Andre
    //Base class for toys
    // Contributors: Gustavo

    public delegate void ToyDeath(Toy toy);
    public static event ToyDeath ToyDied;

    public static Toy possessedToy;
    protected Toy toyScript;
    protected static List<Toy> aliveToys = new List<Toy>();
    public bool startingToy;
    public bool isDead;
    public Animator animator;
    public Transform character;
    public float walkAnimMult = 1;

    protected bool canPush;

    //Control Variables
    protected int moveDir;

    [SerializeField] protected float jumpPermisiveness = 0.15f;
    protected float jumpPermisivenessTimer;

    [SerializeField] protected float walkSpeed = 10;

    [SerializeField] protected float jumpGravity = -30;

    [SerializeField] protected float jumpHeight = 3.5f;
    protected float jumpSpeed;

    [SerializeField] float fallDamageHeight = 10;
    protected float dropPos;

    public bool isMetalic;

    protected override void Start()
    {
        //Initialize
        base.Start();

        jumpSpeed = Mathf.Sqrt(-2 * jumpGravity * jumpHeight);

        toyScript = GetComponent<Toy>();

        aliveToys.Add(toyScript);

        if (startingToy)
        {
            possessedToy = toyScript;
        }

        aliveToys.Remove(possessedToy);

        canPush = possessedToy == toyScript;
    }

    protected virtual void Update()
    {
        if (possessedToy != toyScript) return;

        moveDir = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPermisivenessTimer = jumpPermisiveness;
            animator.SetTrigger("Jump");
        }

        if (animator != null)
        {
            animator.SetInteger("MoveState", Mathf.Abs(moveDir) > 0 ? 1 : 0);
            animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x * walkAnimMult));
            character.rotation = Quaternion.Euler(0, 180 - moveDir * 90, 0);
        }
    }

    protected override void FixedUpdate()
    {
        if (!isDead && vel.y >= 0)
        {
            dropPos = rb.position.y;
        }

        if (possessedToy == toyScript)
        {
            Move();
        }
        else
        {
            Unpossessed();
        }
        PushCheck();
        ColUpdate();
    }

    protected override void ColUpdate()
    {
        //Fall damage
        if (groundObjs.Count > 0)
        {
            if (dropPos - rb.position.y >= fallDamageHeight)
            {
                foreach (var obj in groundObjs)
                {
                    if (!obj.CompareTag("Soft Landing"))
                    {
                        Die();
                        break;
                    }
                }
            }

            dropPos = rb.position.y;
        }
        else
        {
            animator.SetTrigger("Land");
        }

        base.ColUpdate();
    }

    protected virtual void PushCheck()
    {
        //Push objects
        if (vel.x > 0)
        {
            bool clearList = false;
            foreach (var obj in rightWallObjs)
            {
                DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

                if (pushableObj != null && pushableObj.pushable && weight >= pushableObj.weight)
                {
                    clearList = true;
                    pushableObj.referenceFrame.x += vel.x;
                }
            }

            if (clearList)
            {
                rightWallObjs.Clear();
            }
        }

        if (vel.x < 0)
        {
            bool clearList = false;
            foreach (var obj in leftWallObjs)
            {
                DynamicObject pushableObj = obj.GetComponent<DynamicObject>();

                if (pushableObj != null && pushableObj.pushable && weight >= pushableObj.weight)
                {
                    clearList = true;
                    pushableObj.referenceFrame.x += vel.x;
                }
            }

            if (clearList)
            {
                leftWallObjs.Clear();
            }
        }
    }

    protected override void Move()
    {
        vel.x = Mathf.MoveTowards(vel.x, walkSpeed * moveDir, acceleration * Time.fixedDeltaTime);

        float gravity = Input.GetKey(KeyCode.Space) && vel.y > 0 ? jumpGravity : fallGravity;

        vel.y = Mathf.MoveTowards(vel.y, fallSpeed, -gravity * Time.fixedDeltaTime);

        if (groundObjs.Count > 0 && jumpPermisivenessTimer > 0)
        {
            vel.y = jumpSpeed;
            jumpPermisivenessTimer = 0;
        }

        jumpPermisivenessTimer = Mathf.MoveTowards(jumpPermisivenessTimer, 0, Time.fixedDeltaTime);
    }

    protected virtual void Unpossessed()
    {
        base.Move();
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        animator.SetTrigger("Die");

        canPush = false;

        aliveToys.Remove(this);

        vel = Vector2.zero;

        if (possessedToy != this)
        {
            return;
        }

        //Find closest toy
        Toy closestToy = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < aliveToys.Count; i++)
        {
            if (closestToy == null)
            {
                closestToy = aliveToys[i];
            }

            float toyDistance = Vector3.Distance(transform.position, aliveToys[i].transform.position);
            
            if (toyDistance < closestDistance)
            {
                closestDistance = toyDistance;
                closestToy = aliveToys[i];
            }
        }

        // Check if the toy is within the range of a spirit seal
        /*
        foreach (GameObject seal in GameObject.FindGameObjectsWithTag("SpiritSeal"))
        {
            if (seal.GetComponent<SpiritSeal>().SealingRadius > Vector3.Distance(transform.position, seal.transform.position))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        */
        if (ToyDied != null)
        {
            ToyDied(closestToy);
        }
    }

    private void OnDestroy()
    {
        Die();
    }
}
