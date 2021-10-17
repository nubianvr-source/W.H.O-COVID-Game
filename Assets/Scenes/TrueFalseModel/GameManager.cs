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
    private Questions[] questions;
    private static List<Questions> _unansweredQuestions;
    private Questions _currentQuestion;

    [Header("TMP Values")]
    [SerializeField] private TMP_Text questionTmpText;
    [SerializeField] private TMP_Text numberOfQuestionsAnsweredText;
    [SerializeField] private TMP_Text trueAnswerTmpText;
    [SerializeField] private TMP_Text falseAnswerTmpText;
    [SerializeField] private TMP_Text countdownValueTmpText;
    /*[SerializeField] private TMP_Text interventionText;
    [SerializeField] private TMP_Text playerPointsText;
    [SerializeField] private TMP_Text finalPointsText;
    [SerializeField] private TMP_Text interventionTitle;
    [SerializeField] private TMP_Text questionPoints;*/

    [Header("Game play Values")]
    private static float _numberOfQuestionsAnswered = 1;
    public static float _numberOfQuestionsToAsk;
    [SerializeField] private static float correctAnswers;
    [SerializeField] private static float wrongAnswers;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField]private float countdownValue = 6.0f;
    private float countdownBaseValue;
    private static float totalTimeTaken = 0.0f;
    //[SerializeField] private static int playerPoints;

    [Header("UI System")]
    [SerializeField] private Animator baseAnimator;
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private UI_System UIManager;
    [SerializeField] private UI_Screen finishScreen;
    
    //[SerializeField] private UI_Screen interventionScreen;

    [Header("Buttons")]
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    [Header("Images")]
    [SerializeField] private Image questionImage;
    [SerializeField] private Image progressBarMask;
    [SerializeField] private Image countdownRadialBarMask;
    //[SerializeField] private Image imagePrompt;

    [Header("Sprites")]
    [SerializeField] private Sprite btnCorrectSprite;
    [SerializeField] private Sprite btnIncorrectSprite;
    [SerializeField] private Sprite btnNeutralSprite;
    //[SerializeField] private Sprite correctPrompt;
    //[SerializeField] private Sprite incorrectPrompt;

    [Header("Progress Bar")]
    [SerializeField] private Sprite checkpointLitSprite;
    [SerializeField] private Sprite firstBadgeSprite;
    private static bool acquiredFirstBadge = false;
    [SerializeField] private Sprite secondBadgeSprite;
    private static bool acquiredSecondBadge = false;
    [SerializeField] private Sprite thirdBadgeSprite;
    private static bool acquiredThirdBadge = false;
    [SerializeField]private GameObject[] checkpoints;
    [SerializeField] private GameObject[] badges;
    private float progressPoint;

    [Header("Congratulatory Screen")]
    [SerializeField] private TMP_Text congratulatoryText;
    [SerializeField] private Text correctAnswersText;
    [SerializeField] private Text wrongAnswersText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text badgesWonText;
    [SerializeField] private Image firstBadgeImage;
    [SerializeField] private Image secondBadgeImage;
    [SerializeField] private Image thirdBadgeImage;

    private void Start()
    {
        countdownBaseValue = countdownValue;
        if (countdownRadialBarMask)
            countdownRadialBarMask.fillAmount = countdownValue / countdownBaseValue;
        if (numberOfQuestionsAnsweredText)
            numberOfQuestionsAnsweredText.text = "Question " + _numberOfQuestionsAnswered + " of " + _numberOfQuestionsToAsk;

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
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            LoadQuestions();
        }
        SetCurrentQuestion();
        ProgressBarHandler();
       
        EnableButtons(true);
        InvokeRepeating("StartCountdown", 1.0f, 1.0f);

    }

    //Randomly picks a question from the _unansweredQuestion array and sets its the current question to ask.
    private void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count);
        _currentQuestion = _unansweredQuestions[randomQuestionIndex];

        if(questionTmpText)
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
       
    }

    //To be possible removed and every instance stated would be replase with TransitionTo NextQuestion...
    public void LoadNextQuestion()
    {
        //StartCoroutine(TransitionToNextQuestion());
        TransitionToNextQuestion();
    }

    //Run a coroutine to wait for specified delay before switching screen to intervention screen.
   /* IEnumerator TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;

        _unansweredQuestions.Remove(_currentQuestion);
        
        yield return new WaitForSeconds(delay);

        PresentQuestion(2.0f);
    }*/

    private void TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;

        _unansweredQuestions.Remove(_currentQuestion);

        PresentQuestion(delayTime);
    }

    private void LoadQuestions()
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
        if (_currentQuestion.isClickTrue)
        {
            //correct...
            EndCountdown();
            correctAnswers += 1;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            ProgressBarHandler();
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueCorrect");
            if (trueButton)
                trueButton.image.sprite = btnCorrectSprite;






            /*if (interventionText)
               interventionText.text = _currentQuestion.correctIntervention;
            if (questionPoints)
                questionPoints.text = "CORRECT\n+10 POINTS";
            if (interventionTitle)
                interventionTitle.text = "You Did the Right Thing!";
                if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
        }
        else
        {
            //false
            EndCountdown();
            if (buttonAnimator)
                buttonAnimator.SetTrigger("TrueWrong");
            if (trueButton)
                trueButton.image.sprite = btnIncorrectSprite;
            wrongAnswers += 1;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);




           
            /*if (interventionText)
               interventionText.text = _currentQuestion.wrongIntervention;
            if (questionPoints)
                questionPoints.text = "WRONG\n-10 POINTS";
            if (interventionTitle)
                interventionTitle.text = "Risk Alert";
            if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
        }
    }


    //Player selects the false button
    public void UserSelectsFalse()
    {
        if (!_currentQuestion.isClickTrue)
        {
            //correct
            EndCountdown();
            correctAnswers += 1;
            Debug.Log("You have this number of correct answers : " + correctAnswers);
            ProgressBarHandler();
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseCorrect");
            if (falseButton)
                falseButton.image.sprite = btnCorrectSprite;
            


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
        }
        else
        {
            //false
            EndCountdown();
            if (buttonAnimator)
                buttonAnimator.SetTrigger("FalseWrong");
            if (falseButton)
                falseButton.image.sprite = btnIncorrectSprite;
            wrongAnswers += 1;
            Debug.Log("You have this number of wrong answers : " + wrongAnswers);

            /*if (interventionText)
                interventionText.text = _currentQuestion.wrongIntervention;
            if (playerPointsText)
                playerPointsText.gameObject.SetActive(false);
            if (interventionTitle)
                interventionTitle.text = "Risk Alert";
            if (imagePrompt)
                imagePrompt.sprite = correctPrompt;*/
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

    public void PresentQuestion(float time)
    {
        if (_numberOfQuestionsAnswered > _numberOfQuestionsToAsk)
        {
            StartCoroutine(FinishScreenHandler());
        }
        else
        {
            StartCoroutine(ReloadSceneForNextQuestion(time));
        }
    }

    IEnumerator ReloadSceneForNextQuestion(float waitTimeToLoadNextQuestion)
    { 
        yield return new WaitForSeconds(waitTimeToLoadNextQuestion);
        baseAnimator.SetTrigger("hide");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Finish()
    {
        SceneManager.LoadScene("OnlineSafetyStoryGame");
    }

    private void StartCountdown()
    {
        if(countdownValue > 0)
        {
            countdownValue--;
            countdownValueTmpText.text = countdownValue.ToString();
            if (countdownRadialBarMask)
                countdownRadialBarMask.fillAmount = countdownValue / countdownBaseValue;
        }
        else
        {
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
        progressPoint = correctAnswers / _numberOfQuestionsToAsk;
        progressBarMask.fillAmount = progressPoint;
        if (progressPoint >= 0.1f && checkpointLitSprite && checkpoints.Length >= 0)
        {
            checkpoints[0].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.2f && checkpointLitSprite && checkpoints.Length >= 1)
        {
            checkpoints[1].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.3f && firstBadgeSprite && badges.Length >= 0)
        {
            badges[0].GetComponent<Image>().sprite = firstBadgeSprite;
            acquiredFirstBadge = true;
        }
        if (progressPoint >= 0.4f && checkpointLitSprite && checkpoints.Length >= 2)
        {
            checkpoints[2].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.5f && checkpointLitSprite && checkpoints.Length >= 3)
        {
            checkpoints[3].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.6f && secondBadgeSprite && badges.Length >= 1)
        {
            badges[1].GetComponent<Image>().sprite = secondBadgeSprite;
            acquiredSecondBadge = true;
        }
        if (progressPoint >= 0.7f && checkpointLitSprite && checkpoints.Length >= 4)
        {
            checkpoints[4].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.8f && checkpointLitSprite && checkpoints.Length >= 5)
        {
            checkpoints[5].GetComponent<Image>().sprite = checkpointLitSprite;
        }
        if (progressPoint >= 0.9f && secondBadgeSprite && badges.Length >= 2)
        {
            badges[2].GetComponent<Image>().sprite = thirdBadgeSprite;
            acquiredThirdBadge = true;
        }
    }

    private void CongratulatoryScreenHandler()
    {
        correctAnswersText.text = correctAnswers.ToString();
        wrongAnswersText.text = wrongAnswers.ToString();
        totalTimeText.text = totalTimeTaken.ToString();
        if (acquiredFirstBadge)
        {
            congratulatoryText.text = "Nice!";
            firstBadgeImage.gameObject.SetActive(true);
            secondBadgeImage.gameObject.SetActive(false);
            thirdBadgeImage.gameObject.SetActive(false);
            badgesWonText.text = "You won "+ 1.ToString() + " badge";
        }
            
        if (acquiredSecondBadge)
        {
            congratulatoryText.text = "Good Job!";
            firstBadgeImage.gameObject.SetActive(true);
            secondBadgeImage.gameObject.SetActive(true);
            thirdBadgeImage.gameObject.SetActive(false);
            badgesWonText.text = "You won " + 2.ToString() + " badges";
        }
            
        if (acquiredThirdBadge)
        {
            congratulatoryText.text = "Excellent!";
            firstBadgeImage.gameObject.SetActive(true);
            secondBadgeImage.gameObject.SetActive(true);
            thirdBadgeImage.gameObject.SetActive(true);
            badgesWonText.text = "You won " + 3.ToString() + " badges";
        }

        if(!acquiredFirstBadge && !acquiredSecondBadge && !acquiredThirdBadge)
        {
            congratulatoryText.text = "Oops! Try Again...";
            firstBadgeImage.gameObject.SetActive(false);
            secondBadgeImage.gameObject.SetActive(false);
            thirdBadgeImage.gameObject.SetActive(false);
            badgesWonText.text = "You won no badge";
        }
    }

    private IEnumerator FinishScreenHandler()
    {
        yield return new WaitForSeconds(delayTime);
        UIManager.SwitchScreens(finishScreen);
        CongratulatoryScreenHandler();
    }
}
