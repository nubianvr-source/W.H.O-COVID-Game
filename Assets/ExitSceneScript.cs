using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitSceneScript : MonoBehaviour
{

    private void Start()
    {
        Finish();
    }
  
    public void Finish()
    {
        Screen.fullScreen = false;
        UnityMessageManager.Instance.SendMessageToRN("Finish");
        Debug.Log("Finish");
        SceneManager.LoadScene("StartScene");
        
    }

   
}
