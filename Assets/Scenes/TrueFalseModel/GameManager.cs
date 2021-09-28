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

    [SerializeField] private Text questionText;
    [SerializeField] private Text trueAnswerText;
    [SerializeField] private Text falseAnswerText;
    [SerializeField] private Text playerPointsText;
    [SerializeField] private Text numberOfQuestionsAnsweredText;

    
    [SerializeField] private TMP_Text interventionText;
    [SerializeField] private TMP_Text finalPointsText;
    [SerializeField] private TMP_Text interventionTitle;
    [SerializeField] private TMP_Text questionPoints;

    private static int _numberOfQuestionsAnswered = 1;
    public int _numberOfQuestionsToAsk = 5;
    [SerializeField] private static int playerPoints;
    [SerializeField] private float delay = 0.5f;

    [SerializeField] private Animator animator;

    [SerializeField] private UI_System UIManager;

    [SerializeField] private UI_Screen interventionScreen;
    [SerializeField] private UI_Screen finishScreen;

    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    [SerializeField] private Image imagePrompt;
    [SerializeField] private Image questionImage;

    [SerializeField] private Sprite correctPrompt;
    [SerializeField] private Sprite incorrectPrompt;
    



    private void Start()
    {
        //Initiate questions from the questions array in to the unanswered question list, to know which questions have been answered and which are yet to be answered.
        //For a retry scenario just set the unanswered question list to null and the count to 0 to load all the questions again.
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            LoadQuestions();
        }


        SetCurrentQuestion();


        Screen.fullScreen = true;


        numberOfQuestionsAnsweredText.text = _numberOfQuestionsAnswered + " of 5";


        EnableButtons();

        //Change Screen Orientation to potrait on start.
        Screen.orientation = ScreenOrientation.Portrait;

        playerPointsText.gameObject.SetActive(true);
    }

    //Randomly picks a question from the _unansweredQuestion array and sets its the current question to ask.
    private void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count);
        _currentQuestion = _unansweredQuestions[randomQuestionIndex];

        questionText.text = _currentQuestion.textQuestion;
        questionImage.sprite = _currentQuestion.imageQuestion;
        trueAnswerText.text = _currentQuestion.TrueAnswerText;
        falseAnswerText.text = _currentQuestion.falseAnswerText;

    }


    //Update Loop just updates the players score visually during gameplay.
    private void Update()
    {
        playerPointsText.text = playerPoints.ToString();
    }

    public void LoadNextQuestion()
    {
         StartCoroutine(TransitionToNextQuestion());
    }


    //Run a coroutine to wait for specified delay before switching screen to intervention screen.
    IEnumerator TransitionToNextQuestion()
    {
        _numberOfQuestionsAnswered++;

        _unansweredQuestions.Remove(_currentQuestion);
        
        yield return new WaitForSeconds(delay);

        UIManager.SwitchScreens(interventionScreen);
    }

    private void LoadQuestions()
    {
        Object[] questionObject = Resources.LoadAll("Questions", typeof(Questions));
        questions = new Questions[questionObject.Length];
        for (int i = 0; i < questionObject.Length; i++)
        {
            questions[i] = (Questions) questionObject[i];
            questions[i].ChangeLanguage(0);
        }

        _unansweredQuestions = questions.ToList();

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
            //correct
            interventionText.text = _currentQuestion.correctIntervention;
            animator.SetTrigger("TrueCorrect");
            questionPoints.text = "CORRECT\n+10 POINTS";
            interventionTitle.text = "You Did the Right Thing!";
            imagePrompt.sprite = correctPrompt;
            playerPointsText.gameObject.SetActive(false);
            playerPoints += 10;
        }
        else
        {
            //false
            interventionText.text = _currentQuestion.wrongIntervention;
            animator.SetTrigger("TrueWrong");
            questionPoints.text = "WRONG\n-10 POINTS";
            interventionTitle.text = "Risk Alert";
            imagePrompt.sprite = incorrectPrompt;
            playerPointsText.gameObject.SetActive(false);
            playerPoints -= 10;

        }
        

    }


    //Player selects the false button
    public void UserSelectsFalse()
    {

        if (!_currentQuestion.isClickTrue)
        {
            //correct
            interventionText.text = _currentQuestion.correctIntervention;
            animator.SetTrigger("FalseCorrect");
            questionPoints.text = "CORRECT\n+10 POINTS";
            interventionTitle.text = "You Did the Right Thing!";
            imagePrompt.sprite = correctPrompt;
            playerPointsText.gameObject.SetActive(false);
            playerPoints += 10;

        }
        else
        {
            //false
            interventionText.text = _currentQuestion.wrongIntervention;
            animator.SetTrigger("FalseWrong");
            questionPoints.text = "WRONG\n-10 POINTS";
            interventionTitle.text = "Risk Alert";
            imagePrompt.sprite = correctPrompt;
            playerPointsText.gameObject.SetActive(false);
            playerPoints -= 10;
        }

    }


    private void EnableButtons() {
        trueButton.interactable = true;
        falseButton.interactable = true;
    }

   

    public void PresentQuestion(float time)
    {
        if (_numberOfQuestionsAnswered > _numberOfQuestionsToAsk)
        {
            if (playerPoints < 0)
            {
                finalPointsText.text = "Your final score is\n" + playerPoints + " Points.\nIt's seems we might have to take this lesson all over again";
            }

            else if (playerPoints > 0 && playerPoints <= 20)
            {
                finalPointsText.text = "Your final score is\n" + playerPoints + " Points.\nWell at least you didn't get a zero, but a lot more can be done to improve. Maybe try taking this lesson again?";
            }
            else if (playerPoints > 20 && playerPoints <= 40)
            {
                finalPointsText.text = "Your final score is\n" + playerPoints + " Points.\nNicely done, just a few rough edges here and there, nothing a little revision can't fix";
            }
            else
            {
                finalPointsText.text = "Your final score is\n" + playerPoints + " Points.\nA perfect score, well done";
            }

            UIManager.SwitchScreens(finishScreen);
            _numberOfQuestionsAnswered = 0;

        }
        else
        {
            StartCoroutine(ReloadSceneForNextQuestion(time));
            
        }
       

    }


    //A question is set up by reloading the scene after which a question is picked from the variable unanswered questions list.
    IEnumerator ReloadSceneForNextQuestion(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
 

    public void Finish()
    {
        playerPoints = 0;
        SceneManager.LoadScene("OnlineSafetyStoryGame");
    }

   
    ////Initiate the Unity React Native bridge  
    //void Awake()
    //{
    //    UnityMessageManager.Instance.OnMessage += RecieveMessage;
    //}

    ////Destroy the Unity React Native bridge
    //void onDestroy()
    //{
    //    UnityMessageManager.Instance.OnMessage -= RecieveMessage;
    //}

    ////Handle messages received from React Native through the bridge here.
    //void RecieveMessage(string message)
    //{
        
    //}


}
