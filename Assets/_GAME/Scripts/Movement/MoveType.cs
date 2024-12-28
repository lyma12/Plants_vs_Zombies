using System.Drawing;
using PlantsVsZombies.Enemy;

public abstract class MoveType{
    protected Point dir;
    public MoveType(Point dir){
        this.dir = dir;
    }
    public abstract StateGame MakeMove(StateGame oldState);
    public abstract void MakeMove(Grid1x1[,] broad, Slot slot);
    protected StateGame CheckAddEnergy(StateGame stateGame){
        foreach(IGround grid in stateGame.Broad){
            if(grid.GetEnemyPlantOn() == null) continue;
            if(grid.GetEnemyPlantOn() is EnemyAdddEnergy){
                if(grid.GetEnemyPlantOn().TypePlayer == stateGame.BotPlayerType){
                    stateGame.EnergyBot += (grid.GetEnemyPlantOn() as EnemyAdddEnergy).EnergyOneTurn();
                }
                else{
                    stateGame.EnergyPlayer += (grid.GetEnemyPlantOn() as EnemyAdddEnergy).EnergyOneTurn();
                }
            }
        }
        return stateGame;
    }
    public Point Dir => dir;
}

public class MoveAddEnemy: MoveType{
    private Enemy enemy;
    private int priceBuy;
    private Player playerBuyEnemy;
    public MoveAddEnemy(Point dir, Enemy enemy, int priceBuy, Player playerBuy): base(dir){
        this.dir = dir;
        this.enemy = enemy;
        this.priceBuy =  priceBuy;
        this.playerBuyEnemy = playerBuy;
    }

    public override StateGame MakeMove(StateGame oldState)
    {
        StateGame stateGame = new StateGame(oldState.Broad, oldState.EnergyBot, oldState.EnergyPlayer, oldState.Size, oldState.BotPlayerType);
        stateGame = CheckAddEnergy(stateGame);
        stateGame.Broad[dir.X, dir.Y].PlantEnemy(enemy);
        if(playerBuyEnemy == stateGame.BotPlayerType){
            stateGame.EnergyBot -= priceBuy;
        }
        else{
            stateGame.EnergyPlayer -= priceBuy;
        }
        return stateGame;
    }

    public override void MakeMove(Grid1x1[,] broad, Slot slot)
    {
        broad[dir.X, dir.Y].PlantEnemy(enemy);
        slot.BuyEnemy(priceBuy);
        GameStateManager.Instance.MakeMove(slot.PlayerType, dir);
    }
}

public class MoveChangeEnemy: MoveType{
    private Point start;
    private Enemy enemy;
    public MoveChangeEnemy(Point start, Point dir, Enemy enemy): base(dir){
        this.start = start;
        this.enemy = enemy;
        this.dir = dir;
    }

    public override StateGame MakeMove(StateGame oldState)
    {
        StateGame stateGame = new StateGame(oldState.Broad, oldState.EnergyBot, oldState.EnergyPlayer, oldState.Size, oldState.BotPlayerType);
        stateGame = CheckAddEnergy(stateGame);
        stateGame.Broad[start.X, start.Y].OnRemoveEnemy();
        stateGame.Broad[dir.X, dir.Y].PlantEnemy(enemy);
        return stateGame;
    }

    public override void MakeMove(Grid1x1[,] broad, Slot slot)
    {
        broad[start.X, start.Y].OnRemoveEnemy();
        broad[dir.X, dir.Y].OnChangeEnemy(enemy);
        GameStateManager.Instance.MakeMove(slot.PlayerType, dir);
    }
}