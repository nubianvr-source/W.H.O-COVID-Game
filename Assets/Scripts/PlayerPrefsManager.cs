using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerPrefsManager : MonoBehaviour
{
  private PlayerData playerData;

  private void Awake()
  {
    if (AnalyticsSessionInfo.sessionCount != 1) return;
    CreatePlayerData();
    SavePlayerData();
  }
  
  private void Start()
  {
   
    
  }

  public void CreatePlayerData()
  {
    playerData = new PlayerData(1,1,0, 0);
    //PlayerLanguagePrefs : English = 0, French = 1....
  }

  public void SavePlayerData()
  {
    PlayerPrefs.SetInt("PlayMusic",playerData.playMusic);
    PlayerPrefs.SetInt("PlaySFX", playerData.playSFX);
    PlayerPrefs.SetInt("Lang", playerData.playerLanguagePref);
    PlayerPrefs.SetInt("HeroIndex",playerData.heroIndex);
  }

  public void LoadPlayerData()
  {
    PlayerPrefs.GetInt("PlayMusic");
    PlayerPrefs.GetInt("PlaySFX");
    PlayerPrefs.GetInt("Lang");
    PlayerPrefs.GetInt("HeroIndex");

  }
}
