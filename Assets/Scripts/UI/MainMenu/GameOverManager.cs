using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private Animator anim;
    private Image sceneTransitionImage;

    private void Start()
    {
        anim = GameObject.Find("SceneTrans").GetComponentInChildren<Animator>();
        sceneTransitionImage = GameObject.Find("SceneTrans").GetComponentInChildren<Image>();
    }
    public void RetryButton()
    {
        StartCoroutine(LoadSceneAfterFadeOut("Game"));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneAfterFadeOut("MainMenu"));
    }

    private IEnumerator LoadSceneAfterFadeOut(string sceneName)
    {
        sceneTransitionImage.enabled = true;
        anim.SetTrigger("End");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(sceneName);
    }
}
