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
       StartCovidSpriteTweens();
    }

    public void StartCovidSpriteTweens()
    {
        foreach (var sprite in covidVirusSprites)
        {
            sprite.transform.DOShakePosition(2.5f,150f,0,95f,false,true).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void StopCovidSpriteTweens()
    {foreach (var sprite in covidVirusSprites)
        {
            sprite.transform.DOKill();
        }
        
    }
    


}
