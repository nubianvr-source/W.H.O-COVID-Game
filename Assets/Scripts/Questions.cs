using System;
using LocalizationScripts;
using UnityEngine;

//Customizable class for questions
[System.Serializable]

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/New Question")]
public class Questions: ScriptableObject
{
    
    public Sprite imageQuestion;
    public string questionKey;
    public bool isClickTrue;
    public string trueAnswerKey;
    public string falseAnswerKey;
    public string correctInterventionKey;
    public string wrongInterventionKey;
    public bool isBadgeWorthy;
    public Sprite LitbadgeImage;
    
    [HideInInspector]
    public string textQuestion;
    [HideInInspector]
    public string correctIntervention;
    [HideInInspector]
    public string wrongIntervention;
    [HideInInspector]
    public string TrueAnswerText;
    [HideInInspector]
    public string falseAnswerText;

    public void ChangeLanguage(int index)
    {
        textQuestion = CSVParser.GetTextFromId(questionKey, index);
        TrueAnswerText = CSVParser.GetTextFromId(trueAnswerKey, index);
        falseAnswerText = CSVParser.GetTextFromId(falseAnswerKey, index);
        correctIntervention = CSVParser.GetTextFromId(correctInterventionKey, index);
        wrongIntervention = CSVParser.GetTextFromId(wrongInterventionKey, index);
    }
}
