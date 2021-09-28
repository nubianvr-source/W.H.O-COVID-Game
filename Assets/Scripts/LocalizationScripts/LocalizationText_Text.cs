using System.Collections;
using System.Collections.Generic;
using LocalizationScripts;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationText_Text : MonoBehaviour
{
    public string key;
    
    // Start is called before the first frame update
    void Start()
    {
        //Default Language is English for now.
        ChangeLanguage(0);
    }

    private void ChangeLanguage(int index)
    {
        gameObject.GetComponent<Text>().text = CSVParser.GetTextFromId(key, index);
    }

    private void OnEnable()
    {
        LanguageSwap.ChangeLanguage += ChangeLanguage;
    }

    private void OnDisable()
    {
        LanguageSwap.ChangeLanguage -= ChangeLanguage;
    }
}
