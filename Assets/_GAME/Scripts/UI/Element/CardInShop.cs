using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardInShop : GameUnit, IPointerDownHandler, IUIObserver
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameCard;
    [SerializeField] private TMP_Text priceCard;
    [SerializeField] private GameObject canBuyUI;
    [SerializeField] private Card cardPrefab;
    private Transform parentCard;
    private EnemyData data;
    private bool isEnable = true;
    private bool canBuy = false;
    public Transform ParentCard{set{ parentCard = value;}}
    public bool CanBuy{
        get{
            return canBuy;
        }
        private set{
            canBuy = value;
            canBuyUI.SetActive(!value);
        }
    }
    public bool IsEnable{
        get{
            return isEnable;
        }
        set{
            isEnable = value;
        }
    }
    public EnemyData Data
    {
        get { return data; }
        set
        {
            data = value;
            UpdateSprite();
            UpdatePrice();
            UpdateName();
        }
    }

    public override void OnDespawn()
    {
        
    }

    public override void OnInit()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isEnable || !canBuy) return;
        Card card = SimplePool.Spawn<Card>(cardPrefab, transform.position, Quaternion.identity);
        card.Data = Data;
        card.transform.SetParent(parentCard);
    }

    public void UpdateNotifyUI(IUISubject uISubject)
    {
        if(uISubject is PlayerSlot){
            CanBuy = (uISubject as Slot).CanBuyEnemy(data.PriceEnemy);
        }
    }

    private void UpdateName(){
        if(data != null && nameCard != null){
            nameCard.text = data.name;
        }
    }
    private void UpdatePrice(){
        if(data != null && priceCard != null){
            priceCard.text = Common.PriceCardConvert(data.PriceEnemy);
        }
    }
    private void UpdateSprite()
    {
        if (data != null && image != null)
        {
            image.sprite = data.Image;
        }
    }
}