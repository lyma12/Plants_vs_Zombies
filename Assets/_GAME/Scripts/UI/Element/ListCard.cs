using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ListCard : GameUnit, IUIObserver
{
    [SerializeField] private Player type;
    [SerializeField] private Transform scrollView;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CardInShop cardInShopPrefab;
    [SerializeField] private TMP_Text coinText;

    private List<EnemyData> data;
    private List<CardInShop> listCard = new List<CardInShop>();
    public Player Type{
        get{ return type;}
        set{ type = value;}
    }
    public void SetUp(IUIObserver uIObserver, Player playerType, Transform parentUI, IUISubject slotController){
        type = playerType;
        if(data == null) data = DataManager.Instance.GetEnemyData(type);
        foreach(EnemyData enemy in data){
            CardInShop cardInShop = SimplePool.Spawn<CardInShop>(cardInShopPrefab, transform.position, Quaternion.identity);
            cardInShop.Data = enemy;
            cardInShop.AttachUI(uIObserver);
            cardInShop.transform.SetParent(scrollView, false);
            slotController.AttachUI(cardInShop);
            listCard.Add(cardInShop);
        }
        slotController.AttachUI(this);
        transform.SetParent(parentUI);
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.offsetMin = new Vector2(0, 0);
    }
    public void Enabled(){
        foreach(CardInShop cardInShop in listCard){
            cardInShop.IsEnable = false;
        }
    }
    public void Able(){
        foreach(CardInShop cardInShop in listCard){
            cardInShop.IsEnable = true;
        }
    }
    public override void OnDespawn()
    {
        
    }

    public override void OnInit()
    {
    }

    public void UpdateNotifyUI(IUISubject uISubject)
    {
        Debug.Log(uISubject);
        if(uISubject is Slot){
            coinText.text = $"{AppContanst.COIN}: {(uISubject as Slot).EnergyCoin}";
        }
    }
    
}