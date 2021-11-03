using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using LocalizationScripts;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using NubianVR.UI;
using TMPro;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    #region Declarations
    [SerializeField]private Questions[] questions;
    private static List<Questions> _unansweredQuestions;
    private Questions _currentQuestion;

    //Player Life Stats
    ////This can be an int variable type since there are no fractions involved. 
    private static int playerLife = 3;
    //private static float maxPlayerLife = 3.0f;
    


    [Header("TMP Values")]
    [SerializeField] private TMP_Text questionTmpText;
    [SerializeField] private TMP_Text numberOfQuestionsAnsweredText;
    [SerializeField] private TMP_Text trueAnswerTmpText;
    [SerializeField] private TMP_Text falseAnswerTmpText;
    [SerializeField] private TMP_Text countdownValueTmpText;
    

    [Header("Game play Values")]
    private static int _numberOfQuestionsAnswered = 1;
    public static int _numberOfQuestionsToAsk;
    [SerializeField] private static int correctAnswers;
    [SerializeField] private static int wrongAnswers;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField]private float countdownValue = 6.0f;
    private float countdownBaseValue;
    private static float totalTimeTaken = 0.0f;
   

    [Header("UI System")]
    [SerializeField] private Animator baseAnimator;
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private UI_System UIManager;
    [SerializeField] private UI_Screen finishScreen;
    
    

    [Header("Buttons")]
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;
    

    [Header("Images")]
    [SerializeField] private Image questionImage;
    [SerializeField] private Image countdownRadialBarMask;

    [Header("Sprites")]
    [SerializeField] private Sprite btnCorrectSprite;
    [SerializeField] private Sprite btnIncorrectSprite;
    [SerializeField] private Sprite btnNeutralSprite;

    [Header("Progress Bar")]
    [SerializeField] private Image progressBarMask;
    [SerializeField] private Sprite CheckpointLitSprite;
    [SerializeField]private Image[] checkpoints;
    private static float progressPoint = 0.0f;
    
    [Header("Badges")]
    [SerializeField] private Image[] badges;
    [SerializeField] private Sprite firstBadgeSprite;
    private static bool acquiredFirstBadge = false;
    [SerializeField] private Sprite secondBadgeSprite;
    private static bool acquiredSecondBadge = false;
    [SerializeField] private Sprite thirdBadgeSprite;
    private static bool acquiredThirdBadge = false;
    private static int noOfBadgesWon;
    private static int  removeBadgesfromCheckpoints = 1;

    [Header("Congratulatory Screen")]
    [SerializeField] private TMP_Text congratulatoryText;
    [SerializeField] private TMP_Text passText;
    [SerializeField] private Text correctAnswersText;
    [SerializeField] private Text wrongAnswersText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text badgesWonText;
    [SerializeField] private Image firstBadgeImage;
    [SerializeField] private Image secondBadgeImage;
    [SerializeField] private Image thirdBadgeImage;

    private AudioSource _audioPlayer;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip timeTickAudioClip;
    [SerializeField] private AudioClip timeUpAudioClip;
    [SerializeField] private AudioClip correctClickAudioClip;
    [SerializeField] private AudioClip falseClickAudioClip;
    #endregion


    private void Awake()
    {
        //A good practice is to get and store your Get components in a variable because the Get Component call is expensive
        if(Camera.main)
            _audioPlayer = Camera.main.GetComponent<AudioSource>();
    }

    private void Start()
    {
        countdownBaseValue = countdownValue;
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            //With all the static variables you will eventually need a reset function that runs when the game is starts to prevent data crossing from different scenes
            //It can be placed here after the question list check and before the question loads.
            LoadQuestions();
        }
        SetCurrentQuestion();

        ProgressBarHandler();

        if (countdownRadialBarMask)
            countdownRadialBarMask.fillAmount = 1.0f;
        if (numberOfQuestionsAnsweredText)
            numberOfQuestionsAnsweredText.text = "Question " + _numberOfQuestionsAnswered + " of " + _numberOfQuestionsToAsk;
        EnableButtons(true);
        InvokeRepeating("StartCountdown", 1.5f, 1.0f);

        #region
        /*checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Debug.Log(checkpoints[i].name + " with index : " + i);
        }
        badges = GameObject.FindGameObjectsWithTag("Badge");
        for (int i = 0; i < badges.Length; i++)
        {
            Debug.Log(badges[i].name + " with index : " + i);
        }*/
        #endregion
    }

    //Randomly picks a question from the _unansweredQuestion array and sets its the current question to ask.
    private void SetCurrentQuestion()
    {
        _currentQuestion = _unansweredQuestions[0];
        if (questionTmpText)
            questionTmpText.text = _currentQuestion.textQuestion;
        if (questionImage)
            questionImage.sprite = _currentQuestion.imageQuestion;
        if (trueAnswerTmpText)
            trueAnswerTmpText.text = _currentQuestion.TrueAnswerText;
        if (falseAnswerTmpText)
            falseAnswerTmpText.text = _currentQuestion.falseAnswerText;
        
    }


    //Update Loop just updates the players score visually during gameplay.
    private void Update()
    {
        //Helped with the smooth timer animation. Added plus one to the countdownBaseValue to account for the timer ending on zero
        countdownRadialBarMask.fillAmount -= 1.0f / (countdownBaseValue + 1f) * Time.deltaTime;
        
    }

    //To be possible removed and every instance stated would be replase with TransitionTo NextQuestion...
    public void LoadNextQuestion()
    {
        //Executed based on the health value...
        if (playerLife > 0)
            TransitionToNextQuestion();
        else
            StartCoroutine(FinishScreenHandler());
    }

   
    private void TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;
        if( 5<= _numberOfQuestionsAnswered && _numberOfQuestionsAnswered <=7)
        {
            removeBadgesfromCheckpoints = 2;
        }
        else if (7 <= _numberOfQuestionsAnswered && _numberOfQuestionsAnswered <=9 )
        {
            removeBadgesfromCheckpoints = 3;
        }

        _unansweredQuestions.Remove(_currentQuestion);

        PresentQuestion(delayTime);
    }

    private void LoadQuestions()
    {
        _unansweredQuestions = questions.ToList();
        _numberOfQuestionsToAsk = questions.Length;
        Debug.Log("Number of questions to ask is : " + _numberOfQuestionsToAsk);
    }

    private void ChangeLanguage(int index)
    {
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i].ChangeLanguage(index);
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
            _audioPlayer.PlayOneShot(correctClickAudioClip);
            correctAnswers += 1;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueCorrect");
            if (trueButton)
                trueButton.image.sprite = btnCorrectSprite;
            BadgeHandler();
        }
        else
        {
            //false
            playerLife--;
            _audioPlayer.PlayOneShot(falseClickAudioClip);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueWrong");
            if (trueButton)
                trueButton.image.sprite = btnIncorrectSprite;
            wrongAnswers ++;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);
            
        }
        ProgressBarHandler();
    }


    //Player selects the false button
    public void UserSelectsFalse()
    {
        EndCountdown();
        if (!_currentQuestion.isClickTrue)
        {
            //correct
            _audioPlayer.PlayOneShot(correctClickAudioClip);
            correctAnswers ++;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseCorrect");
            if (falseButton)
                falseButton.image.sprite = btnCorrectSprite;
            BadgeHandler();
        }
        else
        {
            //false
            playerLife--;
            _audioPlayer.PlayOneShot(falseClickAudioClip);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseWrong");
            if (falseButton)
                falseButton.image.sprite = btnIncorrectSprite;
            wrongAnswers++;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);
        }
        ProgressBarHandler();
    }

    public void EnableButtons(bool condition) 
    {
        if(trueButton && falseButton)
        {
            trueButton.interactable = condition;
            falseButton.interactable = condition;
        }
    }

    public void PresentQuestion(float time)
    {
        if (_numberOfQuestionsAnswered > _numberOfQuestionsToAsk)
            StartCoroutine(FinishScreenHandler());
        else
            StartCoroutine(ReloadSceneForNextQuestion(time));
    }

    IEnumerator ReloadSceneForNextQuestion(float waitTimeToLoadNextQuestion)
    { 
        yield return new WaitForSeconds(waitTimeToLoadNextQuestion);
        baseAnimator.SetTrigger("hide");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StartCountdown()
    {
        if(countdownValue > 0)
        {
            if (timeTickAudioClip)
                _audioPlayer.PlayOneShot(timeTickAudioClip);
            countdownValue--;
            countdownValueTmpText.text = countdownValue.ToString();
        }
        else
        {
            wrongAnswers++;
            ProgressBarHandler();
            if (timeUpAudioClip)
                _audioPlayer.PlayOneShot(timeUpAudioClip);
            EndCountdown();
            EnableButtons(false);
            trueButton.image.sprite = btnNeutralSprite;
            falseButton.image.sprite = btnNeutralSprite;
            LoadNextQuestion();
        }
    }

    private void EndCountdown()
    {
        CancelInvoke("StartCountdown");
        totalTimeTaken += (countdownBaseValue - countdownValue);
        Debug.Log("Time taken to answer question : " + totalTimeTaken + " secs");
    }

    private void ProgressBarHandler()
    {
        progressPoint = (correctAnswers + wrongAnswers) / (float)_numberOfQuestionsToAsk;
        progressBarMask.fillAmount = progressPoint;
        print("Progress Point: "+progressPoint);
        
        for (int j = 0; j < _numberOfQuestionsAnswered - removeBadgesfromCheckpoints; j++)
        {
            checkpoints[j].sprite = CheckpointLitSprite;
            if (acquiredFirstBadge)
                badges[0].sprite = firstBadgeSprite;

            if (acquiredSecondBadge)
                badges[1].sprite = secondBadgeSprite;

            if (acquiredThirdBadge)
                badges[2].sprite = thirdBadgeSprite;
                    
        }
    }

    private void BadgeHandler()
    {
        if(_currentQuestion == questions[3]  && _currentQuestion.isBadgeWorthy )
        {
            badges[0].sprite = _currentQuestion.LitbadgeImage;
            acquiredFirstBadge = true;
            noOfBadgesWon++;
        }
        if (_currentQuestion == questions[6] && _currentQuestion.isBadgeWorthy)
        {
            badges[1].sprite = _currentQuestion.LitbadgeImage;
            acquiredSecondBadge = true;
            noOfBadgesWon++;
        }
        if (_currentQuestion == questions[9] && _currentQuestion.isBadgeWorthy)
        {
            badges[2].sprite = _currentQuestion.LitbadgeImage;
            acquiredThirdBadge = true;
            noOfBadgesWon++;
        }
    }
    private void CongratulatoryScreenHandler()
    {
        if(correctAnswers < 7)
        {
            congratulatoryText.text = "Oops!";
            passText.text = "You failed to pass this level, please try again!";
        }
        if(correctAnswers >= 7 && correctAnswers <= 8)
            congratulatoryText.text = "Nice!";
        if(correctAnswers == 9 )
            congratulatoryText.text = "Good Job!";
        if (correctAnswers >= 10)
            congratulatoryText.text = "Excellent";
        if (noOfBadgesWon > 1)
            badgesWonText.text = "You earned " + noOfBadgesWon + " badges";
        else
            badgesWonText.text = "You earned " + noOfBadgesWon + " badge";
        if (acquiredFirstBadge)
            firstBadgeImage.sprite = firstBadgeSprite;
        if (acquiredSecondBadge)
            secondBadgeImage.sprite = secondBadgeSprite;
        if (acquiredThirdBadge)
            thirdBadgeImage.sprite = thirdBadgeSprite;

        correctAnswersText.text = correctAnswers.ToString();
        wrongAnswersText.text = wrongAnswers.ToString();
        totalTimeText.text = totalTimeTaken.ToString();
    }

    private IEnumerator FinishScreenHandler()
    {
        yield return new WaitForSeconds(delayTime);
        UIManager.SwitchScreens(finishScreen);
        //UIManager.SwitchScreens1(finishScreen, 0.0f);
        CongratulatoryScreenHandler();
    }
}
