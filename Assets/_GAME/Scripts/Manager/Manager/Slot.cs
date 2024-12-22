public abstract class Slot{
    protected Player player;
    private float energy = 0;
    protected bool isFirstStep = false;
    public Player PlayerType{
        get { return player;}
    }
    public float EnergyCoin{
        get { return energy;}
    }
    public virtual void AddEnergy(float energyCoin){
        if(energyCoin > 0){
            energy += energyCoin;
        }
    }
    public Slot(Player playerType, bool isFirstStep, float initEnergy = 10){
        player = playerType;
        this.isFirstStep = isFirstStep;
        energy = initEnergy;
    }
    public virtual void BuyEnemy(float energyCoin){
        energy -= energyCoin;
    }
    public bool CanBuyEnemy(float energyCoin){
        if(energy <= 0) return false;
        return energy >= energyCoin;
    }
    public abstract void TurnPass(Player playerType);
}