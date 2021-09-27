using UnityEngine;

//Customizable class for questions
[System.Serializable]

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/New Question")]
public class Questions: ScriptableObject
{
    public Sprite imageQuestion;
    [TextArea]public string textQuestion;
    public bool isClickTrue;
    [TextArea]public string correctIntervention;
    [TextArea]public string wrongIntervention;
    [TextArea]public string TrueAnswerText;
    [TextArea]public string falseAnswerText;

}
