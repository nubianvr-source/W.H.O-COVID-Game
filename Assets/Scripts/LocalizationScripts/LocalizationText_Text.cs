using System;
using System.Collections;
using System.Collections.Generic;
using LocalizationScripts;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationText_Text : MonoBehaviour
{
    public string key;
    private Text textObject;

    private void Awake()
    {
        textObject = gameObject.GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Update()
    {
        //Default Language is English for now.
        ChangeLanguage(PlayerPrefs.GetInt("Lang"));
    }

    private void ChangeLanguage(int index)
    {
        textObject.text = CSVParser.GetTextFromId(key, index);
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
