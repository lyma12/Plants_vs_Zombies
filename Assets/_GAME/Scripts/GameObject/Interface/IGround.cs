using System.Drawing;
using UnityEngine;
namespace MyNameSpace{
    public interface IGround{
    public bool CanPlant(Enemy plantGrid);
    public void OnDragOn();
    public void PlantEnemy(Enemy enemy);
    public void OnSelect();
    public void OnLight();
    public void ResetMeshMaterial();
    public Point GetColumnAndRow();
    public void SetColumnAndRow(Point point);
    public Transform GetCenterPoint();
    public Enemy GetEnemyPlantOn();
    public void OnChangeEnemy(Enemy enemy);
    public void OnRemoveEnemy();
    internal void CheckPlantEnemy(Enemy enemy);
}
}