using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
[System.Serializable]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string nameEnemy;
    [SerializeField] private float priceEnemy;
    [SerializeField] private Enemy enemy;

    public string NameEnemy
    {
        get
        {
            return nameEnemy;
        }
    }
    public float PriceEnemy
    {
        get
        {
            return priceEnemy;
        }
    }
    public Sprite Image
    {
        get
        {
            return image;
        }
    }
    public Enemy Enemy{
        get {
            return enemy;
        }
    }
}