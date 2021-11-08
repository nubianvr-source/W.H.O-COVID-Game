using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    //Properties...
    [SerializeField]private TMP_Text gameMusicOnOffText;
    [SerializeField]private Image gameMusicSlashImage;
    [SerializeField]private TMP_Text soundEffectOnOffText;
    [SerializeField]private Image soundEffectSlashImage;
    private SoundManager _soundManager;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("PlaySFX") == 1)
        {
            gameMusicSlashImage.gameObject.SetActive(false);
        }
        else
        {
            gameMusicSlashImage.gameObject.SetActive(true);
        }
        
        if (PlayerPrefs.GetInt("PlaySFX") == 1)
        { 
            soundEffectSlashImage.gameObject.SetActive(false);
        }
        else
        { 
            soundEffectSlashImage.gameObject.SetActive(true);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MuteSFX()
    {
        if (PlayerPrefs.GetInt("PlaySFX") == 1)
        {
            PlayerPrefs.SetInt("PlaySFX", 0);
            if (soundEffectSlashImage && soundEffectOnOffText)
            {
                soundEffectSlashImage.gameObject.SetActive(true);
                soundEffectOnOffText.text = "Off";
            }
        }
        else
        {
            PlayerPrefs.SetInt("PlaySFX", 1);
            if (soundEffectSlashImage && soundEffectOnOffText)
            {
                soundEffectSlashImage.gameObject.SetActive(false);
                soundEffectOnOffText.text = "On";
            }
        }
    }

    public void MuteMusic()
    {
        if(PlayerPrefs.GetInt("PlayMusic") == 1)
        {
            PlayerPrefs.SetInt("PlayMusic", 0);
            _soundManager.StopAllMusic();
            if (gameMusicSlashImage && gameMusicOnOffText)
            {
                gameMusicSlashImage.gameObject.SetActive(true);
                gameMusicOnOffText.text = "Off";
            }
        }
        else
        {
            PlayerPrefs.SetInt("PlayMusic", 1);
            _soundManager.PlayAudio("BGMusic");
            if (gameMusicSlashImage && gameMusicOnOffText)
            {
                gameMusicSlashImage.gameObject.SetActive(false);
                gameMusicOnOffText.text = "On";
            }
        }
    }

}
