using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Transactions;
using DG.Tweening;
using LocalizationScripts;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using NubianVR.UI;
using TMPro;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    #region Declarations
    private Questions[] questions;
    private List<Questions> _unansweredQuestions;
    private Questions _currentQuestion;
    
    private int playerLife = 3;
    

    [Header("TMP Values")]
    [SerializeField] private TMP_Text questionTmpText;
    [SerializeField] private TMP_Text numberOfQuestionsAnsweredText;
    [SerializeField] private TMP_Text trueAnswerTmpText;
    [SerializeField] private TMP_Text falseAnswerTmpText;
    [SerializeField] private TMP_Text countdownValueTmpText;
    

    [Header("Game play Values")]
    private  int _numberOfQuestionsAnswered = 1;
    public  int _numberOfQuestionsToAsk;
    private  int correctAnswers;
    private  int wrongAnswers;
    [SerializeField] private float delayTime = 3.0f;
    [SerializeField]private float countdownValue = 6.0f;
    private float countdownBaseValue;
    private float totalTimeTaken = 0.0f;
   

    [Header("UI System")]
    [SerializeField] private Animator baseAnimator;
    [SerializeField] private UI_System UIManager;
    [SerializeField] private UI_Screen finishScreen;
    [SerializeField] private UI_Screen levelUpScreen; 
    
    

    [Header("Buttons")]
    [SerializeField] private Button trueButton;
    [SerializeField] private Image trueInnerBtn;
    [SerializeField] private Button falseButton;
    [SerializeField] private Image falseInnerBtn;
    [SerializeField] private Button tryAgainBtn;
    [SerializeField] private Button shareBtn;


    [Header("Images")]
    [SerializeField] private Image questionImage;
    [SerializeField] private Image countdownRadialBarMask;
    public Image heroIcon;
    
    [Header("Sprites")]
    [SerializeField] private Sprite incorrectCheckpointSprite;
    [SerializeField] private Sprite correctCheckpointSprite;
    [SerializeField] private Sprite neutralCheckpointSprite;
    


    [Header("Progress Bar")]
    [SerializeField] private Image progressBarMask;
    private float progressPoint = 0.0f;
    [SerializeField] private GameObject checkPointsParents;
    
    [Header("Badges")]
    private int noOfBadgesWon;

    private int noOfBadgesPlayedThisLevel;
    [Header("Badges")] 
    [SerializeField] private Badges[] level1Badges;
    [SerializeField] private Badges[] level2Badges;



    [Header("Level Summary Screen")]
    [SerializeField] private TMP_Text congratulatoryText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private TMP_Text passText;
    [SerializeField] private Text correctAnswersText;
    [SerializeField] private Text wrongAnswersText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text badgesWonText;
    [SerializeField] private LocalizationText_TMP badgeText;
    [SerializeField] private Image[] badgeImages;

    [Header("Level Up Screen")] [SerializeField]
    private GameObject fakeBGSprites;

    private SoundManager _soundManager;

    private int alreadyEarned;
    private int selectedHeroIndex;
    private bool timerStopped;
    private GameObject currentQuestionCheckpoint;
    #endregion

    public ParticleSystem[] confetti;
    public Image redVignette;

    private static bool levelRestarted = true;

    private void Start()
    {
        _soundManager = SoundManager.instance;
     
    }

    public void OnStart()
    { 
        
        _soundManager.Stop("BGMusic");
        alreadyEarned = 0;
        ResetProgressBar();
        ResetData();
        switch (MainAppManager.mainAppManager.selectedLevel + 1)
        {
            case 1:
                heroIcon.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                    .characterBustImage_Level_1;
                break;
            case 2:
                heroIcon.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                    .characterBustImage_Level_2;
                break;
            case 3:
                heroIcon.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                    .characterBustImage_Level_3;
                break;
            default:
                break;
        }

       SetCurrentQuestion();
    }

    public void ResetProgressBar()
    {
        progressBarMask.fillAmount = 0f;
        switch (MainAppManager.mainAppManager.selectedLevel + 1 )
        {
            case 1:
            {
                for (int i = 0; i < checkPointsParents.transform.childCount; i++)
                {
                    switch (i + 1)
                    {
                        case 4:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                level1Badges[0].unlitBadge;
                            break;
                        case 7:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                level1Badges[1].unlitBadge;
                            break;
                        case 10:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                level1Badges[2].unlitBadge;
                            break;
                        default:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                neutralCheckpointSprite;
                            break;
                    }
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < checkPointsParents.transform.childCount; i++)
                {
                    switch (i + 1)
                    {
                        case 4:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                level2Badges[0].unlitBadge;
                            break;
                        case 7:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                               level2Badges[1].unlitBadge;
                            break;
                        case 10:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                level2Badges[2].unlitBadge;
                            break;
                        default:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                neutralCheckpointSprite;
                            break;
                    }
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < checkPointsParents.transform.childCount; i++)
                {
                    switch (i + 1)
                    {
                        case 4:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[0]
                                    .unlitBadge;
                            break;
                        case 7:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[1]
                                    .unlitBadge;
                            break;
                        case 10:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[2]
                                    .unlitBadge;
                            break;
                        default:
                            checkPointsParents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite =
                                neutralCheckpointSprite;
                            break;
                    }
                }
                break;
            }
                
        }
        

        
    }

    private void SetCurrentQuestion()
    {
        countdownValue = 30f;
        timerStopped = false;
        countdownValueTmpText.text = countdownValue.ToString();
        countdownBaseValue = countdownValue;
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            //With all the static variables you will eventually need a reset function that runs when the game is starts to prevent data crossing from different scenes
            //It can be placed here after the question list check and before the question loads.
            LoadQuestions();
        }
        

        trueInnerBtn.DOColor(Color.white, 0.1f);
        falseInnerBtn.DOColor(Color.white, 0.1f);
        ProgressBarHandler();

        if (countdownRadialBarMask)
            countdownRadialBarMask.fillAmount = 1.0f;
        if (numberOfQuestionsAnsweredText)
            numberOfQuestionsAnsweredText.text = _numberOfQuestionsAnswered.ToString();
        EnableButtons(true);
        InvokeRepeating(nameof(StartCountdown),
            1.5f,
            1.0f);
        _soundManager.PlayAudio("ClockTick");
        _currentQuestion = _unansweredQuestions[0];
        if (questionTmpText)
            questionTmpText.text = _currentQuestion.textQuestion;
        if (questionImage)
            questionImage.sprite = _currentQuestion.imageQuestion;
        if (trueAnswerTmpText)
            trueAnswerTmpText.text = _currentQuestion.trueAnswerText;
        if (falseAnswerTmpText)
            falseAnswerTmpText.text = _currentQuestion.falseAnswerText;

        if (MainAppManager.mainAppManager.selectedLevel + 1 == 3)
        {
            _currentQuestion.questionBadge = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .finalCharacterBadges[noOfBadgesPlayedThisLevel];
        }

        
        EnableButtons(true);
        
    }

    

    //Update Loop just updates the players score visually during gameplay.
    private void Update()
    {
        selectedHeroIndex = PlayerPrefs.GetInt("HeroIndex");
        if(!timerStopped)
        //Helped with the smooth timer animation. Added plus one to the countdownBaseValue to account for the timer ending on zero
            countdownRadialBarMask.fillAmount -= 1.0f / (countdownBaseValue + 1f) * Time.deltaTime;
        
    }

    //To be possible removed and every instance stated would be replase with TransitionTo NextQuestion...
    public void LoadNextQuestion()
    {
        //Executed based on the health value...
        if (playerLife > 0)
            StartCoroutine(TransitionToNextQuestion());
        else
            StartCoroutine(FinishScreenHandler());
    }

   
    IEnumerator TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;
        _unansweredQuestions.Remove(_currentQuestion);
        
        yield return new WaitForSeconds(delayTime);
        currentQuestionCheckpoint.transform.DOScale(new Vector3(1f, 1f), 0.25f);
        PresentQuestion();
    }

    private void LoadQuestions()
    {
        
       //ChangeLanguage(PlayerPrefs.GetInt("Lang"));
       switch (MainAppManager.mainAppManager.levels[MainAppManager.mainAppManager.selectedLevel].levelNumberKey)
       {
           case "ID_MMenuLevel1Text":
               Object[] questionObjectLevel1 = Resources.LoadAll("Questions/Level1", typeof(Questions));
               questions = new Questions[questionObjectLevel1.Length];
               for (int i = 0; i < questionObjectLevel1.Length; i++)
               {
                   questions[i] = (Questions) questionObjectLevel1[i];
                   questions[i].ChangeLanguage(PlayerPrefs.GetInt("Lang"));
               }

               _unansweredQuestions = questions.ToList();
               _numberOfQuestionsToAsk = questions.Length;
               break;
           case "ID_MMenuLevel2Text":
               Object[] questionObjectLevel2 = Resources.LoadAll("Questions/Level2", typeof(Questions));
               questions = new Questions[questionObjectLevel2.Length];
               for (int i = 0; i < questionObjectLevel2.Length; i++)
               {
                   questions[i] = (Questions) questionObjectLevel2[i];
                   questions[i].ChangeLanguage(PlayerPrefs.GetInt("Lang"));
               }

               _unansweredQuestions = questions.ToList();
               _numberOfQuestionsToAsk = questions.Length;
               break;
           case  "ID_MMenuLevel3Text":
               Object[] questionObjectLevel3 = Resources.LoadAll("Questions/Level3", typeof(Questions));
               questions = new Questions[questionObjectLevel3.Length];
               for (int i = 0; i < questionObjectLevel3.Length; i++)
               {
                   questions[i] = (Questions) questionObjectLevel3[i];
                   questions[i].ChangeLanguage(PlayerPrefs.GetInt("Lang"));
               }

               _unansweredQuestions = questions.ToList();
               _numberOfQuestionsToAsk = questions.Length;
               break;
           default:
               break;
               
       }
       
       
    }

    private void ChangeLanguage(int index)
    {
        foreach (var question in questions)
        {
            question.ChangeLanguage(index);
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

    //Player selects the true button
    public void UserSelectsTrue()
    {
        EndCountdown();
        if (_currentQuestion.isClickTrue)
        {
            //correct...
            _soundManager.PlaySFX("CorrectClick");
            PlayAllConfetti();
            correctAnswers += 1;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            trueInnerBtn.DOColor(new Color(0f / 255f, 211f / 255f, 57f / 255f), 0.5f);
            if (_currentQuestion.isBadgeWorthy)
            {
                if (MainAppManager.mainAppManager.selectedLevel+1 == 3)
                {
                    currentQuestionCheckpoint.GetComponent<Image>().sprite = MainAppManager.mainAppManager
                        .characters[selectedHeroIndex].finalCharacterBadges[noOfBadgesPlayedThisLevel].litBadge;
                    noOfBadgesPlayedThisLevel++;
                    noOfBadgesWon++;
                    UpdateBadgesFound();
                }
                else
                {
                    currentQuestionCheckpoint.GetComponent<Image>().sprite = _currentQuestion.questionBadge.litBadge;
                    noOfBadgesPlayedThisLevel++;
                    noOfBadgesWon++;
                    UpdateBadgesFound();
                }
               
            }
            else
            {
                currentQuestionCheckpoint.GetComponent<Image>().sprite = correctCheckpointSprite;
            }
            
        }
        else
        {
            //false
            playerLife--;
            _soundManager.PlaySFX("WrongClick");
            PlayRedVignetteAnimation();
            trueInnerBtn.DOColor(new Color(246f / 255f, 40f / 255f, 40f / 255f), 0.5f);
            wrongAnswers ++;
            if (_currentQuestion.isBadgeWorthy)
            {
                noOfBadgesPlayedThisLevel++;
            }
            else
            {
                currentQuestionCheckpoint.GetComponent<Image>().sprite = incorrectCheckpointSprite;
                
            }
  
            
        }
        //ProgressBarHandler();
    }


    //Player selects the false button
    public void UserSelectsFalse()
    {
        EndCountdown();
        if (!_currentQuestion.isClickTrue)
        {
            //correct
            _soundManager.PlaySFX("CorrectClick");
            PlayAllConfetti();
            correctAnswers ++;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            falseInnerBtn.DOColor(new Color(0f / 255f, 211f / 255f, 57f / 255f), 0.5f);
            if (_currentQuestion.isBadgeWorthy)
            {
                if (MainAppManager.mainAppManager.selectedLevel+1 == 3)
                {
                    currentQuestionCheckpoint.GetComponent<Image>().sprite = MainAppManager.mainAppManager
                        .characters[selectedHeroIndex].finalCharacterBadges[noOfBadgesPlayedThisLevel].litBadge;
                    noOfBadgesPlayedThisLevel++;
                    noOfBadgesWon++;
                    UpdateBadgesFound();
                }
                else
                {
                    currentQuestionCheckpoint.GetComponent<Image>().sprite = _currentQuestion.questionBadge.litBadge;
                    noOfBadgesPlayedThisLevel++;
                    noOfBadgesWon++;
                    UpdateBadgesFound();
                }
            }
            else
            {
                currentQuestionCheckpoint.GetComponent<Image>().sprite = correctCheckpointSprite;
            }

        }
        else
        {
            //false
            playerLife--;
            _soundManager.PlaySFX("WrongClick");
            PlayRedVignetteAnimation();
            falseInnerBtn.DOColor(new Color(246f / 255f, 40f / 255f, 40f / 255f), 0.5f);
            wrongAnswers++;
            if (_currentQuestion.isBadgeWorthy)
            {
                noOfBadgesPlayedThisLevel++;
            }
            else
            {
                  currentQuestionCheckpoint.GetComponent<Image>().sprite = incorrectCheckpointSprite;
            }

        }
        //ProgressBarHandler();
    }

    public void UpdateBadgesFound()
    {
        switch (MainAppManager.mainAppManager.selectedLevel + 1)
        {
            case 1:
            {
                for (int i = 0; i < level1Badges.Length; i++)
                {
                    if (_currentQuestion.questionBadge.litBadge == level1Badges[i].litBadge)
                    {
                        if (PlayerPrefs.GetInt($"Level1Badge{i + 1}") == 1)
                        {
                            alreadyEarned += 1;
                        }
                        else
                        {
                            PlayerPrefs.SetInt($"Level1Badge{i+1}",1);
                        }

                        
                    }
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < level2Badges.Length; i++)
                {
                    if (_currentQuestion.questionBadge.litBadge == level2Badges[i].litBadge)
                    {
                        if (PlayerPrefs.GetInt($"Level2Badge{i + 1}") == 1)
                        {
                            alreadyEarned += 1;
                        }
                        else
                        {
                            PlayerPrefs.SetInt($"Level2Badge{i+1}",1);
                        }

                        
                    }
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges.Length; i++)
                {
                    if (_currentQuestion.questionBadge.litBadge == MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[i].litBadge)
                    {
                        if (PlayerPrefs.GetInt($"Level3Badge{i + 1}") == 1)
                        {
                            alreadyEarned += 1;
                        }
                        else
                        {
                            PlayerPrefs.SetInt($"Level3Badge{i+1}",1);
                        }

                        
                    }
                }
                break;
            }
        }
    }

    public void EnableButtons(bool condition) 
    {
        if(trueButton && falseButton)
        {
            trueButton.interactable = condition;
            falseButton.interactable = condition;
        }
    }

    public void PresentQuestion()
    {
        if (_numberOfQuestionsAnswered > _numberOfQuestionsToAsk)
            StartCoroutine(FinishScreenHandler());
        else
        {
            SetCurrentQuestion();
           
        }

        
    }

    IEnumerator ReloadSceneForNextQuestion(float waitTimeToLoadNextQuestion)
    { 
        yield return new WaitForSeconds(waitTimeToLoadNextQuestion);
        baseAnimator.SetTrigger("hide");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayAllConfetti()
    {
        
        foreach (var localConfetti in confetti)
        {
            localConfetti.Play();
        }
        _soundManager.PlaySFX("CorrectCelebration");
    }

    public void PlayRedVignetteAnimation()
    {
        redVignette.DOFade(255f, 1.0f);
        _soundManager.PlaySFX("WrongPrompt");
        redVignette.DOFade(0, 1.0f);
        _soundManager.PlaySFX("WrongDisappointment");
    }

    private void StartCountdown()
    {
        if(countdownValue > 0)
        {
            countdownValue--;
            countdownValueTmpText.text = countdownValue.ToString();
        }
        else
        {
            wrongAnswers++;
            currentQuestionCheckpoint.GetComponent<Image>().sprite = incorrectCheckpointSprite;
            //ProgressBarHandler();
            _soundManager.PlaySFX("TimeOut");
            EndCountdown();
            EnableButtons(false);
            LoadNextQuestion();
        }
    }

    private void EndCountdown()
    {
        CancelInvoke("StartCountdown");
        _soundManager.Stop("ClockTick");
        timerStopped = true;
        totalTimeTaken += (countdownBaseValue - countdownValue);
        Debug.Log("Time taken to answer question : " + totalTimeTaken + " secs");
    }

    private void ProgressBarHandler()
    {
        progressPoint = _numberOfQuestionsAnswered / (float)_numberOfQuestionsToAsk;
        progressBarMask.DOFillAmount(progressPoint, 0.75f);
        print("Progress Point: " + progressPoint);

        for (int i = 0; i < checkPointsParents.transform.childCount; i++)
        {
            if (_numberOfQuestionsAnswered - 1 == i)
            {
                currentQuestionCheckpoint = checkPointsParents.transform.GetChild(i).gameObject;
                currentQuestionCheckpoint.transform.DOScale(new Vector3(1.5f, 1.5f), 0.25f);
            }
        }

    }

  
    private void LevelSummaryHandler()
    {     
        var congratsLocalizationTextTmp = congratulatoryText.gameObject.GetComponent<LocalizationText_TMP>();
        
        var passTextLocalizationTextTmp = passText.gameObject.GetComponent<LocalizationText_TMP>();

        

        var newBadgesEarned = 0;
        var seconds = Mathf.FloorToInt(totalTimeTaken % 60) < 10
            ? $"0{Mathf.FloorToInt(totalTimeTaken % 60)}"
            : Mathf.FloorToInt(totalTimeTaken % 60).ToString(); 

        totalTimeText.text = $"{Mathf.FloorToInt(totalTimeTaken / 60)}:{seconds}";

        if (playerLife > 0)
        {

            switch (MainAppManager.mainAppManager.selectedLevel + 1)
            {
                case 1:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_1;
                    passTextLocalizationTextTmp.key = "ID_PassTextLevel1";
                    PlayerPrefs.SetInt("Level1Complete", 1);
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level1Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = level1Badges[i].litBadge;
                        }
                    }
                    break;
                case 2:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_2;
                    passTextLocalizationTextTmp.key = "ID_PassTextLevel2";
                    PlayerPrefs.SetInt("Level2Complete", 1);
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level2Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = level2Badges[i].litBadge;
                        }
                    }
                    break;
                case 3:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_3;
                    passTextLocalizationTextTmp.key = "ID_PassTextLevel3";
                    PlayerPrefs.SetInt("Level3Complete", 1);
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level3Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[i].litBadge;
                        }
                    }
                    break;
                default:
                    break;
            }

           
            congratsLocalizationTextTmp.key = "ID_LReviewGJText";
            correctAnswersText.text = correctAnswers.ToString();
            wrongAnswersText.text = wrongAnswers.ToString();
            shareBtn.gameObject.SetActive(true);
            tryAgainBtn.gameObject.SetActive(false);
            
            newBadgesEarned = (noOfBadgesWon - alreadyEarned);
            if (newBadgesEarned < 0)
            {
                newBadgesEarned = 0;
                badgesWonText.text = newBadgesEarned.ToString();
                badgeText.key = "ID_BadgesText";
                
            }
            else
            {
                if (newBadgesEarned == 0)
                {
                    badgesWonText.text = newBadgesEarned.ToString();
                    badgeText.key = "ID_BadgesText";
                }
                else
                {
                    badgesWonText.text = newBadgesEarned.ToString();
                    badgeText.key = newBadgesEarned > 1 ? "ID_BadgesText" : "ID_BadgeText";
                }

                
            }
      
        }
        else
        {
            shareBtn.gameObject.SetActive(false);
            tryAgainBtn.gameObject.SetActive(true);
            congratsLocalizationTextTmp.key = "ID_LReviewFailedText";
            passTextLocalizationTextTmp.key = "ID_FailText";
            correctAnswersText.text = correctAnswers.ToString();
            wrongAnswersText.text = wrongAnswers.ToString();
            switch (MainAppManager.mainAppManager.selectedLevel + 1)
            {
                case 1:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_1;
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level1Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = level1Badges[i].litBadge;
                        }
                    }
                    break;
                case 2:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_2;
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level2Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = level2Badges[i].litBadge;
                        }
                    }
                    break;
                case 3:
                    avatarImage.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex]
                        .characterHalfImage_Level_3;
                    for (int i = 0; i < badgeImages.Length; i++)
                    {
                        if (PlayerPrefs.GetInt($"Level3Badge{i + 1}") == 1)
                        {
                            badgeImages[i].sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[i].litBadge;
                        }
                    }
                    break;
                default:
                    break;
            }

             newBadgesEarned = (noOfBadgesWon - alreadyEarned);
            if (newBadgesEarned < 0)
            {
                newBadgesEarned = 0;
                badgesWonText.text = newBadgesEarned.ToString();
                badgeText.key = "ID_BadgesText";
                
            }
            else
            {
                if (newBadgesEarned == 0)
                {
                    badgesWonText.text = newBadgesEarned.ToString();
                    badgeText.key = "ID_BadgesText";
                }
                else
                {
                    badgesWonText.text = newBadgesEarned.ToString();
                    badgeText.key = newBadgesEarned > 1 ? "ID_BadgesText" : "ID_BadgeText";
                }
            }
            
        }

    }

    public void OnLevelUpScreenStart()
    {
        StartCoroutine(LevelUpEnumeratorMethod());
    }

    IEnumerator LevelUpEnumeratorMethod()
    {
        MainAppManager.mainAppManager.InstantiateLevelUpCharacter();
        _soundManager.PlayAudio("LevelUpAudio");
        fakeBGSprites.transform.DOScale(new Vector3(1.5f, 1.5f), 3f);
        yield return new WaitForSeconds(5f);
        UIManager.SwitchScreens(finishScreen);
    }

    private IEnumerator FinishScreenHandler()
    {
        yield return new WaitForSeconds(delayTime);
       
        if (playerLife > 0)
            switch (MainAppManager.mainAppManager.selectedLevel + 1)
            {
                case 1:
                {
                    if (PlayerPrefs.GetInt("Level1Complete") == 1)
                        UIManager.SwitchScreens(finishScreen);
                    else
                        UIManager.SwitchScreens(levelUpScreen);
                    break;
                }
                case 2:
                {
                    if (PlayerPrefs.GetInt("Level2Complete") == 1)
                        UIManager.SwitchScreens(finishScreen);
                    else
                        UIManager.SwitchScreens(levelUpScreen);
                    break;
                }
                case 3:
                {
                    if (PlayerPrefs.GetInt("Level3Complete") == 1)
                        UIManager.SwitchScreens(finishScreen);
                    else
                        UIManager.SwitchScreens(levelUpScreen);
                    break;
                }
            }
        else
            UIManager.SwitchScreens(finishScreen);
        
        LevelSummaryHandler();
    }
    
  
    public void DeleteAllSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
    
    //Badge Set up for Level Start Menu too lazy to rewrite it
    public void SetupBadgesSummaryPage(GameObject badgeHolder)
    {
        switch (MainAppManager.mainAppManager.selectedLevel+1)
        {
            case 1:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level1Badge{i + 1}") == 1)
                    {
                        child.gameObject.SetActive(true);
                        child.sprite = level1Badges[i].litBadge;
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }

                    
                }
                break;
            } 
            case 2:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level2Badge{i+1}") == 1)
                    {
                        child.gameObject.SetActive(true);
                        child.sprite = level2Badges[i].litBadge;
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }

                    
                }
                break;
            } 
            case 3:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level3Badge{i + 1}") == 1)
                    {
                        child.gameObject.SetActive(true);
                        child.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[i].litBadge;
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }

                    
                }
                break;
            }
            default:
                break;

        }
    }

    public void SetUpBadgesStartLevelPage(GameObject badgeHolder)
    {
         switch (MainAppManager.mainAppManager.selectedLevel+1)
        {
            case 1:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level1Badge{i + 1}") == 1)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        
                        child.gameObject.SetActive(true);
                        child.sprite = level1Badges[i].litBadge;
                    }

                    
                }
                break;
            } 
            case 2:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level2Badge{i+1}") == 1)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        
                        child.gameObject.SetActive(true);
                        child.sprite = level2Badges[i].litBadge;
                    }

                    
                }
                break;
            } 
            case 3:
            {
                for (int i = 0; i < badgeHolder.transform.childCount; i++)
                {
                    var child = badgeHolder.transform.GetChild(i).GetComponent<Image>();
                    if (PlayerPrefs.GetInt($"Level3Badge{i + 1}") == 1)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                        child.sprite = MainAppManager.mainAppManager.characters[selectedHeroIndex].finalCharacterBadges[i].litBadge;
                        
                    }

                    
                }
                break;
            }
            default:
                break;

        }
    }



    public void ResetData()
    {
        noOfBadgesWon = 0;
        noOfBadgesPlayedThisLevel = 0;
        playerLife = 3;
        correctAnswers = 0;
        wrongAnswers = 0;
        totalTimeTaken = 0;
        progressPoint = 0;
        _numberOfQuestionsAnswered = 1;
        _unansweredQuestions = null;
    }
}

