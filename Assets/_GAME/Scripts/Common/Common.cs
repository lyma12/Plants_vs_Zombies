using System.Collections.Generic;
using UnityEngine;

public class Common{
    public static string PriceCardConvert(float price){
        return $"{price} {AppContanst.SUNFLOWER}";
    }
    public static List<Vector2Int> Direction = new List<Vector2Int>(){
        Vector2Int.zero,
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
    };
}