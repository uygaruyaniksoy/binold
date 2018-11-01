using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public GameObject GameObjects;
    public GameObject Result;
    public GameObject TopRight;
    public GameObject Arrow;
    public GameObject[] Prefabs;
    public GameObject PauseButton;
    public GameObject PauseMenu;
    public GameObject LevelOverMenu;
    public Border InnerBorder;
    private readonly Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();
    private struct ObjTransform {
        public readonly string Name;
        public readonly Vector3 Pos;
        public ObjTransform(string name, Vector3 pos) {
            Name = name;
            Pos = pos;
        }
    }

    [HideInInspector]
    public int XAXIS = 1;
    [HideInInspector]
    public int YAXIS = 2;
    [HideInInspector]
    public int ZAXIS = 4;

    private int level;
    
    public string LevelStatus;
    public int page = 0;
    public int pageLimit = 4;

    public CanvasScaler MainCanvasScaler;

    private void Start() {
        //MainCanvasScaler.scaleFactor = Screen.width / 800f; 
        //string levels = PlayerPrefs.GetString("Levels", "\0\0\0\0");
        LevelStatus = PlayerPrefs.GetString("Levels", "" +
            // pagelimit(4) * 10 levels
            (char)1024 + 
            (char)1024 + 
            (char)1024 + 
            (char)1024);
        
        foreach (var prefab in Prefabs) {
            _dictionary.Add(prefab.name, prefab);
        }
        
        // hide minimap before levels start and lock rotation
        Camera.main.GetComponent<RotateCamera>().Lock = true;
        TopRight.SetActive(false);
//        CreateLevel();
        
        Camera.main.GetComponent<RotateCamera>().CheckSideValidity();
    }
    
    public void CreateLevel(int level = 0) {
        this.level = level;
        ClearLevel();
        CreateLevelObjects(level);
        CreateResultObjects(level);
        ResumeGame();
    }

    public bool CheckValidity(int axes = 7) {
        var results = new List<ObjTransform>();
        var objects = new List<ObjTransform>();
        var resCenter = new Vector3(0,0,0);
        var objCenter = new Vector3(0,0,0);
        var objCount = Result.transform.childCount;
        
        for (var i = 0; i < objCount; i++) {
            var res = Result.transform.GetChild(i);
            var obj = GameObjects.transform.GetChild(i);
            results.Add(new ObjTransform(res.name, new Vector3(
                (axes & XAXIS) > 0 ? res.position.x : 0, 
                (axes & YAXIS) > 0 ? res.position.y : 0,
                (axes & ZAXIS) > 0 ? res.position.z : 0)));
            objects.Add(new ObjTransform(obj.name, new Vector3(
                (axes & XAXIS) > 0 ? obj.position.x : 0, 
                (axes & YAXIS) > 0 ? obj.position.y : 0,
                (axes & ZAXIS) > 0 ? obj.position.z : 0)));
            resCenter += results[i].Pos;
            objCenter += objects[i].Pos;
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

        return distance < objCount * 0.05;
    }

    public void HandleLevelOver() {
        Camera.main.GetComponent<RotateCamera>().Lock = true;
        Highlightable.Locked = true;
        PauseButton.SetActive(false);
        LevelOverMenu.SetActive(true);
        LevelOverMenu.GetComponent<LevelOver>().Animate();
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
            Camera.main.transform.GetChild(1).GetComponent<Glow>().Deactivate();
        }
        Debug.Log("Level is over");
        TopRight.SetActive(false);
        var ch = LevelStatus[page];
        LevelStatus = LevelStatus.Remove(page, 1);
        LevelStatus = LevelStatus.Insert(page, ""+(char)(ch| (1 << (level % 10)))); 
        
        PlayerPrefs.SetString("Levels", LevelStatus);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void PauseGame() {
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
            Camera.main.transform.GetChild(1).GetComponent<Glow>().Deactivate();
        }
        PauseButton.SetActive(false);
        PauseMenu.SetActive(true);
        TopRight.SetActive(false);
        Camera.main.GetComponent<RotateCamera>().Lock = true;
        Highlightable.Locked = true;
    }
    
    public void ResumeGame() {
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
        TopRight.SetActive(true);
        Camera.main.GetComponent<RotateCamera>().Lock = false;
        Highlightable.Locked = false;
    }

    public void ClearLevel() {
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
            Camera.main.transform.GetChild(1).GetComponent<Glow>().Deactivate();
        }
        for (int i = 0; i < GameObjects.transform.childCount; i++) {
            Destroy(GameObjects.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Result.transform.childCount; i++) {
            Destroy(Result.transform.GetChild(i).gameObject);
        }
        TopRight.SetActive(false);
    }

    public void NextLevel() {
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
            Camera.main.transform.GetChild(1).GetComponent<Glow>().Deactivate();
        }
        Camera.main.GetComponent<RotateCamera>().Lock = false;
        Highlightable.Locked = false;
        LevelOverMenu.SetActive(false);
        CreateLevel(++level);
    }

    public void Mailto() {
        Application.OpenURL("mailto:uygaruyaniksoy@gmail.com?subject=binold");
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
            var arrow = Instantiate(Arrow,
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
