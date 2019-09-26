using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTile
{
    public int FCost;
    public int GCost;
    public int HCost;
    public bool explored = false;

    public Vector2Int position;

    public AStarTile previousTile;

    public AStarTile(Vector2Int position)
    {
        this.position = position;
    }

    public int DistanceFromSource()
    {
        int _count = 0;
        AStarTile _currentTile = this;

        while(_currentTile.previousTile != null)
        {
            _count++;
            _currentTile = _currentTile.previousTile;
            if (_currentTile.previousTile == this)
                throw new System.Exception("Stuck in loop");
        }

        return _count;
    }
}
