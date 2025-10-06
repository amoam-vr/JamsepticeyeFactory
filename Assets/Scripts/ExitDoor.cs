using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    // Author: Gustavo
    // Loads the next scene on contact
    
    static int currentLevel = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Toy contacter = collision.GetComponent<Toy>();

        if (contacter)
        {
            if (Toy.possessedToy == contacter)
            {
                currentLevel++;
                SceneManager.LoadScene(currentLevel);
            }
        }
    }
}
