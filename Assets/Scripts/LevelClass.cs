using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelClass
{
    public Sprite levelIcon;
    public string levelNumberKey;
    public string levelTitleKey;
    public string levelQuestDescKey;
    public int sceneToLoad;
    public bool levelComplete;
    
    [HideInInspector]
    public string levelNumber;
    [HideInInspector]
    public string levelTitle;
    [HideInInspector]
    public string levelQuestDesc;

    public void ChangeLanguage(int index)
    {
        levelNumber = CSVParser.GetTextFromId(levelNumberKey, index);
        levelTitle = CSVParser.GetTextFromId(levelTitleKey, index);
        levelQuestDesc = CSVParser.GetTextFromId(levelQuestDescKey, index);
    }

}
