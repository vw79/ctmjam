using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkinDeets{
    public GameObject Skin;
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
    private GameObject DisplayModel;
    private int CurrentIndex=0;

    private void Start() {
        ShowCurrentSkin();
    }
    public void ShowCurrentSkin(){

        if (CurrentIndex >= 0 && CurrentIndex < skinDeets.Length)
        {
            SkinDeets CurrentSkin = skinDeets[CurrentIndex];
            if (DisplayModel != null) 
            {
                DisplayModel.SetActive(false);
                DisplayModel = CurrentSkin.Skin;
                DisplayModel.SetActive(true);
            }
            else { 
                DisplayModel = CurrentSkin.Skin;
                DisplayModel.SetActive(true);
            }
            SkinName.text = (CurrentSkin.Name);
            if(CurrentSkin.IsOwned){
                SelectBtn.interactable = false;
            }else{
                SelectBtn.interactable = true;
            }
            nextBtn.interactable = CurrentIndex > 0;
            prevBtn.interactable = CurrentIndex < skinDeets.Length - 1;

        }
    }

    public void OnNextBtnPressed(){
        if (CurrentIndex < skinDeets.Length - 1)
            {
                CurrentIndex++;
                ShowCurrentSkin();
            } 

    }

    public void OnPrevBtnPressed(){
        if (CurrentIndex > 0)
            {
                CurrentIndex--;
                ShowCurrentSkin();
            } 
    }

    public void OnSelectedBtnPressed(){
        
    }
   
}
