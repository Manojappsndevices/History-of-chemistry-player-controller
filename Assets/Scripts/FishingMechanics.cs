using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingMechanics : MonoBehaviour
{
    [Header("References")]
    public RectTransform movingIndicator;
    public RectTransform greenZone;
    public Button startButton;
    public GameObject gameContainer; // Parent panel for the fishing game

    [Header("Settings")]
    public float baseSpeed = 200f;
    public float speedIncreaseRate = 0.1f;
    public float greenZoneWidth = 100f;
    public float movementRange = 250f; // Manual control of movement range

    private bool isActive;
    private float currentSpeed;
    private Vector2 moveDirection = Vector2.right;
    private int successCount;

    void Start()
    {
        startButton.onClick.AddListener(StartFishing);
        InitializeGame();
    }

    void InitializeGame()
    {
        // Set initial state
        gameContainer.SetActive(true); // Keep panel visible
        greenZone.gameObject.SetActive(true);
        movingIndicator.gameObject.SetActive(false);
        isActive = false;

        startButton.gameObject.SetActive(true); // Reactivate start button

        // Initial green zone position
        RandomizeGreenZonePosition();
    }

    void Update()
    {
        if (!isActive) return;

        // Move indicator
        movingIndicator.anchoredPosition += moveDirection * currentSpeed * Time.deltaTime;

        // Reverse direction at manual movement limits
        if (Mathf.Abs(movingIndicator.anchoredPosition.x) > movementRange)
        {
            moveDirection *= -1;
        }

        // Gradually increase speed
        currentSpeed += currentSpeed * speedIncreaseRate * Time.deltaTime;

        // Check for player input
        if (Input.GetMouseButtonDown(0))
        {
            CheckPosition();
        }
    }

    void StartFishing()
    {
        gameContainer.SetActive(true);
        movingIndicator.gameObject.SetActive(true);
        ResetGame();
        isActive = true;
        startButton.gameObject.SetActive(false); // Hide start button once clicked
    }

    void ResetGame()
    {
        // Reset positions and speed
        movingIndicator.anchoredPosition = Vector2.zero;
        currentSpeed = baseSpeed;
        RandomizeGreenZonePosition();
    }

    void RandomizeGreenZonePosition()
    {
        // Calculate valid position within movement range
        float maxPosition = movementRange - (greenZoneWidth / 2);
        float randomX = Random.Range(-maxPosition, maxPosition);

        greenZone.anchoredPosition = new Vector2(randomX, 0);
        greenZone.sizeDelta = new Vector2(greenZoneWidth, 40);
    }

    void CheckPosition()
    {
        isActive = false;
        float indicatorX = movingIndicator.anchoredPosition.x;
        float greenZoneX = greenZone.anchoredPosition.x;

        bool success = Mathf.Abs(indicatorX - greenZoneX) < (greenZoneWidth / 2);

        if (success)
        {
            successCount++;
            Debug.Log($"Correct! Successes: {successCount}");

            if (successCount >= 3)
            {
                Debug.Log("Game Completed!");
                gameContainer.SetActive(false);
                startButton.gameObject.SetActive(true); // Show start button again
                successCount = 0;
                return;
            }
        }
        else
        {
            Debug.Log("Wrong timing!");
        }

        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        // Reset for next attempt
        movingIndicator.anchoredPosition = Vector2.zero;
        currentSpeed = baseSpeed;
        isActive = true;
        RandomizeGreenZonePosition();
    }
}
