using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class specialStart : MonoBehaviour
{
    public Button startWithSkin;

    void Start()
    {
        startWithSkin.onClick.AddListener(CallGameManagerFunction);
    }

    void CallGameManagerFunction()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.UpdateSelectedCharacter();
            GameManager.instance.LoadGameWithSkin();
        }
        else
        {
            Debug.LogError("GameManager instance is null");
        }
    }
}