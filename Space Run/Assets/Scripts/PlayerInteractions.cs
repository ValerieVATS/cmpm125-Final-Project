
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Star Collection")]
    public int starsCollected = 0;
    public int requiredStars = 3;

    [Header("UI")]
    public TMP_Text starCounterText;

    [Header("Level Progression")]
    public GameObject exitGate;

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        if (exitGate != null)
        {
            exitGate.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            starsCollected++;

            UpdateStarCounter();

            Debug.Log("Stars: " +
                starsCollected + "/" + requiredStars);

            Destroy(other.gameObject);

            if (starsCollected >= requiredStars)
            {
                Debug.Log("All stars collected! Exit unlocked.");

                if (exitGate != null)
                {
                    exitGate.SetActive(true);
                }
            }
        }

        if (other.CompareTag("Checkpoint"))
        {
            movement.SetCheckpoint(
                other.transform.position + Vector3.up * 1.5f
            );
        }

        if (other.CompareTag("Hazard"))
        {
            movement.Respawn();
        }

        if (other.CompareTag("Finish"))
        {
            if (starsCollected >= requiredStars)
            {
                Debug.Log("LEVEL COMPLETE!");
            }
        }
    }
    // Updates the visible star counter
    void UpdateStarCounter()
    {
        if (starCounterText != null)
        {
            starCounterText.text =
                "Stars: " + starsCollected + "/" + requiredStars;
        }
    }
}
