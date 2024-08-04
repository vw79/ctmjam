using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkinDeets
{
    public Sprite prefabSprite;
    public string Name;
    public bool IsOwned;

}

public class DRoomController : MonoBehaviour
{
    public SkinDeets[] skinDeets;
    public Button nextBtn;
    public Button prevBtn;
    public Button SelectBtn;
    public TextMeshProUGUI SkinName;
    public GameObject DisplayModelBase;
    public GameObject DisplayModel1;
    public GameObject DisplayModel2;
    public GameObject DisplayModel3;
    private int CurrentIndex = 0;


    private void Start()
    {
        ShowCurrentSkin();
        DisplayModel1.SetActive(false);
        DisplayModel2.SetActive(false);
        DisplayModel3.SetActive(false);
        DisplayModelBase.SetActive(true);

    }
    public void ShowCurrentSkin()
    {

        if (CurrentIndex == 0)
        {
            DisplayModelBase.SetActive(true);
            DisplayModel1.SetActive(false);
            DisplayModel2.SetActive(false);
            DisplayModel3.SetActive(false);
        }

        if (CurrentIndex == 1)
        {
            DisplayModelBase.SetActive(false);
            DisplayModel1.SetActive(true);
            DisplayModel2.SetActive(false);
            DisplayModel3.SetActive(false);
        }

        if (CurrentIndex == 2)
        {
            DisplayModelBase.SetActive(false);
            DisplayModel1.SetActive(false);
            DisplayModel2.SetActive(true);
            DisplayModel3.SetActive(false);
        }

        if (CurrentIndex == 3)
        {
            DisplayModelBase.SetActive(false);
            DisplayModel1.SetActive(false);
            DisplayModel2.SetActive(false);
            DisplayModel3.SetActive(true);
        }

        //if (CurrentIndex >= 0 && CurrentIndex < skinDeets.Length)
        //{
        //    SkinDeets CurrentSkin = skinDeets[CurrentIndex];
        //    if (DisplayModel != null) 
        //    {
        //        DisplayModel.SetActive(false);
        //        imgRenderer.sprite = CurrentSkin.prefabSprite;
        //        DisplayModel.SetActive(true);
        //    }
        //    else {
        //        imgRenderer.sprite = CurrentSkin.prefabSprite;
        //        DisplayModel.SetActive(true);
        //    }
        //    SkinName.text = (CurrentSkin.Name);
        //    if(CurrentSkin.IsOwned){
        //        SelectBtn.interactable = false;
        //    }else{
        //        SelectBtn.interactable = true;
        //    }
        //    nextBtn.interactable = CurrentIndex > 0;
        //    prevBtn.interactable = CurrentIndex < skinDeets.Length - 1;

        //}
    }

    public void OnNextBtnPressed()
    {
        if (CurrentIndex < skinDeets.Length - 1)
        {
            CurrentIndex++;
            ShowCurrentSkin();
        }

    }

    public void OnPrevBtnPressed()
    {
        if (CurrentIndex > 0)
        {
            CurrentIndex--;
            ShowCurrentSkin();
        }
    }

    public void OnSelectedBtnPressed()
    {

    }

}