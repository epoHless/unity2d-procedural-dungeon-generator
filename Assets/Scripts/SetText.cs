using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    private Text floorText;

    private void Awake()
    {
        floorText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        floorText.text =
            $"Current Floor: {RoomDungeonGenerator.instance.CurrentFloor()} / {RoomDungeonGenerator.instance.MaxFloor()}";
    }
}
