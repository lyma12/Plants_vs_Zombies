using UnityEngine.EventSystems;

public interface IPointerEventObserver: IObserver{
    public void OnPointerDown(PointerEventData pointerEventData);
    public void OnPointerUp(PointerEventData pointerEventData);
}