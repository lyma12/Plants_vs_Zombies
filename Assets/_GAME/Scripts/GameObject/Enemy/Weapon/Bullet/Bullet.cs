using UnityEngine;
using System;

public class Bullet : GameUnit
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float mass = 1f;
    private Vector3 direction;
    private Action<Enemy> OnHitVictim;
    private bool isActive = false;
    public void SetUp(Action<Enemy> onHitVictim, Vector3 direction, Vector3 endPoint){
        OnHitVictim = onHitVictim;
        this.direction = direction;
        isActive = true;
        Invoke(nameof(Despawn), TimeDespawn(transform.position, endPoint));
    }
    protected virtual void FixedUpdate() {
        if(isActive){
            rb.AddForce(direction * shootForce);
        }
    }
    private float TimeDespawn(Vector3 startPoint, Vector3 endPoint){
        float distance = Vector3.Distance(startPoint, endPoint);
        float acceleration = shootForce / mass;
        return Mathf.Sqrt((2 * distance) / acceleration);
    }
    public override void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        isActive = false;
    }
    private void Despawn(){
        SimplePool.Despawn(this);
    }
    public override void OnInit()
    {
    }
    private void OnCollisionEnter(Collision other){
        Enemy victim = Cache.Instance.GetEnemy(other.gameObject);
        if(victim != null){
            OnHitVictim?.Invoke(victim);
        }
        SimplePool.Despawn(this);
    }
}