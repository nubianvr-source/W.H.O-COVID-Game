using System;
using System.Collections;
using System.Collections.Generic;
using NubianVR.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeLanguageBtn : MonoBehaviour
{  
    public Image englishButtonImage;
    public Image frenchButtonImage;
    private int _langIndex;
    public UI_Screen modalScreen;
    

    public void ChangeToEnglishBtn()
    {
        if (englishButtonImage.enabled) return;
        _langIndex = englishButtonImage.gameObject.transform.GetSiblingIndex();
        MainAppManager.mainAppManager.uiManager.ShowModalScreen(modalScreen);
    }

    public void ChangeToFrenchBtn()
    {
        if (frenchButtonImage.enabled) return;
        _langIndex = frenchButtonImage.gameObject.transform.GetSiblingIndex();
        MainAppManager.mainAppManager.uiManager.ShowModalScreen(modalScreen);
    }

    public void ConfirmLanguageChangeBtn()
    {
        MainAppManager.mainAppManager.languageIndex = _langIndex;
        //MainAppManager.mainAppManager.ShowSelectedLanguage();
    }
}
