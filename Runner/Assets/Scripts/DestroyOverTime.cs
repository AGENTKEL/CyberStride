using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float timeToDestroy = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
