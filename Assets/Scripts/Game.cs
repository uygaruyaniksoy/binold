using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject GameObjects;
    public GameObject Result;
    public GameObject[] Prefabs;
    private readonly Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();
    private struct ObjTransform {
        public readonly string Name;
        public readonly Vector3 Pos;
        public ObjTransform(string name, Vector3 pos) {
            Name = name;
            Pos = pos;
        }
    }
    
    private void Start() {
        foreach (var prefab in Prefabs) {
            _dictionary.Add(prefab.name, prefab);
        }
        CreateLevel();
    }

    public void CreateLevel(int level = 0) {
        ClearLevel();
        CreateLevelObjects(level);
        CreateResultObjects(level);
    }

    public bool CheckValidity() {
        var results = new List<ObjTransform>();
        var objects = new List<ObjTransform>();
        var resCenter = new Vector3(0,0,0);
        var objCenter = new Vector3(0,0,0);
        var objCount = Result.transform.childCount;
        
        for (var i = 0; i < objCount; i++) {
            var res = Result.transform.GetChild(i);
            var obj = GameObjects.transform.GetChild(i);
            results.Add(new ObjTransform(res.name, new Vector3(res.position.x, res.position.y, res.position.z)));
            objects.Add(new ObjTransform(obj.name, new Vector3(obj.position.x, obj.position.y, obj.position.z)));
            resCenter += res.position;
            objCenter += obj.position;
        }
        resCenter *= 1f / objCount;
        objCenter *= 1f / objCount;
        
        // Normalize
        for (var i = 0; i < objCount; i++) {
            results[i] = new ObjTransform(results[i].Name, results[i].Pos - resCenter);
            objects[i] = new ObjTransform(objects[i].Name, objects[i].Pos - objCenter);
        }
        var distance = 0f;
        for (var i = 0; i < objCount; i++) {
            var res = results[i];
            ObjTransform closest = objects[0];
            var closestDist = float.PositiveInfinity;
            foreach (var obj in objects) {
                if ((obj.Pos - res.Pos).magnitude > closestDist || !obj.Name.Equals(res.Name)) continue;
                closest = obj;
                closestDist = (obj.Pos - res.Pos).magnitude;
            }
            distance += closestDist;
            objects.Remove(closest);
        }
//        Debug.Log("Distance is: " + distance);

        return distance < objCount * 0.05;
    }

    public void HandleLevelOver() {
        Debug.Log("Level is over");
    }
    
    private void ClearLevel() {
        for (int i = 0; i < GameObjects.transform.childCount; i++) {
            Destroy(GameObjects.transform.GetChild(i).gameObject);
        }
    }

    private void CreateLevelObjects(int level) {
        var levelData = Resources.Load<TextAsset>("Levels/_slevel" + level);
        var objects = levelData.text.Split('\n');
        for (int i = 1; i < objects.Length - 1; i++) {
            var opts = objects[i].Split(',');
            var obj = Instantiate(_dictionary[opts[0]],
                new Vector3(float.Parse(opts[1]), float.Parse(opts[2]), float.Parse(opts[3])),
                Quaternion.Euler(float.Parse(opts[4]), float.Parse(opts[5]), float.Parse(opts[6]))
            );
            obj.transform.SetParent(transform);
            var arrow = Instantiate(_dictionary["Arrow"],
                new Vector3(float.Parse(opts[1]), float.Parse(opts[2]), float.Parse(opts[3])),
                Quaternion.Euler(0,0,0)
            );
            obj.GetComponentInChildren<Highlightable>().arrow = arrow;
            arrow.transform.SetParent(obj.transform);
        }
        
    }
    
    private void CreateResultObjects(int level) {
        var levelData = Resources.Load<TextAsset>("Levels/_elevel" + level);
        var objects = levelData.text.Split('\n');
        for (int i = 1; i < objects.Length - 1; i++) {
            var opts = objects[i].Split(',');
            var obj = Instantiate(_dictionary[opts[0]],
                new Vector3(float.Parse(opts[1]) + Result.transform.position.x, float.Parse(opts[2]) + Result.transform.position.y, float.Parse(opts[3]) + Result.transform.position.z),
                Quaternion.Euler(float.Parse(opts[4]), float.Parse(opts[5]), float.Parse(opts[6]))
            );
            obj.transform.SetParent(Result.transform);
        }
    }
}
