using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameUnitListenerPointerEvent
{
    [SerializeField] private float heightEnemy = 2f;
    [SerializeField] private Player type = Player.NONE;
    [SerializeField] protected List<Direction> gridCanMove;
    [SerializeField] protected LayerMask layerMaskGround;
    [SerializeField] protected GameObject iconDecoratePlant;
    [SerializeField] protected GameObject iconDecorateZombie;
    [SerializeField] protected Animator anim;
    protected string currentAnim = AnimationName.Idle;
    protected IGround groundPlant;
    public virtual IGround GroundPlant {
        get {
            return groundPlant;
        }
        set{
            groundPlant = value;
        }
    }
    public List<Direction> GridMove => gridCanMove;
    public Player TypePlayer
    {
        get
        {
            return type;
        }
        internal set{
            type = value;
            if(this is IPlant && value == Player.ZOMBIE_PLAYER){
                DecorateWithZombieIcon();
                return;
            }
            if(this is IZombie && value == Player.PLANT_PLAYER){
                DecorateWithPlantIcon();
                return;
            }
            ResetDecorate();
        }
    }
    public override void OnInit()
    {
        ResetDecorate();
    }

    public float Height
    {
        get
        {
            return heightEnemy;
        }
    }
    public abstract void OnBeAttack();
    public virtual void OnChangeAnimation(string newAim)
    {
        if (newAim.Contains(currentAnim)) return;
        if (anim != null)
        {
            anim.SetTrigger(newAim);
        }
        currentAnim = newAim;
    }
    private void DecorateWithZombieIcon(){
        iconDecoratePlant.SetActive(false);
        iconDecorateZombie.SetActive(true);
    }
    private void DecorateWithPlantIcon(){
        iconDecoratePlant.SetActive(true);
        iconDecorateZombie.SetActive(false);
    }
    private void ResetDecorate(){
        iconDecoratePlant.SetActive(false);
        iconDecorateZombie.SetActive(false);
    }
    public Enemy Clone(){
        return (Enemy)this.MemberwiseClone();
    }
}

public abstract class HypnosisEnemy: Enemy{
    public void Hypnosis(Enemy otherEnemy, Player playerTypeChange){
        otherEnemy.TypePlayer = playerTypeChange;
    }
}