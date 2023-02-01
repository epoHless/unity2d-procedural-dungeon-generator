using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
     AbstractDungeonGenerator generator;
//     private string[] m_Tabs = {"Settings", "Events"};
//     private int m_TabsSelected = -1;
//
//     private SerializedProperty _tilemapVisualizer;
//     private SerializedProperty _onCreateEvent;
//     private SerializedProperty _onEndEvent;
//     private SerializedProperty _onStairsEvent;
//
//     private SerializedProperty player;
//     private SerializedProperty endPoint;
//
//     protected static bool showDCE = true; 
//     protected static bool showDEE = true; 
//     protected static bool showDSE = true; 
//     
//     private void OnEnable()
//     {
//         _tilemapVisualizer = serializedObject.FindProperty("tilemapVisualizer");
//         _onCreateEvent = serializedObject.FindProperty("onDungeonCreate");
//         _onEndEvent = serializedObject.FindProperty("onDungeonEnd");
//         _onStairsEvent = serializedObject.FindProperty("onReachStairs");
//
//         player = serializedObject.FindProperty("player");
//     }
//
     private void Awake()
     {
         generator = (AbstractDungeonGenerator)target;
     }

     public override void OnInspectorGUI()
     {
//         EditorGUILayout.BeginVertical();
//         m_TabsSelected = GUILayout.Toolbar(m_TabsSelected, m_Tabs);
//         EditorGUILayout.EndVertical();
//
//         if (m_TabsSelected >=0 || m_TabsSelected < m_Tabs.Length)
//         {
//             switch (m_Tabs[m_TabsSelected])
//             {
//                 case "Settings":
//                     EditorGUILayout.PropertyField(player);
//                     // base.OnInspectorGUI();
//                     break;
//                 case "Events":
//                     EditorGUILayout.BeginVertical();
//                     showDCE = EditorGUILayout.Foldout(showDCE, "On Dungeon Create");
//                     if (showDCE)
//                     {
//                         EditorGUILayout.PropertyField(_onCreateEvent);
//                     }
//                     EditorGUILayout.Space();
//                     showDSE = EditorGUILayout.Foldout(showDSE, "On Stairs Reached");
//                     if (showDSE)
//                     {
//                         EditorGUILayout.PropertyField(_onStairsEvent);
//                     }
//                     EditorGUILayout.Space();
//                     showDEE = EditorGUILayout.Foldout(showDEE, "On Dungeon End");
//                     if (showDEE)
//                     {
//                         EditorGUILayout.PropertyField(_onEndEvent);
//                     }
//                     EditorGUILayout.EndVertical();
//                     break;
//             }
//         }
//         
         base.OnInspectorGUI();

         if (GUILayout.Button("Create Dungeon"))
         {
             generator.GenerateDungeon();
         }
     }
}
