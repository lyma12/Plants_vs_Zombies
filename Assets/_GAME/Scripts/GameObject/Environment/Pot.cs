using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pot : Enemy, IGround, IPlantGrid
{
    [SerializeField] private Material normalMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform centerPlant;

    private Enemy currentPlant;
    private bool isHaveEnemy = false;
    private Point columnAndRown;
    public override IGround GroundPlant
    {
        get => base.GroundPlant;
        set
        {
            base.GroundPlant = value;
            columnAndRown = value.GetColumnAndRow();
        }
    }

    public bool CanPlant(Enemy plantGrid)
    {
        return (plantGrid is IPlant) && !isHaveEnemy;
    }

    public override void OnBeAttack()
    {
        GroundPlant.OnRemoveEnemy();
    }

    public void OnDelete()
    {
        SimplePool.Despawn(this);
        SimplePool.Despawn(currentPlant);
        currentPlant = null;
        isHaveEnemy = false;
    }

    public override void OnDespawn()
    {

    }

    public void OnDragOn()
    {
        meshRenderer.material = DataManager.Instance.GridDragMaterial;
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public void OnLight()
    {
        meshRenderer.material = DataManager.Instance.GridLightMaterial;
    }

    public Player PlayerOn()
    {
        if(isHaveEnemy){
            return currentPlant.TypePlayer;
        }
        return Player.NONE;
    }

    public void OnSelect()
    {
        meshRenderer.material = DataManager.Instance.GridSlectMaterial;
    }

    public void PlantEnemy(Enemy enemy)
    {
        currentPlant = SimplePool.Spawn<Enemy>(enemy, centerPlant.position, Quaternion.identity);
        currentPlant.GroundPlant = this;
        currentPlant.transform.localPosition = currentPlant.transform.localPosition + new Vector3(0, currentPlant.Height / 2, 0);
        currentPlant.transform.SetParent(transform);
        isHaveEnemy = true;
    }

    public void ResetMeshMaterial()
    {
        meshRenderer.material = normalMaterial;
    }

    public Point GetColumnAndRow()
    {
        return columnAndRown;
    }

    public void SetColumnAndRow(Point point)
    {
        columnAndRown = point;
    }

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        OnSelect();
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        ResetMeshMaterial();
    }

    public Transform GetCenterPoint()
    {
        return centerPlant;
    }

    public Enemy GetEnemy()
    {
        if (currentPlant == null) return this;
        else return currentPlant;
    }

    public Enemy GetEnemyPlantOn()
    {
        if (currentPlant == null) return this;
        else return currentPlant;
    }

    public void OnChangeEnemy(Enemy enemy)
    {
        if (enemy != currentPlant && CanPlant(enemy))
        {
            isHaveEnemy = true;
            currentPlant = enemy;
            enemy.GroundPlant = this;
            currentPlant.transform.localPosition = currentPlant.transform.localPosition + new Vector3(0, currentPlant.Height / 2, 0);
            currentPlant.transform.SetParent(transform);
            enemy.transform.position = GetCenterPoint().position;
        }
    }

    public void OnRemoveEnemy()
    {
        if (currentPlant != null)
        {
            if(currentPlant is IGround){
                (currentPlant as IGround).OnRemoveEnemy();
                return;
            }
            SimplePool.Despawn(currentPlant);
            currentPlant = null;
            isHaveEnemy = false;
        }
    }
}