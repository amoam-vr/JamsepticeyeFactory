using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject narration;

    public void ButtonAction(int action)
    {
        switch (action)
        {
            case 0:
                mainMenu.SetActive(false);
                narration.SetActive(true);
                break;
            case 1:
                Application.Quit();
                break;
        }
    }
}
