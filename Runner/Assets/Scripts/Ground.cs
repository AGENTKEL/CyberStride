using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    public float defaultGroundColliderSizeX;
    const float defaultGroundLength = 100f; // Default ground length
    const float shortGroundLength = 50f; // Short ground length
    const float longGroundLength = 150f; // Long ground length
    const float buildingGroundLength = 125f; // Long ground length
    const float shortestGroundLength = 25f; // Long ground length
    BoxCollider2D floorCollider;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public GameObject groundFallSound;

    public GroundTypes[] groundTypes;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        floorCollider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2 + 20;
    }


    void Update()
    {
        groundHeight = transform.position.y + (floorCollider.size.y / 2);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (floorCollider.size.x / 2);

        if (groundRight < -80)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        int groundTypeIndex = Random.Range(0, groundTypes.Length);
        GroundTypes selectedGroundType = groundTypes[groundTypeIndex];

        GameObject go = Instantiate(selectedGroundType.prefab);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float groundLength;
        switch (groundTypeIndex)
        {
            case 0: // First floor in the list (default ground)
                groundLength = defaultGroundLength;
                break;
            case 1: // Second floor in the list (short ground)
                groundLength = shortGroundLength;
                break;
            case 2: // Third floor in the list (long ground)
                groundLength = buildingGroundLength;
                break;
            case 3: // Third floor in the list (long ground)
                groundLength = longGroundLength;
                break;
            case 4: // Third floor in the list (long ground)
                groundLength = buildingGroundLength;
                break;
            case 5: // Third floor in the list (long ground)
                groundLength = shortestGroundLength;
                break;
            default: // In case there are more types, default to standard length
                groundLength = defaultGroundLength;
                break;
        }


        float height1 = player.jumpVelocity * player.maxHoldJumpTime;
        float time = player.jumpVelocity / -player.gravity;
        float height2 = player.jumpVelocity * time + (0.5f * (player.gravity * (time * time)));
        float maxJumpHeight = height1 + height2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 4.3f)
            pos.y = 4.3f;

        float time1 = time + player.maxHoldJumpTime;
        float time2 = Mathf.Sqrt(2.0f * (maxY - actualY) / -player.gravity);
        float totalTime = time1 + time2;
        // Adjust maxX based on the normalization factor
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f; // You can keep or adjust this scaling factor as needed
        maxX += groundRight; // Assuming groundRight is the right edge of the current ground
        float minX = screenRight + 10;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + (groundLength / 2);
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        GroundFall fall = go.GetComponent<GroundFall>();
        if (fall != null)
        {
            Instantiate(groundFallSound);
            Destroy(fall);
            fall = null;
        }

        if (Random.Range (0, 4) == 0)
        {
            fall = go.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(3f, 6f);
        }


        int obstacleNum = Random.Range(0, 2);
        switch (groundTypeIndex)
        {
            case 0: // First floor in the list (default ground)
                for (int i = 0; i < obstacleNum; i++)
                {

                    GameObject box = Instantiate(boxTemplate.gameObject);
                    float y = goGround.groundHeight;
                    float halfWidth = goCollider.size.x / 2 - 4;
                    float left = go.transform.position.x - halfWidth;
                    float right = go.transform.position.x + halfWidth;
                    float x = Random.Range(left, right);
                    Vector2 boxPos = new Vector2(x, y);
                    box.transform.position = boxPos;

                    if (fall != null)
                    {
                        Obstacle o = box.GetComponent<Obstacle>();
                        fall.obstacles.Add(o);
                    }
                }

                break;
            case 1: // Second floor in the list (short ground)

                break;
            case 2: // Third floor in the list (long ground)
                for (int i = 0; i < obstacleNum; i++)
                {

                    GameObject box = Instantiate(boxTemplate.gameObject);
                    float y = goGround.groundHeight;
                    float halfWidth = goCollider.size.x / 2 - 4;
                    float left = go.transform.position.x - halfWidth;
                    float right = go.transform.position.x + halfWidth;
                    float x = Random.Range(left, right);
                    Vector2 boxPos = new Vector2(x, y);
                    box.transform.position = boxPos;

                    if (fall != null)
                    {
                        Obstacle o = box.GetComponent<Obstacle>();
                        fall.obstacles.Add(o);
                    }
                }

                break;
            case 3: // Third floor in the list (long ground)
                for (int i = 0; i < obstacleNum; i++)
                {

                    GameObject box = Instantiate(boxTemplate.gameObject);
                    float y = goGround.groundHeight;
                    float halfWidth = goCollider.size.x / 2 - 4;
                    float left = go.transform.position.x - halfWidth;
                    float right = go.transform.position.x + halfWidth;
                    float x = Random.Range(left, right);
                    Vector2 boxPos = new Vector2(x, y);
                    box.transform.position = boxPos;

                    if (fall != null)
                    {
                        Obstacle o = box.GetComponent<Obstacle>();
                        fall.obstacles.Add(o);
                    }
                }

                break;
            case 4: // Third floor in the list (long ground)
                for (int i = 0; i < obstacleNum; i++)
                {

                    GameObject box = Instantiate(boxTemplate.gameObject);
                    float y = goGround.groundHeight;
                    float halfWidth = goCollider.size.x / 2 - 4;
                    float left = go.transform.position.x - halfWidth;
                    float right = go.transform.position.x + halfWidth;
                    float x = Random.Range(left, right);
                    Vector2 boxPos = new Vector2(x, y);
                    box.transform.position = boxPos;

                    if (fall != null)
                    {
                        Obstacle o = box.GetComponent<Obstacle>();
                        fall.obstacles.Add(o);
                    }
                }

                break;
            case 5: // Third floor in the list (long ground)
                break;
            default: // In case there are more types, default to standard length
                for (int i = 0; i < obstacleNum; i++)
                {

                    GameObject box = Instantiate(boxTemplate.gameObject);
                    float y = goGround.groundHeight;
                    float halfWidth = goCollider.size.x / 2 - 4;
                    float left = go.transform.position.x - halfWidth;
                    float right = go.transform.position.x + halfWidth;
                    float x = Random.Range(left, right);
                    Vector2 boxPos = new Vector2(x, y);
                    box.transform.position = boxPos;

                    if (fall != null)
                    {
                        Obstacle o = box.GetComponent<Obstacle>();
                        fall.obstacles.Add(o);
                    }
                }

                break;
        }



    }
}
