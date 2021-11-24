using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
  private PlayerData playerData;

  private void Awake()
  {
    
  }

  private void Start()
  {
    var count = PlayerPrefs.GetString("unity.player_session_count");
    print("Game Launched: " + count + " times");
    var launchCount = int.Parse(count);
   
    if (launchCount == 1)
    {
      CreatePlayerData();
      SavePlayerData();
   
    }
    else
    {
      LoadPlayerData();
    }
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
