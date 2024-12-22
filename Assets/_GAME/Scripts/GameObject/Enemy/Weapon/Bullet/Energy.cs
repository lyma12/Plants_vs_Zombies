using System.Collections;
using UnityEngine;
public abstract class Energy : GameUnitListenerPointerEvent
{
    [SerializeField] private Rigidbody rb;
    protected bool isSelect = false;
    protected int energy = 0;
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
        isSelect = false;
        energy = 0;
    }
}