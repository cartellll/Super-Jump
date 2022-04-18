using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointManager : MonoBehaviour
{
    [SerializeField] private ScriptableObject config;
    public ScriptableObject Config => config;

    [SerializeField]
    private List<PointController> _points = new List<PointController>();

    
   /* [Button("UpdatePoints")]
    public void UpdateSpawnPoints()
    {
        #if UNITY_EDITOR
        _points.Clear();
        var spawnPoints = GetComponentsInChildren<PointController>();
        if (spawnPoints != null)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                _points.Add(spawnPoint);
            }
        }
        
        
        
        PrefabUtility.RecordPrefabInstancePropertyModifications(transform);
        EditorUtility.SetDirty(transform);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Floor18Log.Log($"points updated, total points: {_points.Count}");
        #endif
    }*/

    public List<PointController> GetSpawnPoints()
    {
        return _points;
    }

    public void ResetSpawns()
    {
        foreach (var point in _points)
        {
          //  point.ResetPoint();
        }
    }
}
