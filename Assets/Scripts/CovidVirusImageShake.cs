using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CovidVirusImageShake : MonoBehaviour
{

    public Image[] covidVirusSprites;
    public Image[] finishScreenBackgroundProps;
    
    // Start is called before the first frame update
    void Start()
    {
       
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

    public void StartSpinningBackgroundElements()
    {
        foreach (var props in finishScreenBackgroundProps)
        {
            props.transform.DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void StopSpinningBackgroundElements()
    {
        foreach (var props in finishScreenBackgroundProps)
        {
            props.transform.DOKill();
        }
    }



}
