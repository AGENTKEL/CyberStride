using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);   
        }
        DontDestroyOnLoad(this.gameObject);

    }
}
