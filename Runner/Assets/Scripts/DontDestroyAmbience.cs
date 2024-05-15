using UnityEngine;

public class DontDestroyAmbience : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] ambienceObj = GameObject.FindGameObjectsWithTag("GameAmbience");
        if (ambienceObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }
}
