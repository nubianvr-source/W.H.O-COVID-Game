using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerData
{
    public int playMusic;
    public int playSFX;
    public string playerLanguagePref;
    public int firstTimeRunningApp;



    public PlayerData(int playMusic, int playSFX, string playerLanguagePref, int firstTimeRunningApp)
    {
        this.playMusic = playMusic;
        this.playSFX = playSFX;
        this.playerLanguagePref = playerLanguagePref;
        this.firstTimeRunningApp = firstTimeRunningApp;
    }

    public override string ToString()
    {
        return $"Play Music is {playMusic}, PlaySFX is {playSFX}, PlayerLanguagePref is {playerLanguagePref}, First Time Running App is {firstTimeRunningApp}";
    }
}