using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    //Properties...
    public static Settings singleton;
    [SerializeField]private TMP_Text gameMusicOnOffText;
    [SerializeField] private Image gameMusicSlashImage;
    private bool musicIsMute = true;
    [SerializeField]private TMP_Text soundEffectOnOffText;
    [SerializeField]private Image soundEffectSlashImage;

    private void Awake()
    {
        if(!singleton)
        {
            singleton = this;
        }
        if(singleton!=this)
        {
            Destroy(singleton);
            DontDestroyOnLoad(singleton);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameMusicSlashImage)
            gameMusicSlashImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MuteMusic()
    {
        //muteMusic = false;
        if(musicIsMute)
        {
            musicIsMute = false;
            Camera.main.GetComponent<AudioSource>().Stop();
            if (gameMusicSlashImage && gameMusicOnOffText)
            {
                gameMusicSlashImage.gameObject.SetActive(true);
                gameMusicOnOffText.text = "Off";
            }
        }
        else
        {
            musicIsMute = true;
            Camera.main.GetComponent<AudioSource>().Play();
            if (gameMusicSlashImage && gameMusicOnOffText)
            {
                gameMusicSlashImage.gameObject.SetActive(false);
                gameMusicOnOffText.text = "On";
            }
        }
    }

    private void MuteSoundEffects(bool condition)
    {
        Camera.main.GetComponent<AudioSource>().mute = condition;
    }
}
