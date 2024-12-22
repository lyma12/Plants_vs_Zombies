using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class PointerEventSubject : UICanvas, IPointerDownHandler, IDragHandler, IPointerUpHandler, ISubject
{
    [SerializeField] protected LayerMask pointerLayerMask;
    protected List<IPointerEventObserver> observers = new List<IPointerEventObserver>();
    protected PointerEventData pointerEventData;
    public PointerEventData PointerEventData => pointerEventData;
    public PointerEvent PointerEvent{get; protected set;}
    public void Attach(IObserver observer)
    {
        if(observer is IPointerEventObserver){
            IPointerEventObserver pointerObserver = observer as IPointerEventObserver;
            if(observers.Contains(pointerObserver)) return;
            observers.Add(pointerObserver);
        }
    }

    public void Detach(IObserver observer)
    {
        if(observer is IPointerEventObserver){
            IPointerEventObserver pointerObserver = observer as IPointerEventObserver;
            if(observers.Contains(pointerObserver)) observers.Remove(pointerObserver);
        }
    }

    public void Notify()
    {
        List<IPointerEventObserver> observersCopy = new List<IPointerEventObserver>(observers);
        foreach(IPointerEventObserver observer in observersCopy){
            observer.UpdateNotify(this);
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        PointerEvent = PointerEvent.PointerOnDrag;
        pointerEventData = eventData;
        Notify();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePosition = eventData.position;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100, pointerLayerMask)){
            PointerEvent = PointerEvent.PointerDown;
            pointerEventData = eventData;
            GameObject gameObject = hitData.collider.gameObject;
            IPointerEventObserver pointerEventObserver = Cache.Instance.GetGameUnitListenerPointerEvent(gameObject);
            if(pointerEventObserver == null) return;
            Attach(pointerEventObserver);
        }
        else{
            PointerEvent = PointerEvent.None;
            pointerEventData = null;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Vector3 mousePosition = eventData.position;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100, pointerLayerMask)){
            PointerEvent = PointerEvent.PointerUp;
            pointerEventData = eventData;
            GameObject gameObject = hitData.collider.gameObject;
            IPointerEventObserver pointerEventObserver = Cache.Instance.GetGameUnitListenerPointerEvent(gameObject);
            if(pointerEventObserver != null){
                pointerEventObserver.OnPointerUp(eventData);
                Detach(pointerEventObserver);
            }
        }
        else{
            pointerEventData = null;
            PointerEvent = PointerEvent.None;
        }
    }
}