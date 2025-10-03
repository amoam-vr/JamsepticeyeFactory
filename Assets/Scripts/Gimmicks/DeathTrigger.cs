using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    //Author: Andre
    //A trigger that kills a toy when touched

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Toy toy = collision.GetComponent<Toy>();

        if (toy != null)
        {
            toy.Die();
        }
    }
}
