using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BotSlot : Slot
{
    private List<EnemyData> BotData = new List<EnemyData>();
    private List<EnemyData> PlayerData = new List<EnemyData>();
    public BotSlot(Player playerType, bool isFirstStep) : base(playerType, isFirstStep)
    {
        Player p = playerType == Player.PLANT_PLAYER ? Player.ZOMBIE_PLAYER : Player.PLANT_PLAYER;
        BotData = DataManager.Instance.GetEnemyData(playerType);
        PlayerData = DataManager.Instance.GetEnemyData(p);
    }

    public override void TurnPass(Player playerType)
    {
        // if(player == playerType){
        //     int size = GameStateManager.Instance.Size;
        //     FakeGrid[,] broad = new FakeGrid[size, size];
        //     for(int i = 0; i < size * size; i++){
        //         int c = i % 3;
        //         int r = i / 3;
        //         broad[c, r] = new FakeGrid(GameStateManager.Instance.PlayerGrid[c, r]);
        //     }
        //     StateGame state = new StateGame(broad, (int)EnergyCoin, 10, GameStateManager.Instance.Size, playerType);
        //     MoveType move = Minimax(state, 2, playerType, null).Item2;
        //     move.MakeMove(GameStateManager.Instance.PlayerGrid, this);
        //     Debug.Log($"Minimax value when bot is {player}: {move}");
        // }
    }

    private (int, MoveType) Minimax(StateGame state, int depth, Player checkPlayer, MoveType moveType){
        if(depth == 0){
            return (EvaluateBoard(state.Broad, state.EnergyBot, moveType.Dir.X, moveType.Dir.Y, state.Size, checkPlayer), moveType);
        }
        if(checkPlayer == player){
            int maxEvel = int.MinValue;
            MoveType moveBest = null;
            Player other = checkPlayer == Player.PLANT_PLAYER ? Player.ZOMBIE_PLAYER : Player.PLANT_PLAYER;
            foreach(MoveType move in GetPossibleMoves(state.Broad, checkPlayer, state.EnergyBot, state.Size, BotData)){
                StateGame newState = move.MakeMove(state);
                int eval = Minimax(newState, depth - 1, other, move).Item1;
                if(eval > maxEvel){
                    maxEvel = eval;
                    moveBest = move;
                }
            }
            return (maxEvel, moveBest);
        }
        else{
            int minEval = int.MaxValue;
            MoveType moveBest = null;
            Player other = checkPlayer == Player.PLANT_PLAYER ? Player.ZOMBIE_PLAYER : Player.PLANT_PLAYER;
            foreach(MoveType move in GetPossibleMoves(state.Broad, checkPlayer, state.EnergyPlayer, state.Size, PlayerData)){
                StateGame newState = move.MakeMove(state);
                int eval = Minimax(newState, depth - 1, other, move).Item1;
                if(minEval > eval){
                    minEval = eval;
                    moveBest = move;
                }
            }
            return (minEval, moveBest);
        }
    }

    private List<MoveType> GetPossibleMoves(IGround[,] broad, Player checkPlayer, int energyState, int size, List<EnemyData> data){
        List<EnemyData> enemyPossible = data.Where(enemy => enemy.PriceEnemy <= energyState).ToList();
        List<MoveType> result = new List<MoveType>();
        for(int i = 0; i < size * size; i++){
            int c = i % 3;
            int r = i / 3;
            if(broad[c, r].GetEnemyPlantOn()?.TypePlayer == checkPlayer){
                Enemy enemyPlantOn = (broad[c, r] as IGround).GetEnemyPlantOn();
                if(enemyPlantOn is IMoveWithDirection){
                    result.AddRange((enemyPlantOn as IMoveWithDirection).GetPossibleMoves());
                }
            }else{
                foreach(EnemyData enemyData in enemyPossible){
                    if(broad[c, r].CanPlant(enemyData.Enemy)){
                        MoveType moveType = new MoveAddEnemy(new System.Drawing.Point(c, r), enemyData.Enemy,(int) enemyData.PriceEnemy, checkPlayer);
                        result.Add(moveType);
                    }
                }
            }
        }
        return result;
    }

    private int EvaluateBoard(IGround[,] broad, int energyState, int c, int r, int size, Player checkPlayer){
        int isNum = broad[r, c].GetEnemyPlantOn()?.TypePlayer == checkPlayer ? 1 : 0;
        int row = CountSymbolWithDirection(Vector2Int.left, r - 1, c, size, broad, checkPlayer) + CountSymbolWithDirection(Vector2Int.right, r + 1, c, size, broad, checkPlayer) + isNum;
        int column = CountSymbolWithDirection(Vector2Int.down, r, c - 1, size, broad, checkPlayer) + CountSymbolWithDirection(Vector2Int.up, r, c + 1, size, broad, checkPlayer) + isNum;
        int primaryDiagonal = CountSymbolWithDirection(new Vector2Int( -1, 1), r - 1, c + 1, size, broad, checkPlayer) + CountSymbolWithDirection(new Vector2Int(1, 1), r + 1, c + 1, size, broad, checkPlayer) + isNum;
        int secondDiagonal = CountSymbolWithDirection(new Vector2Int(-1, -1), r - 1 , c - 1, size, broad, checkPlayer) + CountSymbolWithDirection(new Vector2Int(-1, 1), r -1 , c + 1, size, broad, checkPlayer) + isNum;
        bool isWin = size <= row || size <= column || size <= primaryDiagonal || size <= secondDiagonal;
        if(player == checkPlayer){
            if(isWin) return energyState + 100;
        }
        else{
            if(isWin) return -100;
        }
        return energyState;
    }
    private int CountSymbolWithDirection(Vector2Int direction, int r, int c, int size, IGround[,] broad, Player checkPlayer){
        if(r < 0 || c < 0 || r > size - 1 || c > size - 1) return 0;
        if(broad[r, c].GetEnemyPlantOn() == null) return 0;
        if(broad[r, c].GetEnemyPlantOn().TypePlayer != checkPlayer) return 0;
        return 1 + CountSymbolWithDirection(direction, r + direction.x, c + direction.y, size, broad, checkPlayer);
    }
}