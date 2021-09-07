using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.fullScreen = false;
    }

    //void Awake()
    //{
    //    UnityMessageManager.Instance.OnMessage += RecieveMessage;
    //}

    //void onDestroy()
    //{
    //    UnityMessageManager.Instance.OnMessage -= RecieveMessage;
    //}

    //void RecieveMessage(string message)
    //{
    //    switch (message)
    //    {
    //        case "TrueFalseModel":
    //            SceneManager.LoadScene(message);
    //            break;
    //        case "OnlineSafetyStoryGame":
    //            SceneManager.LoadScene(message);
    //            break;

    //    }
    //}

    public void GetSceneLoad()
    {
        UnityMessageManager.Instance.SendMessageToRN("SceneToLoad");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("TrueFalseModel");
    }
}
