using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghost : MonoBehaviour
{
    //Author: Andre
    //A ghost that flies between toys to posses them

    [SerializeField] float possessionDuration;
    float possessionTimer;

    [SerializeField] AnimationCurve speedCurve;

    Toy targetToy;
    Vector2 deadToyPos;

    SpriteRenderer ghostSprite;

    private void Start()
    {
        ghostSprite = GetComponent<SpriteRenderer>();

        possessionTimer = possessionDuration;

        targetToy = null;
        ghostSprite.enabled = false;
    }

    private void OnEnable()
    {
        Toy.ToyDied += OnToyDeath;
    }

    private void OnDisable()
    {
        Toy.ToyDied -= OnToyDeath;
    }

    void Update()
    {
        if (targetToy != null)
        {
            transform.position = (Vector2.Lerp(deadToyPos, targetToy.transform.position, speedCurve.Evaluate(possessionTimer / possessionDuration)));

            possessionTimer = Mathf.MoveTowards(possessionTimer, possessionDuration, Time.unscaledDeltaTime);

            if ((Vector2)transform.position == (Vector2)targetToy.transform.position)
            {
                Toy.possessedToy = targetToy;
                Time.timeScale = 1;

                targetToy = null;
                ghostSprite.enabled = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                TrueDeath();
            }
        }
    }

    void OnToyDeath(Toy toy)
    {
        Time.timeScale = 0;

        if (toy == null)
        {
            TrueDeath();
        }
        else
        {
            deadToyPos = Toy.possessedToy.transform.position;
            transform.position = deadToyPos;
            targetToy = toy;

            possessionTimer = 0;
            ghostSprite.enabled = true;
        }
    }

    public void TrueDeath()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
