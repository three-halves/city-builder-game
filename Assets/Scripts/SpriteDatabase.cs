using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Sprite Database", menuName = "Tile Sprite Database")]
public class SpriteDatabase : ScriptableObject
{
    // Index of sprite corresponds to biome enum
    [SerializeField] public List<Sprite> Tiles;

    // Building sprites, indexed arbitrarily and set in building component
    [SerializeField] public List<Sprite> Buildings;

    // Battle character sprites
    [SerializeField] public List<Sprite> Battlers;

    [SerializeField] public List<Sprite> Statuses;
}