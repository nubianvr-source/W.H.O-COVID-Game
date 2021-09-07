using UnityEngine;

//Customizable class for questions
[System.Serializable]
public class Questions
{
    public Sprite imageQuestion;
    [TextArea]public string textQuestion;
    public bool isClickTrue;
    public string correctIntervention;
    public string wrongIntervention;
    public string TrueAnswerText;
    public string falseAnswerText;

}
