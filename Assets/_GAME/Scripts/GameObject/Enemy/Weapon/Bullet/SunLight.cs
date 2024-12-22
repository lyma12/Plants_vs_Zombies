using UnityEngine.EventSystems;

public class SunLight : Energy
{
    private Player player = Player.PLANT_PLAYER;
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
            GameStateManager.Instance.AddEnergyForPlayer(player, energy);
            SimplePool.Despawn(this);
        }
    }
}