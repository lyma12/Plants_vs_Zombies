using UnityEngine.EventSystems;

public class SunLight : Energy
{
    private Player player = Player.PLANT_PLAYER;
    public override Player PlayerType{
        get{
            return player;
        }
        set{
            player = value;
        }
    }

    public override void OnInit()
    {
        
    }

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        isSelect = true;
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        if (isSelect)
        {
            isSelect = false;
            SimplePool.Despawn(this);
        }
    }
}