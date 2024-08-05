using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI comboTextB;
    [SerializeField] private TextMeshProUGUI comboTextF;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private ParticleSystem coinVFX;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private State currentState;

    protected PlayerActionsExample playerInput;
    private Vector2 movement;

    private float[] sectionCenters;
    private float sectionWidth;

    private int comboCount = 0;

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
        comboTextB = GameObject.Find("ComboTextB").GetComponent<TextMeshProUGUI>();
        comboTextF = GameObject.Find("ComboTextF").GetComponent<TextMeshProUGUI>();

        comboTextB.text = "";
        comboTextF.text = "";
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
            }
        }
    }

    private void HandleState()
    {
        if (GameManager.instance.gameData.selectedCharacter == "Biggie")
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
        else if (GameManager.instance.gameData.selectedCharacter == "Biggie1")
        {
            switch (currentState)
            {
                case State.Idle:
                    animator.Play("biggie1_idle");
                    break;

                case State.Walk:
                    Move();
                    animator.Play("biggie1_walk");
                    break;

                case State.Attack:
                    CheckBarPosition();
                    SetState(State.Idle);
                    break;

                case State.Die:
                    animator.Play("biggie1_die");
                    break;
            }
        }
        else if (GameManager.instance.gameData.selectedCharacter == "Biggie3")
        {
            switch (currentState)
            {
                case State.Idle:
                    animator.Play("biggie3_idle");
                    break;

                case State.Walk:
                    Move();
                    animator.Play("biggie3_walk");
                    break;

                case State.Attack:
                    CheckBarPosition();
                    SetState(State.Idle);
                    break;

                case State.Die:
                    animator.Play("biggie3_die");
                    break;
            }
        }
    }

    public void SetState(State newState)
    {
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

        Debug.Log($"Checking bar position: MovingBarX = {movingBarX}, CorrectBarX = {correctBarX}");

        if (Mathf.Abs(movingBarX - correctBarX) <= 40.0f)
        {
            coinVFX.Play();
            hitBox.SetActive(true);
            SoundManager.instance.Play("X-Coin");
            UpdateCorrectPositionBar();
            StartCoroutine(DisableHitBox(coinVFX.main.duration));

            bool enemyHit = hitBox.GetComponent<hitBox>().CheckEnemyHit();
            Debug.Log($"Correct position. Enemy hit: {enemyHit}");

            if (enemyHit)
            {
                comboCount++;
                UpdateComboText();
            }
        }
        else
        {
            comboCount = Mathf.Max(comboCount / 2, 0);
            UpdateComboText();
            Debug.Log("Wrong position or no enemy hit. Combo count halved.");
        }
    }

    private IEnumerator DisableHitBox(float duration)
    {
        yield return new WaitForSeconds(duration);
        hitBox.SetActive(false);
    }

    private void UpdateComboText()
    {
        if (comboCount > 0)
        {
            string comboText = "x" + comboCount;
            comboTextB.text = comboText;
            comboTextF.text = comboText;
        }
        else
        {
            comboTextB.text = "";
            comboTextF.text = "";
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