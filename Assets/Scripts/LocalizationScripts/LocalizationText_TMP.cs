using System;
using LocalizationScripts;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]

// Unfortunately I made the dumb mistake of having two different UI text so we now
// have two different classes for each text component type.
// Please indulge me if you do have the time to fix it. :)

public class LocalizationText_TMP : MonoBehaviour
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
        gameObject.GetComponent<TMP_Text>().text = CSVParser.GetTextFromId(key, index);
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
