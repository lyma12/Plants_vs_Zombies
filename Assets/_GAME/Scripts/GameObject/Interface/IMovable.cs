using UnityEngine.EventSystems;

public interface IMovable: IPointerEventObserver{
    public void OnDrag(PointerEventData pointerEventData);
}