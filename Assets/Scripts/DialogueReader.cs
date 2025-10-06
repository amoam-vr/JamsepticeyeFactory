using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class DialogueReader : MonoBehaviour
{
    public string[] dialogue;
    public float readSpeed;
    public float afterTextDelay;
    public TMP_Text dialogueBox;
    private float currentChar = 0;
    private int currentSentence = 0;
    private string currentDialogue;
    private bool isWriting;

    private void OnEnable()
    {
        currentDialogue = dialogue[currentSentence];
        isWriting = true;
    }

    private void Update()
    {
        if (isWriting)
        {
            if (currentChar <= currentDialogue.Length + 1)
            {
                dialogueBox.text = Regex.Match(currentDialogue, "^.{" + (int)currentChar + "}").Value;
                currentChar += (readSpeed / 10);
            }
            else
            {
                isWriting = false;
                Invoke("NextDialogue", afterTextDelay);
            }
        }
    }

    private void NextDialogue()
    {
        if (currentSentence >= dialogue.Length - 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            currentSentence++;
            currentChar = 0;
            currentDialogue = dialogue[currentSentence];
            isWriting = true;
        }
    }
}
