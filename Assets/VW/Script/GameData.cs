using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int totalCoinsCollected;
    public string survivalTime;
    public int enemiesKilled;

    public bool unlockedBiggie1;
    public bool unlockedBiggie2;
    public bool unlockedBiggie3;
}
