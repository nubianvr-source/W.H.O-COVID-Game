using System;
using System.Runtime.InteropServices;
using LocalizationScripts;
using UnityEngine;
using UnityEngine.Serialization;

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
    public bool isBadgeWorthy;
    public Badges questionBadge;
    
    [HideInInspector]
    public string textQuestion;
    [FormerlySerializedAs("TrueAnswerText")] [HideInInspector]
    public string trueAnswerText;
    [HideInInspector]
    public string falseAnswerText;

    public void ChangeLanguage(int index)
    {
        textQuestion = CSVParser.GetTextFromId(questionKey, index);
        trueAnswerText = CSVParser.GetTextFromId(trueAnswerKey, index);
        falseAnswerText = CSVParser.GetTextFromId(falseAnswerKey, index);
    }
}
