using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUtilitySO", menuName = "New Utility SO")]
public class SO_Utility : ScriptableObject
{
    public List<Sprite> itemIcons;
    public List<GameObject> gameItems;
    public List<string> itemTags;
}
