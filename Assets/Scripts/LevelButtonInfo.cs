using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonInfo : MonoBehaviour
{
   public Image levelImage;
   public TMP_Text levelNumber;
   public TMP_Text levelSubHeader;
   public Image levelLockStatus;
   private int _sceneNumber;

   public void LoadScene()
   {
      MainAppManager.mainAppManager.uiManager.LoadQuizScene(_sceneNumber);
   }

   public void SetUpBtn(Sprite btnLevelSprite, string levelNumText, string levelSubText, int sceneNumber)
   {
      _sceneNumber = sceneNumber;
      levelImage.sprite = btnLevelSprite;
      levelNumber.text = levelNumText;
      levelSubHeader.text = levelSubText;
   }

   public void BtnSelected()
   {
      MainAppManager.mainAppManager.OpenLevelStartMenu(_sceneNumber);
   }


}
