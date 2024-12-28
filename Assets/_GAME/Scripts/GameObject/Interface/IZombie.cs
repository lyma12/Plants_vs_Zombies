using PlantsVsZombies.Enemy;

public interface IZombie: IPlantGrid, IMoveWithDirection{
    public void Eat(Enemy plant, IGround ground);
}