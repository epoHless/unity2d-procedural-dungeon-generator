using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [Foldout("Tiles Settings", true)]
    [SerializeField] protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
