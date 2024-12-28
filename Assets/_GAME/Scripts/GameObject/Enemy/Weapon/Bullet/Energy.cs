using System.Collections;
using UnityEngine;
public abstract class Energy : GameUnitListenerPointerEvent
{
    [SerializeField] private Rigidbody rb;
    protected bool isSelect = false;
    protected int energy = 0;
    public virtual Player PlayerType {get; set;}
    public void Setup(int energy){
        this.energy = energy;
        rb.AddForce(Vector3.up * AppContanst.FORCE_ENERGY);
        StartCoroutine(SetTimeDespawn());
    }
    private IEnumerator SetTimeDespawn(){
        yield return new WaitForSeconds(AppContanst.TIME_DESPAWN_ENERGY);
        SimplePool.Despawn(this);
    }
    public override void OnDespawn()
    {
        GameStateManager.Instance.AddEnergyForPlayer(PlayerType, energy);
        isSelect = false;
        energy = 0;
    }
}