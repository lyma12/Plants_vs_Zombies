using UnityEngine.EventSystems;
using UnityEngine;
public class SunFlower : EnemyAdddEnergy, IPlant
{
    public void AttackZombie()
    {
        if(isBeginStep){
            isBeginStep = false;
            return;
        }
        AddEnergy();
    }
    public void OnHitVictim(Enemy enemy){
        
    }

    public override void OnBeAttack()
    {
        GroundPlant.OnRemoveEnemy();
    }

    public override void OnDespawn()
    {
        isBeginStep = true;
        GameStateManager.Instance.DetachPlant(this);
    }

    public override void OnInit()
    {
        base.OnInit();
        energyNumber = initEnergy;
        GameStateManager.Instance.AttachPlant(this);
    }

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        
    }

    public override int EnergyOneTurn()
    {
        return energyNumber;
    }
}