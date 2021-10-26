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
    private static float playerLife = 3.0f;
    private static float maxPlayerLife = 3.0f;


    [Header("TMP Values")]
    [SerializeField] private TMP_Text questionTmpText;
    [SerializeField] private TMP_Text numberOfQuestionsAnsweredText;
    [SerializeField] private TMP_Text trueAnswerTmpText;
    [SerializeField] private TMP_Text falseAnswerTmpText;
    [SerializeField] private TMP_Text countdownValueTmpText;
    

    [Header("Game play Values")]
    private static float _numberOfQuestionsAnswered = 1.0f;
    public static float _numberOfQuestionsToAsk;
    [SerializeField] private static float correctAnswers;
    [SerializeField] private static float wrongAnswers;
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
    [SerializeField]private GameObject[] checkpoints;
    private static float progressPoint = 0.0f;
    
    [Header("Badges")]
    [SerializeField] private GameObject[] badges;
    [SerializeField] private Sprite firstBadgeSprite;
    private static bool acquiredFirstBadge = false;
    [SerializeField] private Sprite secondBadgeSprite;
    private static bool acquiredSecondBadge = false;
    [SerializeField] private Sprite thirdBadgeSprite;
    private static bool acquiredThirdBadge = false;
    private static int noOfBadgesWon;

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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip timeTickAudioClip;
    [SerializeField] private AudioClip timeUpAudioClip;
    [SerializeField] private AudioClip correctClickAudioClip;
    [SerializeField] private AudioClip falseClickAudioClip;
    #endregion

    #region("Unused Declarations")
    /*private static int questionIndex = 0;
    [SerializeField] private TMP_Text interventionText;
    [SerializeField] private TMP_Text playerPointsText;
    [SerializeField] private TMP_Text finalPointsText;
    [SerializeField] private TMP_Text interventionTitle;
    [SerializeField] private TMP_Text questionPoints;
    [SerializeField] private static int playerPoints;
    [SerializeField] private UI_Screen interventionScreen;*/
    #endregion

    private void Start()
    {
        countdownBaseValue = countdownValue;
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            LoadQuestions();
        }
        SetCurrentQuestion();

        ProgressBarHandler();

        if (countdownRadialBarMask)
            countdownRadialBarMask.fillAmount = countdownValue / countdownBaseValue;
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

        #region
        //Mavreon made changes here...
        //int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count);
        //questionIndex++;
        #endregion
    }


    //Update Loop just updates the players score visually during gameplay.
    private void Update()
    {
       
    }

    //To be possible removed and every instance stated would be replase with TransitionTo NextQuestion...
    public void LoadNextQuestion()
    {
        //Executed based on the health value...
        if (playerLife > 0.0f)
            TransitionToNextQuestion();
        else
            StartCoroutine(FinishScreenHandler());
    }

   
    private void TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;

        _unansweredQuestions.Remove(_currentQuestion);

        PresentQuestion(delayTime);
    }

    #region
    /*private void LoadQuestions()
    {
        Object[] questionObject = Resources.LoadAll("Level1Questions", typeof(Questions));
        questions = new Questions[questionObject.Length];
        for (int i = 0; i < questionObject.Length; i++)
        {
            questions[i] = (Questions) questionObject[i];
            questions[i].ChangeLanguage(0);
        }
        _unansweredQuestions = questions.ToList();
        _numberOfQuestionsToAsk = questionObject.Length;
        Debug.Log("Number of questions to ask is : " + _numberOfQuestionsToAsk);
    }*/
    #endregion

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
            Camera.main.GetComponent<AudioSource>().PlayOneShot(correctClickAudioClip);
            correctAnswers += 1;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueCorrect");
            if (trueButton)
                trueButton.image.sprite = btnCorrectSprite;
            BadgeHandler();

            #region
            /*if (interventionText)
               interventionText.text = _currentQuestion.correctIntervention;
            if (questionPoints)
                questionPoints.text = "CORRECT\n+10 POINTS";
            if (interventionTitle)
                interventionTitle.text = "You Did the Right Thing!";
                if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
            #endregion
        }
        else
        {
            //false
            playerLife--;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(falseClickAudioClip);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueWrong");
            if (trueButton)
                trueButton.image.sprite = btnIncorrectSprite;
            wrongAnswers ++;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);

            #region
            /*if (interventionText)
               interventionText.text = _currentQuestion.wrongIntervention;
            if (questionPoints)
                questionPoints.text = "WRONG\n-10 POINTS";
            if (interventionTitle)
                interventionTitle.text = "Risk Alert";
            if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
            #endregion
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
            Camera.main.GetComponent<AudioSource>().PlayOneShot(correctClickAudioClip);
            correctAnswers ++;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseCorrect");
            if (falseButton)
                falseButton.image.sprite = btnCorrectSprite;
            BadgeHandler();

            #region
            /*if (interventionText)
                interventionText.text = _currentQuestion.correctIntervention;
            if (questionPoints)
                questionPoints.text = "CORRECT\n+10 POINTS";
            if (playerPointsText)
                playerPointsText.gameObject.SetActive(false);
            if (interventionTitle)
                interventionTitle.text = "You Did the Right Thing!";
            if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
            #endregion
        }
        else
        {
            //false
            playerLife--;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(falseClickAudioClip);
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseWrong");
            if (falseButton)
                falseButton.image.sprite = btnIncorrectSprite;
            wrongAnswers++;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);

            #region
            /*if (interventionText)
                interventionText.text = _currentQuestion.wrongIntervention;
            if (playerPointsText)
                playerPointsText.gameObject.SetActive(false);
            if (interventionTitle)
                interventionTitle.text = "Risk Alert";
            if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
            #endregion
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
                Camera.main.GetComponent<AudioSource>().PlayOneShot(timeTickAudioClip);
            countdownValue--;
            countdownValueTmpText.text = countdownValue.ToString();
            if (countdownRadialBarMask)
                countdownRadialBarMask.fillAmount = countdownValue / countdownBaseValue;
        }
        else
        {
            if (timeUpAudioClip)
                Camera.main.GetComponent<AudioSource>().PlayOneShot(timeUpAudioClip);
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
        progressPoint = (correctAnswers + wrongAnswers) / _numberOfQuestionsToAsk;
        progressBarMask.fillAmount = progressPoint;

        if (progressPoint >= 0.1f)
        {
            checkpoints[0].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        if (progressPoint >= 0.2f)
        {
            checkpoints[1].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        if (progressPoint >= 0.3f)
        {
            checkpoints[2].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        //Badge Related...
        if (progressPoint >= 0.4f && acquiredFirstBadge)
        {
            badges[0].GetComponent<Image>().sprite = firstBadgeSprite;
        }
        if (progressPoint >= 0.5f)
        {
            checkpoints[3].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        if (progressPoint >= 0.6f)
        {
            checkpoints[4].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        //Badge Related...
        if (progressPoint >= 0.7f && acquiredSecondBadge)
        {
            badges[1].GetComponent<Image>().sprite = secondBadgeSprite;
        }
        if (progressPoint >= 0.8f)
        {
            checkpoints[5].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        if (progressPoint >= 0.9f)
        {
            checkpoints[6].GetComponent<Image>().sprite = CheckpointLitSprite;
        }
        //Badge Related...
        if (progressPoint >= 1.0f && acquiredThirdBadge)
        {
            badges[2].GetComponent<Image>().sprite = thirdBadgeSprite;
        }

    }

    private void BadgeHandler()
    {
        if(_currentQuestion == questions[3]  && _currentQuestion.isBadgeWorthy )
        {
            badges[0].GetComponent<Image>().sprite = _currentQuestion.LitbadgeImage;
            acquiredFirstBadge = true;
            noOfBadgesWon++;
        }
        if (_currentQuestion == questions[6] && _currentQuestion.isBadgeWorthy)
        {
            badges[1].GetComponent<Image>().sprite = _currentQuestion.LitbadgeImage;
            acquiredSecondBadge = true;
            noOfBadgesWon++;
        }
        if (_currentQuestion == questions[9] && _currentQuestion.isBadgeWorthy)
        {
            badges[2].GetComponent<Image>().sprite = _currentQuestion.LitbadgeImage;
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
        CongratulatoryScreenHandler();
    }
}
