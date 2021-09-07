using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.fullScreen = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadExitScene()
    {
        SceneManager.LoadScene("ExitScene");
    }
}
