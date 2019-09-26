using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AStar", menuName = "ScriptableObjects/AStar", order = 2)]
public class AStar : ScriptableObject
{
    [SerializeField]
    private Data data;
    [System.NonSerialized]
    private Dictionary<Vector2Int, AStarTile> tileList = new Dictionary<Vector2Int, AStarTile>();
    [System.NonSerialized]
    private Vector2Int currentPos = Vector2Int.zero;
    [System.NonSerialized]
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int sourcePos;

    public List<Vector2Int> CalculatePath(Vector2Int position)
    {
        tileList.Clear();
        path.Clear();
        currentPos = position;
        sourcePos = position;
        AddInitialTile(position);
        Vector2Int _target = FindFruit();
        CheckNeighbors(position, _target);

        int _count = 0;




        (currentPos != _target)
        {
            AStarTile _tile;
            if (findNextTile(out _tile))
            {
                CheckNeighbors(_tile.position, _target);
            }
            else
            {
                tileList.Clear();
                currentPos = position;
                AddInitialTile(position);
                CheckNeighbors(position, data.snakeList.Last.position);
                while(currentPos != data.snakeList.Last.position)
                {
                    if(findNextTile(out _tile))
                    {
                        CheckNeighbors(_tile.position, data.snakeList.Last.position);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            }
            _count++;
            if(_count > 1000)
            {

            }
        }

        AStarTile _currentTile = tileList[currentPos];
        while (_currentTile.previousTile != null)
        {
            path.Add(_currentTile.position);
            _currentTile = _currentTile.previousTile;
            if (_currentTile.previousTile == tileList[currentPos])
                throw new System.Exception("Stuck in loop");
        }
        path.Add(position);
        path.Reverse();
        
        return path;
    }

    private Vector2Int FindFruit()
    {
        foreach(Tile tile in data.tiles.Values)
        {
            if (tile.isFruit)
                return tile.position;
        }

        throw new System.NullReferenceException("Fruit not found");
    }


    private void CheckNeighbors(Vector2Int position, Vector2Int targetPosition)
    {
        UpdateTile(position, position + Vector2Int.up, targetPosition);
        UpdateTile(position, position + Vector2Int.down, targetPosition);
        UpdateTile(position, position + Vector2Int.right, targetPosition);
        UpdateTile(position, position + Vector2Int.left, targetPosition);
    }

    private void UpdateTile(Vector2Int currentPosition, Vector2Int newPosition, Vector2Int targetPosition)
    {
        AStarTile _tile;
        if (!tileList.TryGetValue(newPosition, out _tile))
        {
            _tile = new AStarTile(newPosition);
            tileList.Add(newPosition, _tile);
        }
        else
        {
            if (tileList[currentPosition].DistanceFromSource() + 1 + _tile.HCost > _tile.FCost)
                return;
        }
        try
        {
            if (data.tiles[newPosition].occupied && !data.tiles[newPosition].isFruit)
            {
                _tile.explored = true;
                return;
            }
        }
        catch
        {
            Debug.Log("Couldn't read data.tiles with pos: " + newPosition);
        }
        
        if(_tile.position != sourcePos)
        {
            _tile.previousTile = tileList[currentPosition];
            _tile.GCost = _tile.DistanceFromSource();
            _tile.HCost = Mathf.Abs(newPosition.x - targetPosition.x) + Mathf.Abs(newPosition.y - targetPosition.y);
            _tile.FCost = _tile.GCost + _tile.HCost;
        }
            
    }

    private void AddInitialTile(Vector2Int position)
    {
        AStarTile _tile = new AStarTile(position);
        tileList.Add(position, _tile);
        _tile.explored = true;
        currentPos = position;
    }

    private bool findNextTile(out AStarTile _tile)
    {
        _tile = new AStarTile(Vector2Int.zero);
        int _FCost = int.MaxValue;
        foreach (AStarTile tile in tileList.Values)
        {
            if (!tile.explored && tile.FCost < _FCost)
            {
                _tile = tile;
                _FCost = tile.FCost;
            }
        }

        if (_tile.position == Vector2Int.zero)
            return false;
        _tile.explored = true;
        currentPos = _tile.position;
        return true;
    }
}
