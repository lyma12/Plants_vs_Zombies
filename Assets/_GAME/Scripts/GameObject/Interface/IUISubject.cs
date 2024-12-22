public interface IUISubject{
    public void AttachUI(IUIObserver uIObserver);
    public void DetachUI(IUIObserver uIObserver);
    public void NotifyUI();
}