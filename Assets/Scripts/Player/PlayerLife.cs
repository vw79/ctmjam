using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private Image[] hearts;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private CapsuleCollider2D playerCollider;

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
            hearts[i].enabled = i < playerHealth;
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
        playerCollider.enabled = false;

        for (int i = 0; i < 3; i++)
        {
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        playerCollider.enabled = true;
    }

    private void Die()
    {
        GameManager.instance.isDead = true;
        playerCollider.enabled = false;
        playerController playerController = GetComponent<playerController>();
        if (playerController != null)
        {
            playerController.SetState(playerController.State.Die);
        }
    }

    public void ResetHealth()
    {
        playerHealth = 3;
        playerCollider.enabled = true;
    }
}