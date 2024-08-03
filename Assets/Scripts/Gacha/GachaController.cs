using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkinDetails{
    public GameObject Skin;
}

[System.Serializable]
public class TierList{
    public SkinDetails[] CommonTier;
    public SkinDetails[] UncommonTier;
    public SkinDetails[] RareTier;
}
public class GachaController : MonoBehaviour
{
    private TierList tierList;
    
    private void RandomNumGenerator(){
        int randNum = UnityEngine.Random.Range(1,11);
        RandomGacha(randNum);
    }

   private void RandomGacha(int num){

   }
}
