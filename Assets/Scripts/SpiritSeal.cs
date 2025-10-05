using UnityEngine;

public class SpiritSeal : MonoBehaviour
{
    // Author: Gustavo
    // Marks the location of a spirit seal to reset the level if a toy dies too close to it
    //Contributor: Andre

    Ghost ghost;
    float radius;

    private void Start()
    {
        ghost = GameObject.FindWithTag("Ghost").GetComponent<Ghost>();

        radius = transform.localScale.x / 2;
    }

    private void Update()
    {
        if (!Toy.possessedToy.isDead) return;

        if (Vector2.Distance(ghost.transform.position, transform.position) <= radius)
        {
            ghost.TrueDeath();
        }
    }
}
