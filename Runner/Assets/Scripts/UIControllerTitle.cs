using UnityEngine;

public class UIControllerTitle : MonoBehaviour
{

    [SerializeField] GameObject musicMenu;
    [SerializeField] GameObject musicGame;
    [SerializeField] GameObject musicOnIcon;
    [SerializeField] GameObject musicOffIcon;

    private void Start()
    {
        Application.targetFrameRate = 120;
    }


    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }


}
