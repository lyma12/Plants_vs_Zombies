using System.Collections.Generic;
using UnityEngine;
public class Broad : GameUnit
{
    [SerializeField] List<Grid1x1> grid;
    private int size = -1;
    public int Size
    {
        get
        {
            if (size == -1){ 
                size = (int)Mathf.Sqrt(grid.Count); 
            }
            return size;
        }
    }
    public List<Grid1x1> Grid
    {
        get { return grid; }
    }
    public override void OnDespawn()
    {

    }

    public override void OnInit()
    {

    }
}