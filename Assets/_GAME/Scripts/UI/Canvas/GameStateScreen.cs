using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateScreen : PointerEventSubject, IObserver
{
    [Header("UI")]
    [SerializeField] private List<ListCard> listCards;
    [SerializeField] private Transform transformParentListCard;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform parentCard;

    protected override void Awake()
    {
        base.Awake();
        StartPlay(GameType.PERSON_PERSON);
        //StartPlay(GameType.PERSON_BOT);
        ControlManager.Instance.Attach(this);
    }
    public void StartPlay(GameType gameType)
    {
        List<Slot> listPlayerSlot = new List<Slot>();
        Player firstPlayer = GameStateManager.Instance.FirstPlayer();
        switch (gameType)
        {
            case GameType.PERSON_PERSON:
                for (int i = 0; i < 2; i++)
                {
                    Slot playerSlot = new PlayerSlot((Player)i + 1, parentCard, transformParentListCard, firstPlayer == (Player)i + 1, listCards[i]);
                    listPlayerSlot.Add(playerSlot);
                }
                break;
            case GameType.PERSON_BOT:
                int botIndex = UnityEngine.Random.Range(0, 2);
                for (int i = 0; i < 2; i++)
                {
                    if (botIndex == i)
                    {
                        Slot slot = new BotSlot((Player)i + 1, firstPlayer == (Player)i + 1);
                        listPlayerSlot.Add(slot);
                    }
                    else
                    {
                        Slot playerSlot = new PlayerSlot((Player)i + 1, parentCard, transformParentListCard, firstPlayer == (Player)i + 1, listCards[i]);
                        listPlayerSlot.Add(playerSlot);
                    }
                }
                break;
        }
        GameStateManager.Instance.SetUpPlayerSlot(listPlayerSlot);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        Notify();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Notify();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Notify();
    }

    public void UpdateNotify(ISubject subject)
    {
        if (subject is ControlManager)
        {
            GameState state = ControlManager.Instance.State;
            switch (state)
            {
                case GameState.GAMEOVER:
                    CloseDirectly();
                    break;
            }
        }
    }
}