using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;


[System.Serializable]
public class SkinDetails{
    public GameObject Skin;
    public Sprite skinImg;
    public string name;
    public bool isObtained;

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
    public SkinDetails[] skinDetails;

    [Header("Probability")]
    [SerializeField] [Range(0,1)] private float commonProbability ;
    [SerializeField] [Range(0,1)] private float uncommonProbability ;
    [SerializeField] [Range(0,1)] private float rareProbability ;

    [Header("Buttons")]
    [SerializeField] private GameObject tryBtnObject ;
    [SerializeField] private Button tryBtn ;

    [Header("Claw Anim")]
    [SerializeField] private Animator clawAnimController ;
    [SerializeField] private AnimationClip clawAnim ;

    [Header("Prize Screen")]
    [SerializeField] private GameObject mainScreen ;
    [SerializeField] private GameObject PrizeScreen ;
    private TextMeshProUGUI SkinName;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private Image ImgHolder;
    private SpriteRenderer imgRenderer;

    [Header("Misc")]
    //random index for skin details
    private int randomIndex ;

    private void Start() {
        tryBtn.onClick.AddListener(OnHandleClick);
    }
    private void OnHandleClick(){
        clawAnimController.Play(clawAnim.name);
        StartCoroutine(ShowSkin());
    }
    private IEnumerator ShowSkin(){
        GameObject skin = RandomGacha();
        yield return new WaitForSeconds(2);
        Debug.Log("You got "+skin);
        prize();
        StopAllCoroutines();
    }

    private void prize(){
        mainScreen.SetActive(false);
        PrizeScreen.SetActive(true);
        confetti.Play();
        SkinName.text = skinDetails[randomIndex].name;
        imgRenderer = ImgHolder.GetComponent<SpriteRenderer>();
        imgRenderer.sprite = skinDetails[randomIndex].skinImg;  
    }

   private GameObject RandomGacha(){

    float num = UnityEngine.Random.Range(0f,1f);
    print(num);

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
        tier[randomIndex].isObtained = true;
        return tier[randomIndex].Skin;
    }


}
