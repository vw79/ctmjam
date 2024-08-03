using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkinDetails{
    [SerializeField] private GameObject Skin;
}

[Serializable]
public class TierList{
    [SerializeField] private SkinDetails[] CommonTier;
    [SerializeField] private SkinDetails[] UncommonTier;
    [SerializeField] private SkinDetails[] RareTier;
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
