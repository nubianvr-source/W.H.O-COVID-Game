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
        [SerializeField] private UI_Screen languageSelectMenu;
        [SerializeField] private UI_Screen levelSelectMenu;

        //[SerializeField] private float previousScreenTransitionTime = 1.0f; 
        [Header("Hero Properties")]
        [SerializeField]private Image heroIcon;
        [SerializeField]private TMP_Text heroName;
        private int selectedHeroIndex;
        [SerializeField] private Sprite[] heroIcons;
        [SerializeField] private Sprite[] heroNameImages;


        [Header("System Events")] public UnityEvent onSwitchScreen = new UnityEvent();

        #region Unused Properties...
        [Header("Fader Properties")]
        public Image m_Fader;
        public float m_FadeInDuration = 1f;
        public float m_FadeOutDuration = 1f;
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
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log(IntToBool(PlayerPrefs.GetInt("FirstTime")));
                SelectHero();
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
        public void LoadQuizScene(int sceneIndex)
        {
            GameManager.ResetAllStaticData();
            SceneManager.LoadScene(sceneIndex);
        }
        #endregion


        public void SetHeroIndex()
        {
            print("Hero Index" + MainAppManager.mainAppManager.characterCarouselIndex);
            PlayerPrefs.SetInt("HeroIndex", MainAppManager.mainAppManager.characterCarouselIndex);
            SelectHero();
        }

        private void SelectHero()
        {
            selectedHeroIndex = PlayerPrefs.GetInt("HeroIndex");
            if(heroIcons != null && heroNameImages != null)
            {
                heroIcon.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].characterBustImage_Level_1;
                heroName.text = MainAppManager.mainAppManager.characters[selectedHeroIndex].characterName;
            }
        }

        private int BoolToInt(bool val)
        {
            if (val)
                return 1;
            else
                return 0;
        }

        private bool IntToBool(int val)
        {
            if (val != 0)
                return true;
            else
                return false;
        }

        public void GetStarted()
        {
            var count = PlayerPrefs.GetString("unity.player_session_count");
            var launchCount = int.Parse(count);

            SwitchScreens(launchCount == 1 ? languageSelectMenu : levelSelectMenu);
        }

        public void DeleteAllSavedData()
        {
            PlayerPrefs.DeleteAll();
            
            Debug.Log(IntToBool(PlayerPrefs.GetInt("FirstTime")));
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
