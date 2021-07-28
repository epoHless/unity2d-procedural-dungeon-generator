using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room-Parameter-N_" , menuName = "epoHless Dungeon Generator/Room Data")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLenght = 10;
    public bool startRandomlyEachIteration = true;
}
