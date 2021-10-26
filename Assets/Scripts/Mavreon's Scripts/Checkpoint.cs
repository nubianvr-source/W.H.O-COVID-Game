using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    //Properties
    public static bool isLit;
    public static Sprite litSprite;

    public void SetSprite(Sprite sprite)
    {
        if(isLit)
        {
            litSprite = sprite;
            gameObject.GetComponent<Image>().sprite = litSprite;
        }
        else
        {
            litSprite = sprite;
            gameObject.GetComponent<Image>().sprite = litSprite;
            isLit = true;
        }
    }
}
