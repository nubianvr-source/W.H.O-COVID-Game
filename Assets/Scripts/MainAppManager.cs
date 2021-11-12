using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterEnumValue
{
    AYO = 0,
    BACHIR = 1,
    IYANA = 2,
    MARIUS = 3,
    
}

public class MainAppManager : MonoBehaviour
{
    public Character[] characters;
    public static MainAppManager mainAppManager;
    public int characterCarouselIndex;
    private void Awake()
    {
        mainAppManager = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
