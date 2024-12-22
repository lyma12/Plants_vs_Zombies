using UnityEngine;

public abstract class EnemyAdddEnergy: Enemy{
    [SerializeField] protected int initEnergy = 5;
    [SerializeField] private Energy energyPrefab;
    [SerializeField] private Transform weapon;
    protected bool isBeginStep = true;
    protected int energyNumber = 0;
    public abstract int EnergyOneTurn();
    public void AddEnergy(){
        if(energyPrefab != null){
            Energy energy = SimplePool.Spawn<Energy>(energyPrefab, weapon.transform.position, Quaternion.identity);
            energy.Setup(energyNumber);
        }
    }
}