using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LanguageCarousel : BaseCarouselScript
{

  public TMP_Text languageText;
  public override void Start()
  {
    base.Start();
  }

  public override void Update()
  {
    base.Update();

  }

  public override void OnSwipeComplete()
  {
    base.OnSwipeComplete();
    languageText.text = MainAppManager.mainAppManager.languages[current_index];
    MainAppManager.mainAppManager.languageIndex = current_index;
    PlayerPrefs.SetInt("Lang",current_index);
  }
}
