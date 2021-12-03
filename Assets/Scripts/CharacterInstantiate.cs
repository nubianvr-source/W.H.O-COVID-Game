using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantiate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(
            MainAppManager.mainAppManager.characters[MainAppManager.mainAppManager.characterCarouselIndex]
                .animatedCharacterLevel1, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
