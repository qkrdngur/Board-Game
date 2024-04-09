using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "player", menuName = "SO/player")]
public class PlayerSO : ScriptableObject
{
    public List<Sprite> Img;
    public List<string> Name;
    public List<int> Money;
}
