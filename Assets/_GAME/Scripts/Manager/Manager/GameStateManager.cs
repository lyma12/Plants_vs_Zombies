using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class GameStateManager : Singleton<GameStateManager>, ISubject, ISubjectManagerPlant
{
    [SerializeField] private Broad broad;
    private Grid1x1[,] playerGrid;
    private Player currentPlayer = Player.NONE;
    private Player winner;
    private List<IObserver> observers = new List<IObserver>();
    private List<IPlant> plantObservers = new List<IPlant>();
    private List<Slot> slots = new List<Slot>();
    public Player CurrentPlayer => currentPlayer;
    public Action<Player> OnTurnPass;
    public Grid1x1[,] PlayerGrid => playerGrid;
    public int Size => broad.Size;
    private void Start()
    {
        int n = broad.Size;
        playerGrid = new Grid1x1[n,n];
        for(int i = 0; i < broad.Grid.Count; i++){
            int c = i % 3;
            int r = i / 3;
            playerGrid[c, r] = broad.Grid[i];
            broad.Grid[i].SetColumnAndRow(new Point(c, r));
        }
        foreach(Slot slot in slots){
            slot.TurnPass(currentPlayer);
        }
    }
    public void SetUpPlayerSlot(List<Slot> playerSlot){
        slots = playerSlot;
    }
    public void BuyEnemy(float coin){
        foreach(Slot slot in slots){
            if(slot.PlayerType == currentPlayer){
                slot.BuyEnemy(coin);
                return;
            }
        }
    }
    public Player FirstPlayer(){
        currentPlayer = (Player) UnityEngine.Random.Range(1, 3);
        return currentPlayer;
    }
    private void ChangeTurnPass()
    {
        if (currentPlayer == Player.NONE)
        {
            currentPlayer = Player.PLANT_PLAYER;
            OnTurnPass?.Invoke(currentPlayer);
            foreach(Slot slot in slots){
                slot.TurnPass(currentPlayer);
            }
            return;
        }
        currentPlayer = currentPlayer == Player.PLANT_PLAYER ? Player.ZOMBIE_PLAYER : Player.PLANT_PLAYER;
        OnTurnPass?.Invoke(currentPlayer);
        foreach(Slot slot in slots){
                slot.TurnPass(currentPlayer);
            }
        PlantNotify();
    }
    private bool GameWin(int r, int c)
    {
        int isNum = playerGrid[r, c].PlayerType() == currentPlayer ? 1 : 0;
        int row = CountSymbolWithDirection(Vector2Int.left, r - 1, c) + CountSymbolWithDirection(Vector2Int.right, r + 1, c) + isNum;
        int column = CountSymbolWithDirection(Vector2Int.down, r, c - 1) + CountSymbolWithDirection(Vector2Int.up, r, c + 1) + isNum;
        int primaryDiagonal = CountSymbolWithDirection(new Vector2Int( 1, -1), r + 1, c - 1) + CountSymbolWithDirection(new Vector2Int(-1, 1), r - 1, c + 1) + isNum;
        int secondDiagonal = CountSymbolWithDirection(new Vector2Int(-1, -1), r - 1 , c - 1) + CountSymbolWithDirection(new Vector2Int(1, 1), r + 1 , c + 1) + isNum;
        int size = broad.Size;
        return size <= row || size <= column || size <= primaryDiagonal || size <= secondDiagonal;
    }
    private int CountSymbolWithDirection(Vector2Int direction, int r, int c){
        if(r < 0 || c < 0 || r > broad.Size - 1 || c > broad.Size - 1) return 0;
        if(playerGrid[r, c].PlayerType() != currentPlayer) return 0;
        return 1 + CountSymbolWithDirection(direction, r + direction.x, c + direction.y);
    }

    public void GameOver()
    {
        winner = currentPlayer == Player.PLANT_PLAYER ? Player.ZOMBIE_PLAYER : Player.PLANT_PLAYER;
        ControlManager.Instance.State = GameState.GAMEOVER;
    }
    public bool IsGameOver(){
        foreach(Slot slot in slots){
            if(slot.PlayerType == currentPlayer){
                return !slot.CanMoveNext(playerGrid);
            }
        }
        return false;
    }

    public void Attach(IObserver observer)
    {
        if (observers.Contains(observer)) return;
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
    public void MakeMove(Player playerType, Point point)
    {
        if(playerType != currentPlayer){
            throw new TurnPassException(AppContanst.WRONG_TURN_PASS);
        }
        if (GameWin(point.X, point.Y))
        {
            winner = playerType;
            ControlManager.Instance.State = GameState.GAMEOVER;
            return;
        }
        ChangeTurnPass();
        if(IsGameOver()){
            GameOver();
        }
    }

    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateNotify(this);
        }
    }

    public void AttachPlant(IPlant plantObserver)
    {
        if(plantObservers.Contains(plantObserver)) return;
        plantObservers.Add(plantObserver);
    }

    public void DetachPlant(IPlant plantObserver)
    {
        if(plantObservers.Contains(plantObserver)){
            plantObservers.Remove(plantObserver);
        }
    }

    public void AddEnergyForPlayer(Player player, float energy){
        foreach(Slot slot in slots){
            if(slot.PlayerType == player){
                slot.AddEnergy(energy);
            }
        }
    }

    public void PlantNotify()
    {
        List<IPlant> plantObserversCopy = new List<IPlant>(plantObservers);
        foreach(IPlant plantObserver in plantObserversCopy){
            plantObserver.AttackZombie();
        }
    }
}