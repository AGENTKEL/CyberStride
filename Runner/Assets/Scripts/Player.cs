using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float gravity;
    public Vector2 velocity;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20;
    public float maxXVelocity = 100;
    public float groundHeight = 10;
    public bool isGrounded = false;

    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.0f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float jumpGroundThreshold = 1f;

    public bool isDead = false;

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask glassLayerMask;

    Animator animator;
    float animationSpeed;

    GroundFall fall;
    CameraController cameraController;

    public GameObject obstacleHitSound;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isDead)
        {
            return;
        }


        if (pos.y < -12)
        {
            isDead = true;
            velocity.x = 0;
        }


        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    isGrounded = false;
                    velocity.y = jumpVelocity;
                    isHoldingJump = true;
                    holdJumpTimer = 0.0f;

                    if (fall != null)
                    {
                        fall.player = null;
                        fall = null;
                        cameraController.StopShaking();
                    }
                }


            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                isHoldingJump = false;
            }

        }


    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        if (!isGrounded)
        {

            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.4f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }

                    fall = ground.GetComponent<GroundFall>();
                    if (fall != null)
                    {
                        fall.player = this;
                        cameraController.StartShaking();
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                        isHoldingJump = false;
                    }
                }
            }
            animator.SetBool("Is Grounded", false);
            CheckForGroundCollision(pos);
            CheckForOverheadAndSideCollisions(pos);
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.4f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            if (fall != null)
            {
                rayDistance = -fall.fallSpeed * Time.fixedDeltaTime;
            }
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
            animator.SetBool("Is Grounded", true);

        }

        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                HitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                HitObstacle(obstacle);
            }
        }

        transform.position = pos;
        animationSpeed = velocity.x / 100;
        animator.SetFloat("Player Run Speed", animationSpeed);

        // Adjust the direction and length of the raycast as necessary
        Vector2 direction = Vector2.right; // Assuming you want to check in front of the player
        float length = 1.0f; // How far in front of the player to check for glass

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, velocity.x * Time.fixedDeltaTime, glassLayerMask);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<GlassOnDestroy>().DestroyGlass();
        }
    }

    void HitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        Instantiate(obstacleHitSound);
        velocity.x *= 0.7f;
        StartCoroutine(MovePlayerBack());
    }

    IEnumerator MovePlayerBack()
    {
        float initialX = transform.position.x;
        float targetX = initialX - 4.0f; // Adjust the amount you want to move the player by

        float t = 0.0f;
        float moveDuration = 0.1f; // Adjust the duration of the movement
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / moveDuration;
            float newX = Mathf.Lerp(initialX, targetX, normalizedTime);
            Vector3 newPosition = transform.position;
            newPosition.x = newX;
            transform.position = newPosition;
            yield return null;
        }

        // After moving the player back, smoothly return to the initial position
        t = 0.0f;
        float returnDuration = 1f; // Adjust the duration of the return movement
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / returnDuration;
            float newX = Mathf.Lerp(targetX, initialX, normalizedTime);
            Vector3 newPosition = transform.position;
            newPosition.x = newX;
            transform.position = newPosition;
            yield return null;
        }
    }

    void CheckForOverheadAndSideCollisions(Vector2 position)
    {
        // Overhead collision detection
        RaycastHit2D overheadHit = Physics2D.Raycast(position, Vector2.up, Mathf.Abs(velocity.y * Time.fixedDeltaTime), groundLayerMask);
        if (overheadHit.collider != null)
        {
            // Handle overhead collision (e.g., stop upward movement)
            isHoldingJump = false; ; // Stop vertical movement if hitting something overhead
            velocity.y = -50;
        }

    }

    void CheckForGroundCollision(Vector2 position)
    {
        // Adjust the ray length if necessary
        float rayLength = Mathf.Abs(velocity.x * Time.fixedDeltaTime);
        Vector2 rayStartPosition = position + Vector2.up * 0.2f;

        // Cast a ray to the right to detect ground collision
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, Vector2.right, rayLength, groundLayerMask);
        if (hit.collider != null)
        {
            // Directly use the hit collider to check for ground component without assuming height
            Ground ground = hit.collider.GetComponentInParent<Ground>();
            if (ground != null)
            {
                StopPlayerMovement();
            }
            else
            {
                // Additional check if the hit object is tagged specifically as 'Ground'
                // This is useful if your Ground component is not directly attached to the collider's GameObject
                if (hit.collider.tag == "Ground")
                {
                    StopPlayerMovement();
                }
            }
        }
    }

    void StopPlayerMovement()
    {
        velocity.x = 0;
        isHoldingJump = false;
        // Optionally, set isGrounded to true if you want the player to be able to jump immediately after stopping
        // isGrounded = true;
    }


}
