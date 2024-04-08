using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "name" ,menuName = "SO/name")]
public class BlockName : ScriptableObject
{
    public List<string> RegionName;
    public List<int> BuildPrice;
}
