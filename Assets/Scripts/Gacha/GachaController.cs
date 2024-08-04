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
    public TierList tierList;
    [SerializeField] [Range(0,1)] private float commonProbability ;
    [SerializeField] [Range(0,1)] private float uncommonProbability ;
    [SerializeField] [Range(0,1)] private float rareProbability ;
    
    private void ShowSkin(){
        GameObject skin = RandomGacha();
    }

   private GameObject RandomGacha(){

    float num = UnityEngine.Random.Range(0f,10f);

         if (num <= rareProbability)
        {
            // Select a random item from RareTier
            return GetRandomSkin(tierList.RareTier);
        }
        else if (num <= rareProbability + uncommonProbability)
        {
            // Select a random item from UncommonTier
            return GetRandomSkin(tierList.UncommonTier);
        }
        else
        {
            // Select a random item from CommonTier
            return GetRandomSkin(tierList.CommonTier);
        }
   }

   GameObject GetRandomSkin(SkinDetails[] tier)
    {
        int randomIndex = Random.Range(0, tier.Length);
        return tier[randomIndex].Skin;
    }


}
