using UnityEngine;

public class ElectroMagnet : ActivatableGimmicks
{
    //Author: Andre
    //A magnet that kills metallic toys

    [SerializeField] float attractForce = 60;
    Transform magneticBeam;

    protected override void Start()
    {
        base.Start();
        magneticBeam = transform.GetChild(0);
    }

    protected override void Active()
    {
        magneticBeam.gameObject.SetActive(true);
    }

    protected override void Inactive()
    {
        magneticBeam.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Toy toy = collision.GetComponent<Toy>();

        if (toy != null && toy.isMetalic)
        {
            toy.Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Toy toy = collision.GetComponent<Toy>();

        if (toy != null && toy.isMetalic)
        {
            toy.referenceFrame += (Vector2)transform.up * -attractForce;
        }
    }
}
