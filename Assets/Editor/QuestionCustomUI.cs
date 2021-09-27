using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Questions))]
public class QuestionCustomUI : Editor
{
   //Will work on Custom GUI later
   public override void OnInspectorGUI()
   {
      
      base.OnInspectorGUI();
      //Lets all the Variables from Questions
      //Questions questions = (Questions) target;
      
      //Now we can start setting up the custom GUI
      /*GUILayout.BeginHorizontal();
      GUILayout.Label("Question");
      GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
      {
         fontSize = 15,
         fixedHeight = 200,
         fixedWidth = 800,
         alignment = TextAnchor.UpperLeft
      };
      questions.textQuestion = EditorGUILayout.TextArea("Question", textAreaStyle);
      GUILayout.EndHorizontal();
      */
      
   }
}
