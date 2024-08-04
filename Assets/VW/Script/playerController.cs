using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class playerController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Die
    }

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Transform movingBar;
    [SerializeField] private Transform correctPositionBar;
    [SerializeField] private RectTransform horizontalBar;

    public GameObject hitBox;
    public ParticleSystem coinVFX;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private State currentState;

    protected PlayerActionsExample playerInput;
    private Vector2 movement;

    private float[] sectionCenters;
    private float sectionWidth;

    private void Awake()
    {
        playerInput = new PlayerActionsExample();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
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

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        coinVFX.Stop();
        hitBox.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.instance.isDead)
        {
            HandleInput();
        }
        HandleState();
    }

    private void HandleInput()
    {
        movement = playerInput.Player.Move.ReadValue<Vector2>();

        if (currentState != State.Die)
        {
            if (movement != Vector2.zero)
            {
                SetState(State.Walk);
            }
            else
            {
                SetState(State.Idle);
            }

            if (playerInput.Player.Jump.triggered)
            {
                SetState(State.Attack);
                //SetState(State.Die); testing
            }
        }
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                animator.Play("biggie_idle");
                break;

            case State.Walk:
                Move();
                animator.Play("biggie_walk");
                break;

            case State.Attack:
                CheckBarPosition();
                SetState(State.Idle);
                break;

            case State.Die:
                animator.Play("biggie_die");
                break;
        }
    }

    public void SetState(State newState)
    {
        // Prevent changing state if the current state is Die
        if (currentState == State.Die)
        {
            return;
        }
        currentState = newState;
    }

    private void Move()
    {
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
    }

    private void CheckBarPosition()
    {
        float movingBarX = movingBar.localPosition.x;
        float correctBarX = correctPositionBar.localPosition.x;

        if (Mathf.Abs(movingBarX - correctBarX) <= 30.0f)
        {
            coinVFX.Play();
            hitBox.SetActive(true);
            SoundManager.instance.Play("X-Coin");
            UpdateCorrectPositionBar();
            StartCoroutine(DisableHitBox(coinVFX.main.duration));
        }
        else
        {
            Debug.Log("Try Again!");
        }
    }
    private IEnumerator DisableHitBox(float duration)
    {
        yield return new WaitForSeconds(duration);
        hitBox.SetActive(false);
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