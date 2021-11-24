using System;
using System.Collections;
using System.Collections.Generic;
using LocalizationScripts;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterEnumValue
{
    AYO = 0,
    BACHIR = 1,
    IYANA = 2,
    MARIUS = 3,
    
}

public class MainAppManager : MonoBehaviour
{
    public Character[] characters;
    public static MainAppManager mainAppManager;
    public int characterCarouselIndex;
    public int languageIndex;
    public string[] languages;
    public GameObject langListParent;
    public Text[] languageTests;
    public GameObject activeCharacterParentList;
    private void Awake()
    {
        mainAppManager = this;
        characterCarouselIndex = PlayerPrefs.GetInt("HeroIndex");
        languageIndex = PlayerPrefs.GetInt("Lang");
    }

    void Start()
    {
        ShowSelectedLanguage();
        ShowActiveHero();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("Lang", languageIndex);
        ChangeLanguage(PlayerPrefs.GetInt("Lang"));
    }

    private void ChangeLanguage(int index)
    {
        foreach (var charIndex in characters)
        {
            charIndex.ChangeLanguage(index);
        }
    }

    private void OnEnable()
    {
        LanguageSwap.ChangeLanguage += ChangeLanguage;
    }

    private void OnDisable()
    {
        LanguageSwap.ChangeLanguage -= ChangeLanguage;
    }

    public void SelectLanguageButton()
    {
        PlayerPrefs.SetInt("Lang", languageIndex);
    }
    
    public void ShowSelectedLanguage()
    {
        for (int i = 0; i < langListParent.transform.childCount; i++)
        {
            if (PlayerPrefs.GetInt("Lang") == i)
            {
                langListParent.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
                languageTests[i].color = Color.white;
            }
            else
            {
                languageTests[i].color = Color.black;
                langListParent.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }
    
    public void ShowActiveHero()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            activeCharacterParentList.transform.GetChild(i).GetComponent<Image>().enabled = PlayerPrefs.GetInt("HeroIndex") == i;
        }
    }
}
