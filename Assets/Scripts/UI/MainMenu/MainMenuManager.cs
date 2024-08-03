using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene("Game");
    }

    public void OptionsButton()
    {
        Debug.Log("Options Button Clicked!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
