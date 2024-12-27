using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : GameUnit, IDirection, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameEnemy;
    [SerializeField] private TMP_Text priceEnemy;
    [SerializeField] private LayerMask groundLayerMask;
    private bool isSelect = false;
    private IGround groundSelect;
    private IGround groundOnDrag;
    private IGround currendGround;

    private EnemyData data;
    private List<IGround> gridOnLight = new List<IGround>();
    public EnemyData Data
    {
        get { return data; }
        set
        {
            data = value;
            image.sprite = data.Image;
            nameEnemy.text = data.NameEnemy;
            priceEnemy.text = Common.PriceCardConvert(data.PriceEnemy);
        }
    }

    public bool IsSelect()
    {
        return isSelect;
    }

    public override void OnDespawn()
    {

    }

    private void ResetMaterialGrid()
    {
        groundSelect.ResetMeshMaterial();
        try
        {
            groundSelect.PlantEnemy(data.Enemy);
            GameStateManager.Instance.BuyEnemy(data.PriceEnemy);
            Player typePlayer = data.Enemy.TypePlayer;
            if(typePlayer == Player.NONE){
                if(data.Enemy is Pot){
                    typePlayer = Player.PLANT_PLAYER;
                }
                else{
                    typePlayer = Player.ZOMBIE_PLAYER;
                }
            }
            GameStateManager.Instance.MakeMove(typePlayer, groundSelect.GetColumnAndRow());
        }
        catch (TurnPassException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public override void OnInit()
    {
        groundSelect = null;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (isSelect)
        {
            transform.position = pointerEventData.position;
            Ray ray = Camera.main.ScreenPointToRay(pointerEventData.position);
            if (Physics.Raycast(ray, out RaycastHit hitData, 100, groundLayerMask))
            {
                IGround ground = Cache.Instance.GetIGround(hitData.collider.gameObject);
                if (groundOnDrag != null && (groundOnDrag != ground || ground == null))
                {
                    groundOnDrag.ResetMeshMaterial();
                }
                if (ground != null && ground.CanPlant(data.Enemy))
                {
                    ground.OnDragOn();
                    ShowDirection(ground);
                    groundOnDrag = ground;
                }
                else
                {
                    ShowDirection(null);
                }
            }
            else
            {
                ShowDirection(null);
            }
        }
    }

    public void ShowDirection(IGround ground)
    {
        if (ground == currendGround) return;
        foreach (IGround g in gridOnLight) g.ResetMeshMaterial();
        currendGround = ground;
        if (ground == null) return;
        foreach (Direction i in data.Enemy.GridMove)
        {
            Vector2Int direction = Common.Direction[(int)i];
            Point point = new Point(direction.x + ground.GetColumnAndRow().X, direction.y + ground.GetColumnAndRow().Y);
            if (point.X < 0 || point.Y < 0 || point.X >= GameStateManager.Instance.Size || point.Y >= GameStateManager.Instance.Size)
            {
                continue;
            }
            gridOnLight.Add(GameStateManager.Instance.PlayerGrid[point.X, point.Y]);
            GameStateManager.Instance.PlayerGrid[point.X, point.Y].OnLight();
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isSelect = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!isSelect) return;
        isSelect = false;
        Ray ray = Camera.main.ScreenPointToRay(pointerEventData.position);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100, groundLayerMask))
        {
            IGround gridOnDrag = Cache.Instance.GetIGround(hitData.collider.gameObject);
            if (gridOnDrag.CanPlant(data.Enemy))
            {
                gridOnDrag.OnSelect();
                groundSelect = gridOnDrag;
                foreach (IGround ground in gridOnLight)
                {
                    ground.ResetMeshMaterial();
                }
                gridOnLight.Clear();
                Invoke(nameof(ResetMaterialGrid), 0.5f);
            }
        }
        SimplePool.Despawn(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isSelect) return;
        SimplePool.Despawn(this);
    }
}