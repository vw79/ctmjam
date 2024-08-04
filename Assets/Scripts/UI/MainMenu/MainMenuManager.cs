using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OptionsButton()
    {
        Debug.Log("Options Button Clicked!");
    }

    public void DressingRoom()
    {
        SceneManager.LoadScene("DressingRoom");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GachaRoom()
    {
        SceneManager.LoadScene("GachaScene");
    }
}
