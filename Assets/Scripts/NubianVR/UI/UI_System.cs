using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using TMPro;
using System;
using UnityEngine.Analytics;

namespace NubianVR.UI
{
    public class UI_System : MonoBehaviour
    {
        [Header("Main Properties")] 
        public UI_Screen m_StartScreen;
        [SerializeField] private float delayTime = 2.0f;

        [Header("System Events")] public UnityEvent onSwitchScreen = new UnityEvent();

        
        [Header("Fade Properties")]
        public Image m_Fader;
        public float m_FadeInDuration = 1f;
        public float m_FadeOutDuration = 1f;

        #region Variables

        private Component[] screens = new Component[0];

        private UI_Screen _currentScreen;
        public UI_Screen currentScreen => _currentScreen;

        private UI_Screen _previousScreen;
        public UI_Screen previousScreen => _previousScreen;

        [FormerlySerializedAs("sceneNames")] public string[] scenes;

        #endregion

        #region MainMethods

        // Start is called before the first frame update
        void Start()
        {

            screens = GetComponentsInChildren<UI_Screen>(true);
            InitializeScreens();
            if (!m_StartScreen) return;
            SwitchScreens(m_StartScreen);
            print("Screen Initialized");
            
            var _sceneCount = SceneManager.sceneCountInBuildSettings;  
            
            var scenes = new string[_sceneCount];
            
            for( var i = -1; i < _sceneCount; i++ )
            {
                var nextBuildIndex = SceneManager.GetActiveScene().buildIndex + i;
               print(nextBuildIndex); 
            }
        }
        #endregion

        #region HelperMethods

       

        public void SwitchScreens(UI_Screen aScreen)
        {
            StartCoroutine(TransitionToNextScreen(aScreen));
        }

        private IEnumerator TransitionToNextScreen(UI_Screen aScreen)
        {
            yield return new WaitForSeconds(delayTime); //Reference here...
           SwtichScreenMethod(aScreen);
        }

        public void SwtichScreenMethod(UI_Screen aScreen)
        {
            if (aScreen)
            {
                if (_currentScreen)
                {
                    _currentScreen.CloseScreen();
                    _previousScreen = _currentScreen;
                    print("Current Screen closed = " + _currentScreen.name);
                }
                _currentScreen = aScreen;
                _currentScreen.gameObject.SetActive(true);
                _currentScreen.StartScreen();
                print("New Screen = " + _currentScreen.name);
                onSwitchScreen?.Invoke();
            }
        }

        public void ShowModalScreen(UI_Screen modalScreen)
        {
            if (modalScreen)
            {
                if (_currentScreen)
                {
                    _currentScreen.canvasGroup.interactable = false;
                }
                
                modalScreen.gameObject.SetActive(true);
                modalScreen.StartScreen();
            }
        }

        public void CloseModalScreen(UI_Screen modalScreen)
        {
            if (_previousScreen)
            {
                _currentScreen.canvasGroup.interactable = true;
            }
            modalScreen.CloseScreen();
            
        }

        public void GoToPreviousScreen()
        {
            if (_previousScreen)
            {
                SwitchScreens(previousScreen);
            }
        }


        IEnumerator WaitForLoadScene(int sceneIndex)
        {
            yield return null;
        }

        void InitializeScreens()
        {
            foreach (var screen in screens)
            {
                screen.gameObject.SetActive(true);
            }
        }

        public void Quit()
        {
            UnityEngine.Application.Quit();
        }

        //Mavreon's Functions...
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        #endregion

        public void DeleteAllSavedData()
        {
            PlayerPrefs.DeleteAll();
        }

      
         public void FadeIn()
         {
             if (m_Fader)
             {
                 m_Fader.CrossFadeAlpha(0f, m_FadeInDuration, false);
             }
         }

         public void FadeOut()
         {
             if (m_Fader)
             {
                 m_Fader.CrossFadeAlpha(1f, m_FadeOutDuration, true);
             }
         }


    }
}
