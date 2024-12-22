using UnityEngine;

public class UICanvas : MonoBehaviour {
    [SerializeField]
    protected bool isDestroyOnClose = false;
    protected virtual void Awake() {
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if(ratio > 2.1f){
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
        Canvas canvas = GetComponent<Canvas>();
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera){
            canvas.worldCamera = Camera.main;
        }
        OnInit();
    }
    protected virtual void OnInit(){}
    public virtual void Setup(){}
    public virtual void Open(){
        gameObject.SetActive(true);
    }
    public virtual void OpenTime(float time){
        Invoke(nameof(Open), time);
    }
    public virtual void Close(float time){
        Invoke(nameof(CloseDirectly), time);
    }
    public virtual void CloseDirectly(){
        if(isDestroyOnClose){
            Destroy(gameObject);
        }
        else{
            gameObject.SetActive(false);
        }
    }
}