using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private Material gridDragMaterial;
    [SerializeField] private Material gridSelectMaterial;
    [SerializeField] private Material gridLightMaterial;
    [SerializeField] private List<ListEnemyData> listEnemyData;

    public Material GridDragMaterial { get { return gridDragMaterial; } }
    public Material GridSlectMaterial { get { return gridSelectMaterial; } }
    public Material GridLightMaterial { get { return gridLightMaterial; } }

    public List<EnemyData> GetEnemyData(Player type){
        if(type == Player.NONE) return null;
        return listEnemyData[(int) (type - 1)].getData();
    }

}