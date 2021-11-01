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

namespace NubianVR.UI
{
    public class UI_System : MonoBehaviour
    {
        [Header("Main Properties")] 
        public UI_Screen m_StartScreen;
        [SerializeField] private TMP_Text noOfQuestionText;
        [SerializeField] private float delayTime = 2.0f;
        [SerializeField] private float previousScreenTransitionTime = 1.0f; 

        [Header("System Events")] public UnityEvent onSwitchScreen = new UnityEvent();

        #region Unused Properties...
        /*[Header("Fader Properties")]
        public Image m_Fader;
        public float m_FadeInDuration = 1f;
        public float m_FadeOutDuration = 1f;*/
        #endregion

        #region Variables

        private Component[] screens = new Component[0];

        private UI_Screen _currentScreen;
        public UI_Screen currentScreen => _currentScreen;

        private UI_Screen _previousScreen;
        public UI_Screen previousScreen => _previousScreen;

        [FormerlySerializedAs("sceneNames")] public string[] scenes;

        [Header("Finish Screen Parameters")]
        

        [Header("Sprite Swap")]
        [SerializeField] private Sprite newBackgroundSprite;
        [SerializeField] private Sprite newButtonSprite;

        #endregion

        #region MainMethods

        // Start is called before the first frame update
        void Start()
        {
            if (noOfQuestionText)
            {
                Object[] questionObject = Resources.LoadAll("Level1Questions", typeof(Questions));
                noOfQuestionText.text = questionObject.Length.ToString();
            }
                

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

            /*if (m_Fader)
            {
                m_Fader.gameObject.SetActive(true);
            }
            FadeIn();*/

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

       /* //Mavreon's Duplicate...
        public void SwitchScreensInTime(UI_Screen aScreen, float transitionTime)
        {
            StartCoroutine(TransitionToNextScreen1(aScreen, transitionTime));
        }

        private IEnumerator TransitionToNextScreen1(UI_Screen aScreen, float transitionTime)
        {
            yield return new WaitForSeconds(transitionTime); //Reference here...
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
        }*/

        public void GoToPreviousScreen()
        {
            if (_previousScreen)
            {
                SwitchScreens(previousScreen);
                //SwitchScreens1(previousScreen, previousScreenTransitionTime);
            }
        }

        /*public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }*/

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
        public void LoadQuizScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        #endregion

        #region Unused Code...
        /* public void FadeIn()
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
         }*/
        #endregion

    }
}
