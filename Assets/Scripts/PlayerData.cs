using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerData
{
    public int playMusic;
    public int playSFX;
    public int playerLanguagePref;
    public int firstTimeRunningApp;
    public int heroIndex;



    public PlayerData(int playMusic, int playSFX, int playerLanguagePref, int heroIndex)
    {
        this.playMusic = playMusic;
        this.playSFX = playSFX;
        this.playerLanguagePref = playerLanguagePref;
        this.heroIndex = heroIndex;
    }

    public override string ToString()
    {
        return $"Play Music is {playMusic}, PlaySFX is {playSFX}, PlayerLanguagePref is {playerLanguagePref}";
    }
}