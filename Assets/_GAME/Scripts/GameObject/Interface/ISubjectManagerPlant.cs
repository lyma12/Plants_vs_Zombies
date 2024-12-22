public interface ISubjectManagerPlant{
    public void AttachPlant(IPlant plantObserver);
    public void DetachPlant(IPlant plantObserver);
    public void PlantNotify();
}