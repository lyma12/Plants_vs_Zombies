using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListEnemy", menuName = "ScriptableObjects/ListEnemy", order = 1)]
[System.Serializable]
public class ListEnemyData : ScriptableObject
{
    [SerializeField] private List<EnemyData> listData = new List<EnemyData>();

    public List<EnemyData> getData(){
        return listData;
    }
}