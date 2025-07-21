using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Database", menuName = "Building Database")]
public class BuildingDatabase : ScriptableObject
{
    // Building prefabs
    [SerializeField] public List<GameObject> Buildings;
}