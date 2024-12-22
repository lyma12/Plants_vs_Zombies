using System.Collections;
using System.Drawing;
using UnityEngine;

public class Imp : Zombie
{
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask raycastLayerMask;
    public override void OnInit()
    {
        base.OnInit();
        StartCoroutine(FirstInit());
    }
    private IEnumerator FirstInit()
    {
        yield return new WaitForSeconds(1f);
        Vector2Int vector2Direction = Vector2Int.down;
        Point point = GroundPlant.GetColumnAndRow();
        point.X += vector2Direction.x;
        point.Y += vector2Direction.y;
        if (point.X >= 0 && point.X < GameStateManager.Instance.Size && point.Y >= 0 && point.Y < GameStateManager.Instance.Size)
        {
            Grid1x1 nextGround = GameStateManager.Instance.PlayerGrid[point.X, point.Y];
            if (nextGround.PlayerType() != TypePlayer && !nextGround.CanPlant(this))
            {
                transform.position = GroundPlant.GetCenterPoint().position;
                Eat(nextGround.GetEnemyPlantOn(), nextGround);
            }
        }
    }
    public override void Eat(Enemy plant, IGround ground)
    {
        plant.OnBeAttack();
        ground.OnChangeEnemy(this);
    }
}