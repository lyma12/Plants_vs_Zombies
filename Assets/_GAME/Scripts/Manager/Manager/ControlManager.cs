using System.Collections.Generic;

public class ControlManager : Singleton<ControlManager>, ISubject
{
    private GameState state;
    private List<IObserver> observers = new List<IObserver>();
    private void Awake() {
        state = GameState.NONE;
    }

    public GameState State{
        get{
            return state;
        }
        set{
            if(state == value){
                return;
            }
            switch(value){
                case GameState.MAINMENU:
                OnMainMenu();
                break;
                case GameState.GAMESTART:
                OnGameStart();
                break;
                case GameState.GAMEOVER:
                OnGameOver();
                break;
            }
        }
    }
    private void OnMainMenu(){
        state = GameState.MAINMENU;
    }
    private void OnGameOver(){
        state = GameState.GAMEOVER;
        Notify();
    }
    private void OnGameStart(){
        state = GameState.GAMESTART;
        Notify();
    }
    public void Attach(IObserver observer)
    {
        if(observers.Contains(observer)) return;
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        if(observers.Contains(observer)){
            observers.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach(IObserver observer in observers){
            observer.UpdateNotify(this);
        }
    }
}