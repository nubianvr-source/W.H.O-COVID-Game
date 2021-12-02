using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using LocalizationScripts;
using NubianVR.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
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
   
    public static MainAppManager mainAppManager;
    [Header("Character Settings")]    
    public Character[] characters;
    public int characterCarouselIndex;
    public GameObject activeCharacterParentList;
    
    [Header("Language Settings")]
    public int languageIndex;
    public string[] languages;
    public GameObject langListParent;
    [FormerlySerializedAs("languageTests")] public Text[] languageText;
    
    [Header("UI Manager System")]
    [SerializeField] public UI_System uiManager;
    [SerializeField] private UI_Screen languageSelectMenu;
    [SerializeField] private UI_Screen levelSelectMenu;
    [SerializeField] private UI_Screen levelStartMenu;
    
    [Header("Selected Hero Properties")]
    [SerializeField]private Image heroIcon;
    [SerializeField]private TMP_Text heroName;

    [Header("Levels Data")] 
    [SerializeField] public LevelClass[] levels;
    [SerializeField] private GameObject levelButton;
    [SerializeField] private GameObject levelButtonsParent;
    [SerializeField] public Sprite unlockedLevel;
    [SerializeField] public Sprite lockedLevel;
    [SerializeField] private GameObject completeLevelWarning;
    [SerializeField] private TMP_Text levelNumberforWarning;
    [SerializeField] private Transform closePoint;
    [SerializeField] private Transform openPoint;
    private bool warningShowing;
    private bool showWarningTimerStarted;
    [HideInInspector]
    public int selectedLevel;
    
    [HideInInspector]
    public bool loadedFromTriviaLevel = false;
    public float warningTimer = 4.0f;

    [Header("Selected Level Properties")] 
    [SerializeField] private Image heroImage;
    [SerializeField] private TMP_Text levelNumber;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text levelQuestDescription;

    [Header("Badges")] 
    [SerializeField] public Sprite[] level1Badges;
    [SerializeField] public Sprite[] level2Badges;
    [SerializeField] public Sprite[] level3Badges;

    #region Main Methods
 private void Awake()
    {
        mainAppManager = this;
        
        DontDestroyOnLoad(gameObject);
        
        characterCarouselIndex = PlayerPrefs.GetInt("HeroIndex");
        
        languageIndex = PlayerPrefs.GetInt("Lang");

    }

    void Start()
    {
        SelectHero();
        ShowActiveHero();
        warningShowing = false;
        showWarningTimerStarted = false;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("Lang", languageIndex);
        
        ChangeLanguage(PlayerPrefs.GetInt("Lang"));
        ShowSelectedLanguage();
        
        if (showWarningTimerStarted)
        {
            warningTimer -= Time.deltaTime;
            if (warningTimer <= 0.0f)
            {
                CloseLevelWarning();
            }
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
    #endregion

    #region Private Methods

     private void ChangeLanguage(int index)
        {
            foreach (var charIndex in characters)
            {
                charIndex.ChangeLanguage(index);
            }

            foreach (var level in levels)
            {
                level.ChangeLanguage(index);
            }
        }

    #endregion

    #region Public Methods

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
                    languageText[i].color = Color.white;
                }
                else
                {
                    languageText[i].color = Color.black;
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
        
        public void SetHeroIndex()
        {
            print("Hero Index" + characterCarouselIndex);
            PlayerPrefs.SetInt("HeroIndex", characterCarouselIndex);
            SelectHero();
        }
    
        private void SelectHero()
        {
            var selectedHeroIndex = PlayerPrefs.GetInt("HeroIndex");
            heroIcon.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].characterBustImage_Level_1;
            heroName.text = MainAppManager.mainAppManager.characters[selectedHeroIndex].characterName;
                
        }
    
        public void GetStarted()
        {
            uiManager.SwitchScreens(AnalyticsSessionInfo.sessionCount == 1 ? languageSelectMenu : levelSelectMenu);
        }

        public void LevelPrepHandler()
        {
            foreach (var level in levels)
            {
                var levelBtn = Instantiate(levelButton, levelButtonsParent.transform, false);
                var levelButtonInfo = levelBtn.GetComponent<LevelButtonInfo>();
                levelButtonInfo.SetUpBtn(level.levelIcon,level.levelNumber,level.levelTitle,level.sceneToLoad );
          
                if (level.sceneToLoad == 1)
                {
                    levelButtonInfo.levelLockStatus.sprite = unlockedLevel;
                }
                else
                {
                    levelButtonInfo.levelLockStatus.sprite =
                        PlayerPrefs.GetInt($"Level{level.sceneToLoad - 1}Complete") == 0 ? lockedLevel : unlockedLevel;
                    
                    
                }
            }
        }

        public void RemoveAllBtnChildren()
        {
            foreach (Transform child in levelButtonsParent.transform)
            {
                if(child.GetSiblingIndex() != 0)
                    Destroy(child.gameObject);
            }
        }

        public void OpenLevelStartMenu(int selectedBtnSceneIndex)
        {
            var btnSceneIndex = selectedBtnSceneIndex - 1;
            if (selectedBtnSceneIndex == 1)
            {
                heroImage.sprite = characters[characterCarouselIndex].characterHalfImage_Level_1;
                levelNumber.text = levels[btnSceneIndex].levelNumber;
                levelName.text = levels[btnSceneIndex].levelTitle;
                levelQuestDescription.text = levels[btnSceneIndex].levelQuestDesc;
                selectedLevel = btnSceneIndex;
                uiManager.SwitchScreens(levelStartMenu);
                return;
            }

            if (PlayerPrefs.GetInt($"Level{btnSceneIndex}Complete") == 1)
            {
                switch (selectedBtnSceneIndex)
                {
                    case 2:
                        heroImage.sprite = characters[characterCarouselIndex].characterHalfImage_Level_2;
                        break;
                    case  3:
                        heroImage.sprite = characters[characterCarouselIndex].characterHalfImage_Level_3;
                        break;
                    default:
                        heroImage.sprite = null;
                        break;
                }
                levelNumber.text = levels[btnSceneIndex].levelNumber;
                levelName.text = levels[btnSceneIndex].levelTitle;
                levelQuestDescription.text = levels[btnSceneIndex].levelQuestDesc;
                selectedLevel = btnSceneIndex;
                uiManager.SwitchScreens(levelStartMenu);
                    
            }
            else
            {
                if (warningShowing) return;
                levelNumberforWarning.text = levels[btnSceneIndex-1].levelNumber;
                completeLevelWarning.transform.DOMoveY(openPoint.transform.position.y,0.75f,false);
                warningShowing = true;
                showWarningTimerStarted = true;


            }

        }

        public void LoadedFromQuiz()
        {
            if (!loadedFromTriviaLevel) return;
            uiManager.SwitchScreens(levelSelectMenu);
            
        }
        
        public void CloseLevelWarning()
        {
            completeLevelWarning.transform.DOMoveY(closePoint.transform.position.y,0.75f,false);
            warningShowing = false;
            showWarningTimerStarted = false;
            warningTimer = 4.0f;
        }

    
        #endregion
   

   
}
