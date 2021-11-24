using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarouselView : BaseCarouselScript {
    
    public TMP_Text characterName;
    public TMP_Text characterDescription;
    public TMP_Text characterProfileName;
    public TMP_Text characterProfileAge;
    public Image characterProfileImageBust;
    public Image characterProfileCountry;
    public TMP_Text characterProfileSuperPower;
    public TMP_Text characterProfileGoal;
    public TMP_Text characterProfileCountryName;
    

    #region mono
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        characterName.text = MainAppManager.mainAppManager.characters[current_index].characterName;
        characterDescription.text = MainAppManager.mainAppManager.characters[current_index].characterDescriptionText;
    }
    

    // Update is called once per frame
    public override void Update () {

       base.Update();
       
    }
    #endregion


    #region private methods

    public override void OnSwipeComplete()
    {
        base.OnSwipeComplete();
        characterName.text = MainAppManager.mainAppManager.characters[current_index].characterName;
        characterDescription.text = MainAppManager.mainAppManager.characters[current_index].characterDescriptionText;
        MainAppManager.mainAppManager.characterCarouselIndex = current_index;
    }
    
    #endregion



    #region public methods
 
    public void SetCharacterProfileInformation()
        {
            characterProfileName.text = MainAppManager.mainAppManager.characters[current_index].characterName;
            characterProfileAge.text =
                $" {MainAppManager.mainAppManager.characters[current_index].characterAge.ToString()} years";
            characterProfileImageBust.sprite =
                MainAppManager.mainAppManager.characters[current_index].characterBustImage_Level_1;
            characterProfileCountry.sprite = MainAppManager.mainAppManager.characters[current_index].characterCountryImage;
            characterProfileCountryName.text = MainAppManager.mainAppManager.characters[current_index].characterCountry;
            characterProfileSuperPower.text =
                MainAppManager.mainAppManager.characters[current_index].characterSuperPowerText;
            characterProfileGoal.text = MainAppManager.mainAppManager.characters[current_index].characterGoalText;
        }
    #endregion

    
}
