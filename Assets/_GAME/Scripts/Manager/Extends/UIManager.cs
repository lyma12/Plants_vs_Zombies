using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager> {
    Dictionary<Type, UICanvas> canvasPrefab = new Dictionary<Type, UICanvas>();
    Dictionary<Type, UICanvas> canvasActive = new Dictionary<Type, UICanvas>();
    public event UnityAction OnUIInitialized;
    [SerializeField]
    private Transform parentCanvas;
    private void Awake() {
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("Prefab/UI");
        for(int i = 0; i < prefabs.Length; i++){
            canvasPrefab.Add(prefabs[i].GetType(), prefabs[i]);
        }
        OnUIInitialized?.Invoke();
    }
    public T OpenUI<T>() where T: UICanvas{
        T canvas = GetUI<T>();
        canvas.Setup();
        canvas.Open();
        return canvas;
    }
    public T CloseAndOpenUI<T>() where T : UICanvas{
        foreach(var canva in canvasActive){
            canva.Value.CloseDirectly();
        }
        return OpenUI<T>();
    }
    public void Close<T>(float time) where T : UICanvas{
        if(IsUIOpened<T>()) return;
        Invoke(nameof(CloseUIDirectly), time);
    }
    public void CloseUIDirectly<T>() where T : UICanvas{
        if(IsUIOpened<T>()) return;
        canvasActive[typeof(T)].Close(0);
    }
    public bool IsUILoaded<T>() where T : UICanvas{
        return canvasActive.ContainsKey(typeof(T)) && canvasActive[typeof(T)] != null;
    }
    public bool IsUIOpened<T>() where T: UICanvas{
        return IsUILoaded<T>() && canvasActive[typeof(T)].gameObject.activeSelf;
    }
    public T GetUI<T>() where T : UICanvas{
        if(!IsUILoaded<T>()){
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parentCanvas);
            canvasActive[typeof(T)] = canvas;
        }
        return canvasActive[typeof(T)] as T;
    }
    private T GetUIPrefab<T>() where T: UICanvas{
        if(canvasPrefab.ContainsKey(typeof(T))){
            return canvasPrefab[typeof(T)] as T;
        }
        else throw new Exception("nullable prefab ui canvas");
    }

    public void CloseAll(){
        foreach(var canvas in canvasActive){
            if(canvas.Value != null && canvas.Value.gameObject.activeSelf){
                canvas.Value.Close(0);
            }
        }
    }
}