using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Finish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finish()
    {
        UnityMessageManager.Instance.SendMessageToRN("Finish");
    }

}
