using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

     void Start()
    {
        currentHealth = maxHealth;   
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0) 
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
