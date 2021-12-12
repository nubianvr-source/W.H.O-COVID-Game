using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "New Badge", menuName = "Badge/Add New Badge")]
public class Badges : ScriptableObject
{
   public Sprite litBadge;
   public Sprite unlitBadge;
}
