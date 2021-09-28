using UnityEngine;
using System;
using TMPro;

namespace LocalizationScripts
{
    public class LanguageSwap : MonoBehaviour
    {
        private static int _languageIndex;
        public TMP_Text languageText;
        public static Action<int> ChangeLanguage;

        private void Start()
        {
            
        }

        public void LanguageChanged()
        {
            if (ChangeLanguage != null)
            {
                ChangeLanguage(_languageIndex);
            }
        }


    public void SwapLanguage()
        {
            var availableLanguagesLength = CSVParser.GetAvailableLanguages();
            _languageIndex = (_languageIndex + 1) % availableLanguagesLength.Count;
            if (_languageIndex == 1)
            {
                _languageIndex = 1;
                languageText.text = CSVParser.GetAvailableLanguages()[_languageIndex];
                LanguageChanged();
            }
            else
            {
                _languageIndex = 0;
                languageText.text = CSVParser.GetAvailableLanguages()[_languageIndex];
                LanguageChanged();
            }

        }
    }
}