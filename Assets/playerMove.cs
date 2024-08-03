using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    private SpriteRenderer spriteRenderer;

    protected PlayerActionsExample playerInput;
    private Vector2 movement;

    private void Awake()
    {
        playerInput = new PlayerActionsExample();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        movement = playerInput.Player.Move.ReadValue<Vector2>();

        // Move the player
        Vector3 move = new Vector3(movement.x, movement.y, 0);
        transform.position += move * Time.deltaTime * playerSpeed;

        // Flip the player's sprite based on movement direction
        if (movement.x > 0)
        {
            spriteRenderer.flipX = true; // Face right
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = false; // Face left
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}