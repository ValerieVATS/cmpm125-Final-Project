
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 respawnPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        respawnPoint = transform.position;
    }

    void Update()
    {
        // Check whether the player is on a platform
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // WASD movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);

        controller.Move(direction * moveSpeed * Time.deltaTime);

        // Jump with Space
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Respawn if the player falls into space
        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPoint = position;
        Debug.Log("Checkpoint activated!");
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;

        velocity = Vector3.zero;
        Debug.Log("Player respawned!");
    }
}
