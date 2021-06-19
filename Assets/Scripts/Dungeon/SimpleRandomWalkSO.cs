using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimplerandomWalkParameters_" , menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLenght = 10;
    public bool startRandomlyEachIteration = true;
}
