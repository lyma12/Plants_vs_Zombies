using System.Drawing;
using UnityEngine;

public class FakeGrid : IGround
{
    private Point columnAndRow;
    protected IPlantGrid currentEnemy;
    private bool isHaveEnemy = false;
    public FakeGrid(Grid1x1 grid1X1){
        this.columnAndRow = grid1X1.GetColumnAndRow();
        this.currentEnemy = grid1X1.GetEnemyPlantOn()?.Clone() as IPlantGrid;
        if(currentEnemy != null){
            (currentEnemy as Enemy).GroundPlant = this;
        }
        this.isHaveEnemy = currentEnemy != null;
    }
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
    }
    public void ResetMeshMaterial()
    {
    }
    public void OnSelect()
    {
    }
    public void OnLight()
    {
    }

    public bool CanPlant(Enemy plantGrid)
    {
        return ((plantGrid is IPlantGrid) && !isHaveEnemy) || (isHaveEnemy && (currentEnemy is IGround) && (currentEnemy as IGround).CanPlant(plantGrid));
    }

    public void PlantEnemy(Enemy enemy)
    {
        if(isHaveEnemy && currentEnemy is IGround){
            (currentEnemy as IGround).PlantEnemy(enemy);
        }
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
            currentEnemy = null;
            isHaveEnemy = false;
        }
    }
    public Transform GetCenterPoint()
    {
        return null;
    }

}