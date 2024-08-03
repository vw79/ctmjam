using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Transform movingBar; 
    [SerializeField] private Transform correctPositionBar; 
    [SerializeField] private RectTransform horizontalBar; 
    private SpriteRenderer spriteRenderer;

    protected PlayerActionsExample playerInput;
    private Vector2 movement;

    private float[] sectionCenters;
    private float sectionWidth;

    private void Awake()
    {
        playerInput = new PlayerActionsExample();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CalculateSections();
    }

    private void CalculateSections()
    {
        float width = horizontalBar.rect.width;
        sectionWidth = width / 3;
        sectionCenters = new float[3];
        sectionCenters[0] = -width / 2 + sectionWidth / 2;
        sectionCenters[1] = 0;
        sectionCenters[2] = width / 2 - sectionWidth / 2;
    }

    private void Update()
    {
        movement = playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(movement.x, movement.y, 0);
        transform.position += move * Time.deltaTime * playerSpeed;

        if (movement.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        if (playerInput.Player.Jump.triggered)
        {
            CheckBarPosition();
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

    private void CheckBarPosition()
    {
        float movingBarX = movingBar.localPosition.x;
        float correctBarX = correctPositionBar.localPosition.x;

        if (Mathf.Abs(movingBarX - correctBarX) <= 20.0f)
        {
            Debug.Log("Success!");
            UpdateCorrectPositionBar();
        }
        else
        {
            Debug.Log("Try Again!");
        }
    }

    private void UpdateCorrectPositionBar()
    {
        List<float> otherSections = new List<float>(sectionCenters);
        otherSections.Remove(correctPositionBar.localPosition.x);

        float selectedSectionCenter = otherSections[Random.Range(0, otherSections.Count)];
        float randomXWithinSection = selectedSectionCenter + Random.Range(-sectionWidth / 2, sectionWidth / 2);

        correctPositionBar.localPosition = new Vector3(randomXWithinSection, correctPositionBar.localPosition.y, correctPositionBar.localPosition.z);
    }
}