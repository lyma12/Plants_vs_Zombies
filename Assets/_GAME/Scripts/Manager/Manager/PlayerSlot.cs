using System.Collections.Generic;
using UnityEngine;

public class PlayerSlot: Slot, IUISubject{
    private ListCard listCard;
    protected List<IUIObserver> uIObservers = new List<IUIObserver>();
    public PlayerSlot(Player playerType, Transform parentCardSpawn, Transform parentUI, bool isFirstStep, ListCard listCardPrefab): base(playerType, isFirstStep){
        if(listCardPrefab == null) return;
        listCard = SimplePool.Spawn<ListCard>(listCardPrefab, parentUI.position, Quaternion.identity);
        listCard.SetUp(playerType, parentCardSpawn, parentUI, this);
        if(isFirstStep){
            listCard.Able();
        }
        else{
            listCard.Enabled();
        }
        NotifyUI();
    }
    public override void AddEnergy(float energyCoin)
    {
        base.AddEnergy(energyCoin);
        NotifyUI();
    }
    public override void TurnPass(Player playerType){
        if(player == playerType){
            listCard.Able();
        }
        else{
            listCard.Enabled();
        }
    }
    public void AttachUI(IUIObserver uIObserver)
    {
        if(uIObservers.Contains(uIObserver)) return;
        uIObservers.Add(uIObserver);
    }

    public void DetachUI(IUIObserver uIObserver)
    {
       if(uIObservers.Contains(uIObserver)){
        uIObservers.Remove(uIObserver);
       }
    }
    public override void BuyEnemy(float energyCoin)
    {
        base.BuyEnemy(energyCoin);
        NotifyUI();
    }

    public void NotifyUI()
    {
        foreach(IUIObserver uIObserver in uIObservers){
            uIObserver.UpdateNotifyUI(this);
        }
    }
}