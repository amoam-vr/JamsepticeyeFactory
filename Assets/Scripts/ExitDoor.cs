using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    // Author: Gustavo
    // Loads the next scene on contact
    public static bool win = false;

    private void Awake()
    {
        win = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Toy contacter = collision.GetComponent<Toy>();

        if (contacter)
        {
            if (Toy.possessedToy == contacter)
            {
                win = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
