
using PlantsVsZombies.Enemy;

public interface IPlantGrid
{
    public void OnDelete();
    public Player PlayerOn();
    public Enemy GetEnemy();
}
