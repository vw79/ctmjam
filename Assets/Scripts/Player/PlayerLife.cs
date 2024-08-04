using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private Image[] hearts;
    [SerializeField] private SpriteRenderer playerSprite;

    private void Start()
    {
        ResetHealth();
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (playerHealth <= 0)
        {
            Die();
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player took damage. Current health: " + playerHealth);
        UpdateHealthUI();
        StartCoroutine(FlashRed());
        SoundManager.instance.Play("DamagePlayer");
    }

    private IEnumerator FlashRed()
    {
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerSprite.color = Color.white;
    }

    private void Die()
    {
        GameManager.instance.isDead = true;
        playerController playerController = GetComponent<playerController>();
        if (playerController != null)
        {
            playerController.SetState(playerController.State.Die);
        }
    }

    public void ResetHealth()
    {
        playerHealth = 3;
    }
}