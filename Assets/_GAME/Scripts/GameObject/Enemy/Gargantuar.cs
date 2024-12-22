using UnityEngine;
public class Gargantuar: Zombie{
    [SerializeField] private Zombie impPrefab;
    [SerializeField] private float timeDespawn = 1f;
    private IGround groundNextPlant;
    public override void OnBeAttack()
    {
        groundNextPlant = GroundPlant;
        Invoke(nameof(SpawnImp), timeDespawn);
        base.OnBeAttack();
    }
    private void SpawnImp(){
        groundNextPlant.PlantEnemy(impPrefab);
    }
}