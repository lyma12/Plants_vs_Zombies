public class StateGame{
    private IGround[,] broad;
    private int energyBot;
    private int energyPlayer;
    private int size;
    public IGround[,] Broad{
        get{
            return broad;
        }
        set{
            broad = value;
        }
    }
    public int EnergyBot{
        get{
            return energyBot;
        }
        set{
            energyBot = value;
        }
    }
    public int EnergyPlayer{
        get{
            return energyPlayer;
        }
        set {
            energyPlayer = value;
        }
    }
    private Player botPlayerType;
    public Player BotPlayerType => botPlayerType;
    public int Size => size;
    public StateGame(IGround[,] broad, int energyBot, int energyPlayer, int size, Player playerBot){
        this.broad = broad;
        this.energyBot = energyBot;
        this.energyPlayer = energyPlayer;
        this.size = size;
        this.botPlayerType = playerBot;
    }
}