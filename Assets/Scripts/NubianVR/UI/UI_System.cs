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
        
        [Header("System Events")] public UnityEvent onSwitchScreen = new UnityEvent();

        /*[Header("Fader Properties")]
        public Image m_Fader;
        public float m_FadeInDuration = 1f;
        public float m_FadeOutDuration = 1f;*/


        #region Variables

        private Component[] screens = new Component[0];

        private UI_Screen _currentScreen;
        public UI_Screen currentScreen => _currentScreen;

        private UI_Screen _previousScreen;
        public UI_Screen previousScreen => _previousScreen;

        [FormerlySerializedAs("sceneNames")] public string[] scenes;

        [Header("Finish Screen Parameters")]
        [SerializeField] private float delayTime = 2.0f;

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

        // Update is called once per frame
        void Update()
        {

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

        public void GoToPreviousScreen()
        {
            if (_previousScreen)
            {
                SwitchScreens(previousScreen);
            }
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
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

        #endregion
       
        //Mavreon's Functions...
        public void LoadQuizScene()
        {
            SceneManager.LoadScene(1);
        }

    }
}
