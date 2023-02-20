using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemySpawnPosition))]
public class EnemySpawnPositionEditor : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sv)
    {
        EnemySpawnPosition esp = (EnemySpawnPosition)target;

        int posIndex = 0;
        List<Vector2> newTargetPos = new List<Vector2>();
        EditorGUI.BeginChangeCheck();
        foreach (Vector2 pos in esp.enemyPositions)
        {
            Handles.Label(pos + Vector2.up * 0.5f, $"Pos {posIndex}");
            newTargetPos.Add(Handles.PositionHandle(pos, Quaternion.identity));
            Handles.DrawWireDisc(pos, Vector3.forward, 0.5f);
            posIndex++;
        }
        if(EditorGUI.EndChangeCheck())
        {
            for(int i = 0; i < esp.enemyPositions.Count; i++)
            {
                esp.enemyPositions[i] = new Vector2(Mathf.Round(newTargetPos[i].x * 2.0f) / 2.0f, Mathf.Round(newTargetPos[i].y * 2.0f) / 2.0f);
                
            }
            
        }
    }
}
