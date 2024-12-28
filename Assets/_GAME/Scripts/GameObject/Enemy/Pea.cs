using System.Collections.Generic;
using System.Drawing;
using PlantsVsZombies.Enemy;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pea : Enemy, IPlant, IDirection
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private int distance = 1;
    [SerializeField] private LayerMask raycastLayerMask;
    private List<IGround> gridOnLight = new List<IGround>();
    public void AttackZombie()
    {
        foreach (Direction direction in gridCanMove)
        {
            RaycastHit hit;
            Vector2Int vector2Direction = Common.Direction[(int)direction];
            Vector3 vectorDirection = new Vector3(vector2Direction.x, 0, vector2Direction.y);
            Vector3 rayEndPoint = weapon.transform.position + vectorDirection * (distance * AppContanst.DISTANCE_CENTER_BETWEEN_TWO_GRID);
            Debug.DrawRay(weapon.transform.position, vectorDirection * (distance * AppContanst.DISTANCE_CENTER_BETWEEN_TWO_GRID), UnityEngine.Color.red, 4f);
            if (Physics.Raycast(weapon.transform.position, vectorDirection, out hit, distance * AppContanst.DISTANCE_CENTER_BETWEEN_TWO_GRID, raycastLayerMask))
            {
                Vector3 directionHit = vectorDirection.normalized;
                weapon.OnShoot(OnHitVictim, directionHit, rayEndPoint);
            }
        }
    }
    private void OnHitVictim(Enemy victim)
    {
        victim.OnBeAttack();
    }

    public override void OnBeAttack()
    {
        OnChangeAnimation(AppContanst.ANIMATION_ATTACK);
        GroundPlant.OnRemoveEnemy();
    }

    public override void OnDespawn()
    {
        GameStateManager.Instance.DetachPlant(this);
    }

    public override void OnInit()
    {
        OnChangeAnimation(AppContanst.ANIMATION_IDLE);
        base.OnInit();
        GameStateManager.Instance.AttachPlant(this);
    }

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        ShowDirection(GroundPlant);
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        foreach (IGround g in gridOnLight) g.ResetMeshMaterial();
        gridOnLight.Clear();
    }

    public void ShowDirection(IGround groundDirection)
    {
        foreach (IGround g in gridOnLight) g.ResetMeshMaterial();
        foreach (Direction i in GridMove)
        {
            Vector2Int direction = Common.Direction[(int)i];
            Point point = new Point(direction.x + groundDirection.GetColumnAndRow().X, direction.y + groundDirection.GetColumnAndRow().Y);
            if (point.X < 0 || point.Y < 0 || point.X >= GameStateManager.Instance.Size || point.Y >= GameStateManager.Instance.Size)
            {
                continue;
            }
            gridOnLight.Add(GameStateManager.Instance.PlayerGrid[point.X, point.Y]);
        }
        foreach (IGround ground in gridOnLight)
        {

            ground.OnLight();
        }
    }

    public override void UpdateNotify(ISubject subject)
    {
        if (TypePlayer != GameStateManager.Instance.CurrentPlayer)
        {
            subject.Detach(this);
            return;
        }
        base.UpdateNotify(subject);
    }

    public bool CanMoveOnThisTurnPass()
    {
        foreach (Direction i in GridMove)
        {
            Vector2Int direction = Common.Direction[(int)i];
            Point position = GroundPlant.GetColumnAndRow();
            Point point = new Point(position.X + direction.x, position.Y + direction.y);
            if (point.X < 0 || point.Y < 0 || point.X >= GameStateManager.Instance.Size || point.Y >= GameStateManager.Instance.Size)
            {
                continue;
            }
            else{
                Grid1x1 ground = GameStateManager.Instance.PlayerGrid[position.X, position.Y];
                if(ground.PlayerType() != TypePlayer){
                    return true;
                }
            }
        }
        return false;
    }
}