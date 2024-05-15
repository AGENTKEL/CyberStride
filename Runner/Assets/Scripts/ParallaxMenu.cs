using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    public float depth = 1;

    Player player;


    private void Awake()
    {
    }

    void Start()
    {

    }


    void FixedUpdate()
    {
        float realVelocity = 20 / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= -43.07f)
        {
            pos.x = 100.24f;
        }

        transform.position = pos;
    }
}
