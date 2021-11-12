using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CovidVirusImageShake : MonoBehaviour
{

    public Image[] covidVirusSprites;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < covidVirusSprites.Length; i++)
        {
            covidVirusSprites[i].transform.DOShakePosition(2.5f,150f,0,95f,false,true).SetLoops(-1, LoopType.Yoyo);
            //Dont forget to Kill the Tween after the button is pressed
        }  
    }

   
}
