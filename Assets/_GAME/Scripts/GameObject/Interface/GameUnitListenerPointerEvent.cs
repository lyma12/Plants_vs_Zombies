using UnityEngine.EventSystems;

public abstract class GameUnitListenerPointerEvent : GameUnit, IPointerEventObserver
{
    public abstract void OnPointerDown(PointerEventData pointerEventData);

    public abstract void OnPointerUp(PointerEventData pointerEventData);

    public virtual void UpdateNotify(ISubject subject)
    {
        if(subject is PointerEventSubject){
            PointerEventSubject pointerSubject = subject as PointerEventSubject;
            switch(pointerSubject.PointerEvent){
                case PointerEvent.PointerDown:
                OnPointerDown(pointerSubject.PointerEventData);
                break;
                case PointerEvent.PointerUp:
                OnPointerUp(pointerSubject.PointerEventData);
                break;
            }
        }
    }
}