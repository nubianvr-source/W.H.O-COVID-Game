using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
  private PlayerData playerData;

  private void Awake()
  {
    CreatePlayerData();
  }

  private void Start()
  {
    var count = PlayerPrefs.GetString("unity.player_session_count");
    print("Game Launched: " + count + " times");
    var launchCount = int.Parse(count);
   
    if (launchCount == 1)
    {
      CreatePlayerData();
      playerData.firstTimeRunningApp = launchCount;
      print(playerData.ToString());
    }
    else
    {
      playerData.firstTimeRunningApp = launchCount;
      print(playerData.ToString());
    }
  }

  public void CreatePlayerData()
  {
    playerData = new PlayerData(1,1,"en",0);
  }

  public void SavePlayerData()
  {
    PlayerPrefs.SetInt("PlayMusic",playerData.playMusic);
    PlayerPrefs.SetInt("PlaySFX", playerData.playSFX);
    PlayerPrefs.SetString("Lang", playerData.playerLanguagePref);
    PlayerPrefs.SetInt("First_Time_Running_App", playerData.firstTimeRunningApp);
  }

  public void LoadPlayerData()
  {
    PlayerPrefs.GetInt("PlayMusic");
    PlayerPrefs.GetInt("PlaySFX");
    PlayerPrefs.GetString("Lang");
    PlayerPrefs.GetInt("First_Time_Running_App");

    print(playerData.ToString());
  }
}
