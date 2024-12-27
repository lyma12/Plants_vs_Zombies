using System;
using System.Collections.Generic;
using System.Drawing;
using PlantsVsZombies.Enemy;
using UnityEngine;
using UnityEngine.EventSystems;

public class HypnoMushroom : HypnosisEnemy, IPlant, IMoveWithDirection
{
    private List<IGround> gridOnLight = new List<IGround>();
    public void AttackZombie()
    {

    }

    public bool CanMoveWithDirection(IGround ground)
    {
        if (ground == GroundPlant) return true;
        Point groundOn = groundPlant.GetColumnAndRow();
        Vector2Int vector2 = new Vector2Int(ground.GetColumnAndRow().X, ground.GetColumnAndRow().Y) - new Vector2Int(groundOn.X, groundOn.Y);
        Direction direction = (Direction)Common.Direction.IndexOf(vector2);
        if (gridCanMove.Contains(direction))
        {
            if (ground.CanPlant(this))
            {
                return true;
            }
            if (GameStateManager.Instance.PlayerGrid[ground.GetColumnAndRow().X, ground.GetColumnAndRow().Y].PlayerType() != TypePlayer)
            {
                return true;
            }
        }
        return false;
    }

    public List<MoveType> GetPossibleMoves()
    {
        List<MoveType> moves = new List<MoveType>();
        foreach (Direction i in GridMove)
        {
            Vector2Int direction = Common.Direction[(int)i];
            Point point = new Point(GroundPlant.GetColumnAndRow().X + direction.x, GroundPlant.GetColumnAndRow().Y + direction.y);
            if (point.X < 0 || point.Y < 0 || point.X >= GameStateManager.Instance.Size || point.Y >= GameStateManager.Instance.Size)
            {
                continue;
            }
            MoveType move = new MoveChangeEnemy(GroundPlant.GetColumnAndRow(), point, this);
            moves.Add(move);
        }
        return moves;
    }

    public override void OnBeAttack()
    {

    }

    public override void OnDespawn()
    {

    }

    public void OnDrag(PointerEventData pointerEventData)
    {

    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        ShowDirection(GroundPlant);
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(pointerEventData.position);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100, layerMaskGround))
        {
            IGround ground = Cache.Instance.GetIGround(hitData.collider.gameObject);
            if (ground != null && gridOnLight.Contains(ground))
            {
                if (GameStateManager.Instance.PlayerGrid[ground.GetColumnAndRow().X, ground.GetColumnAndRow().Y].PlayerType() != TypePlayer && !ground.CanPlant(this))
                {
                    transform.position = GroundPlant.GetCenterPoint().position;
                    Enemy enemy = ground.GetEnemyPlantOn();
                    try
                        {
                            Hypnosis(enemy, TypePlayer);
                            Point point = GroundPlant.GetColumnAndRow();
                            GroundPlant.OnRemoveEnemy();
                            GameStateManager.Instance.MakeMove(TypePlayer, point);
                        }
                        catch (TurnPassException ex)
                        {
                            Debug.Log(ex.Message);
                        }
                }
            }
            else
            {
                transform.position = GroundPlant.GetCenterPoint().position;
            }
        }
        foreach (IGround ground in gridOnLight) ground.ResetMeshMaterial();
        gridOnLight.Clear();
    }

    public void ShowDirection(IGround groundDirection)
    {
        if (gridOnLight.Contains(groundDirection)) return;
        foreach (IGround ground in gridOnLight)
        {
            ground.ResetMeshMaterial();
        }
        gridOnLight.Clear();
        gridOnLight.Add(GroundPlant);
        foreach (Direction i in GridMove)
        {
            Vector2Int direction = Common.Direction[(int)i];
            Point point = new Point(groundDirection.GetColumnAndRow().X + direction.x, groundDirection.GetColumnAndRow().Y + direction.y);
            if (point.X < 0 || point.Y < 0 || point.X >= GameStateManager.Instance.Size || point.Y >= GameStateManager.Instance.Size)
            {
                continue;
            }
            gridOnLight.Add(GameStateManager.Instance.PlayerGrid[point.X, point.Y]);
        }
        foreach (IGround ground1 in gridOnLight)
        {
            Point point = new Point(ground1.GetColumnAndRow().X, ground1.GetColumnAndRow().Y);
            GameStateManager.Instance.PlayerGrid[point.X, point.Y].OnLight();
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
        if (subject is PointerEventSubject)
        {
            PointerEventSubject pointerSubject = subject as PointerEventSubject;
            switch (pointerSubject.PointerEvent)
            {
                case PointerEvent.PointerOnDrag:
                    OnDrag(pointerSubject.PointerEventData);
                    break;
                case PointerEvent.PointerUp:
                    subject.Detach(this);
                    break;
            }
        }
    }
}