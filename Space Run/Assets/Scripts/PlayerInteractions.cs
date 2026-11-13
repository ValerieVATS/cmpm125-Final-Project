
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public int starsCollected = 0;
    public int requiredStars = 3;

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
}
