using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = System.Object;

[CustomEditor(typeof(Game))]
public class GameEditor : Editor {
    private List<string> _startingObjects = new List<string>();
    private List<string> _endObjects = new List<string>();
    private List<string> _objectNames = new List<string>();
    private Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();

    private int _level;

    private void SetupDictionary() {
        foreach (var prefab in Selection.activeGameObject.GetComponent<Game>().Prefabs) {
            _dictionary.Add(prefab.name, prefab);
        }
    }

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Setup Dictionary")) SetupDictionary();
        
        base.OnInspectorGUI();
        GUILayout.Space(16f);
        
        GUILayout.BeginVertical();

        foreach (var prefab in Selection.activeGameObject.GetComponent<Game>().Prefabs) {
            if (GUILayout.Button(prefab.name)) {
                var obj = Instantiate(_dictionary[prefab.name],
                    new Vector3(0,1,-5),
                    Quaternion.Euler(0,0,0)
                );
                obj.transform.SetParent(Selection.activeTransform);
                obj.transform.name = prefab.name;
            }
        }
        
        
        GUILayout.EndVertical();
        
        GUILayout.Space(16f);
        _level = EditorGUILayout.IntField("Level", _level);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("SetStartGame")) {
            _startingObjects.Clear();
            _objectNames.Clear();
            for (int i = 0; i < Selection.activeTransform.childCount; i++) {
                var child = Selection.activeTransform.GetChild(i);
                var strings = new List<string> {
                    child.position.x.ToString(),
                    child.position.y.ToString(),
                    child.position.z.ToString(),
                    child.rotation.x.ToString(),
                    child.rotation.y.ToString(),
                    child.rotation.z.ToString()
                };
                _startingObjects.Add(string.Join(",", strings.ToArray()));
                _objectNames.Add(child.name);
            }
            var file = File.Open("Assets/Resources/Levels/_slevel" + _level + ".csv", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.WriteLine("name,px,py,pz,rx,ry,rz");
            for (var i = 0; i < _startingObjects.Count; i++) {
                var strings = new List<string> {_objectNames[i], _startingObjects[i]};
                var output = string.Join(",", strings.ToArray());
                writer.WriteLine(output);
            }
            writer.Close();
        }
        
        if (GUILayout.Button("SetEndGame")) {
            _endObjects.Clear();
            _objectNames.Clear();
            for (int i = 0; i < Selection.activeTransform.childCount; i++) {
                var child = Selection.activeTransform.GetChild(i);
                var strings = new List<string> {
                    child.position.x.ToString(),
                    child.position.y.ToString(),
                    child.position.z.ToString(),
                    child.rotation.x.ToString(),
                    child.rotation.y.ToString(),
                    child.rotation.z.ToString()
                };
                _endObjects.Add(string.Join(",", strings.ToArray()));
                _objectNames.Add(child.name);
            }
            var file = File.Open("Assets/Resources/Levels/_elevel" + _level + ".csv", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.WriteLine("name,px,py,pz,rx,ry,rz");
            for (var i = 0; i < _endObjects.Count; i++) {
                var strings = new List<string> {_objectNames[i], _endObjects[i]};
                var output = string.Join(",", strings.ToArray());
                writer.WriteLine(output);
            }
            writer.Close();
        }
        
        GUILayout.EndHorizontal();
    }
}
