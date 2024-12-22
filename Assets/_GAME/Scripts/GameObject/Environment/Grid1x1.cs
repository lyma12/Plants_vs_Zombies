using System.Drawing;
using UnityEngine;
public class Grid1x1 : GameUnit, IGround
{
    [SerializeField] private Material normalMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform centerPlant;
    private Point columnAndRow;
    protected IPlantGrid currentEnemy;
    private bool isHaveEnemy = false;
    public void SetColumnAndRow(Point point)
    {
        columnAndRow = point;
    }

    public Point GetColumnAndRow()
    {
        return columnAndRow;
    }

    public void DeleteEnemy()
    {
        isHaveEnemy = false;
    }
    public void OnDragOn()
    {
        meshRenderer.material = DataManager.Instance.GridDragMaterial;
    }
    public void ResetMeshMaterial()
    {
        meshRenderer.material = normalMaterial;
        if(currentEnemy != null && currentEnemy is IGround){
            (currentEnemy as IGround).ResetMeshMaterial();
        }
    }
    public void OnSelect()
    {
        meshRenderer.material = DataManager.Instance.GridSlectMaterial;
    }
    public void OnLight()
    {
        meshRenderer.material = DataManager.Instance.GridLightMaterial;
        if(currentEnemy != null && currentEnemy is IGround){
            (currentEnemy as IGround).OnLight();
        }
    }

    public bool CanPlant(Enemy plantGrid)
    {
        return ((plantGrid is IPlantGrid) && !isHaveEnemy) || (isHaveEnemy && (currentEnemy is IGround) && (currentEnemy as IGround).CanPlant(plantGrid));
    }

    public void PlantEnemy(Enemy enemy)
    {
        if(isHaveEnemy && currentEnemy is IGround){
            (currentEnemy as IGround).PlantEnemy(enemy);
            return;
        }
        enemy = SimplePool.Spawn<Enemy>(enemy, centerPlant.position, Quaternion.identity);
        enemy.GroundPlant = this;
        enemy.transform.localPosition = enemy.transform.localPosition + new Vector3(0, enemy.Height / 2, 0);
        isHaveEnemy = true;
        if (enemy is IPlantGrid)
        {
            currentEnemy = enemy as IPlantGrid;
        }
    }
    public Player PlayerType()
    {
        if (isHaveEnemy)
        {
            return currentEnemy.PlayerOn();
        }
        return Player.NONE;
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {

    }

    public Transform GetCenterPoint()
    {
        return centerPlant;
    }

    public Enemy GetEnemyPlantOn()
    {
        if (currentEnemy != null)
        {
            return currentEnemy.GetEnemy();
        }
        return null;
    }

    public void OnChangeEnemy(Enemy enemy)
    {
        if(enemy == null){
            currentEnemy = null;
            isHaveEnemy = false;
            return;
        }
        if(CanPlant(enemy)){
            isHaveEnemy = true;
            enemy.GroundPlant.OnChangeEnemy(null);
            enemy.GroundPlant = this;
            enemy.transform.position = centerPlant.position;
            enemy.transform.localPosition = enemy.transform.localPosition + new Vector3(0, enemy.Height / 2, 0);
            if (enemy is IPlantGrid)
            {
                currentEnemy = enemy as IPlantGrid;
            }
        }
    }

    public void OnRemoveEnemy()
    {
        if (currentEnemy != null)
        {
            if(currentEnemy is IGround && currentEnemy.GetEnemy() != (currentEnemy as Enemy) && currentEnemy.GetEnemy() != null){
                (currentEnemy as IGround).OnRemoveEnemy();
                return;
            }
            SimplePool.Despawn(currentEnemy.GetEnemy());
            currentEnemy = null;
            isHaveEnemy = false;
        }
    }
}
