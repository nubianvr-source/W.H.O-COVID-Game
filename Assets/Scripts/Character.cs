using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "New Character", menuName = "Character/Add New Character")]
public class Character : ScriptableObject
{
     public string characterName;
     public int characterAge;
     public string characterCountry;
     public Sprite characterCountryImage;
     public string characterDescriptionKey;
     public string characterSuperPowerKey;
     public string characterGoalKey;
     public Sprite characterFullImage_Level_1;
     public Sprite characterFullImage_Level_2;
     public Sprite characterFullImage_Level_3;
     public Sprite characterHalfImage_Level_1;
     public Sprite characterHalfImage_Level_2;
     public Sprite characterHalfImage_Level_3;
     public Sprite characterBustImage_Level_1;
     public Sprite characterBustImage_Level_2;
     public Sprite characterBustImage_Level_3;
     public GameObject animatedCharacterLevel1;
     public GameObject animatedCharacterLevel2;
     public GameObject animatedCharacterLevel3;

     [HideInInspector]
     public string characterDescriptionText;
     [HideInInspector]
     public string characterSuperPowerText;
     [HideInInspector]
     public string characterGoalText;

     public void ChangeLanguage(int index)
     {
          characterDescriptionText = CSVParser.GetTextFromId(characterDescriptionKey, index);
          characterSuperPowerText = CSVParser.GetTextFromId(characterSuperPowerKey, index);
          characterGoalText = CSVParser.GetTextFromId(characterGoalKey, index);
     }
}
