using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import Unity UI namespace

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public Transform safeZone;
    public Vector3 teleportPosition = new Vector3(-3.87f, 1f, 0f);
    public float movementSpeed = 5f;
    public float jumpScareDistance = 1f;
    public float detectionRadius = 5f;

    private bool isChasingPlayer = false;
    private float checkDetectionTimer = 1f;

    public Transform[] moveSpots;
    private int randomSpot;
    public float speed = 5f;
    public float startWaitTime = 3f;
    private float waitTime;

    public Image jumpScareImage; // Reference to the UI Image for jump scare

    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);

        // Ensure the jump scare image is initially hidden
        if (jumpScareImage != null)
        {
            jumpScareImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            // Check if the square is too close to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < jumpScareDistance)
            {
                // Jump scare logic
                JumpScare();
            }

            // Move the square towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);

            // Limit movement if X coordinate is below -1.11
            if (transform.position.x > -1.11f)
            {
                // Perform other actions if necessary
            }
            else
            {
                // Stop chasing or take alternative action when X coordinate is below -1.11
                isChasingPlayer = false;
                // Or handle any alternative action here
            }
        }
        else
        {
            // If not chasing, wander mindlessly
            WanderMindlessly();
        }

        // Check if the player is within the detection radius every second
        checkDetectionTimer -= Time.deltaTime;
        if (checkDetectionTimer <= 0)
        {
            CheckPlayerDetection();
            checkDetectionTimer = 1f; // Reset the timer
        }
        if (player.position == teleportPosition)
        {
            isChasingPlayer = false;
        }
    }

    void JumpScare()
    {
        Debug.Log("Jump scare! You're too close to the enemy!");

        // Activate the jump scare image
        if (jumpScareImage != null)
        {
            jumpScareImage.gameObject.SetActive(true);

            // Start a coroutine to hide the image after a delay
            StartCoroutine(HideJumpScareImage(2f)); // Change the duration as needed
        }

        // Teleport the player to the specified position
        player.position = teleportPosition;
    }

    IEnumerator HideJumpScareImage(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Deactivate the jump scare image after the delay
        if (jumpScareImage != null)
        {
            jumpScareImage.gameObject.SetActive(false);
        }
    }

    void WanderMindlessly()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
            if (player.position.x < 4f || player.position.y < 4f || player.position.z < 4f)
            {
                isChasingPlayer = false;
            }
        }
    }

    void CheckPlayerDetection()
    {
        // Check if the player is within the detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }
    }
}
