using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AStar", menuName = "ScriptableObjects/AStar", order = 2)]
public class AStar : ScriptableObject
{
    [SerializeField]
    private Data data = null;
    [System.NonSerialized]
    private Dictionary<Vector2Int, AStarTile> tileList = new Dictionary<Vector2Int, AStarTile>();
    [System.NonSerialized]
    private Vector2Int currentPos = Vector2Int.zero;
    [System.NonSerialized]
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int sourcePos;

    //Try to find the nearest path to the fruit, takes in position of the snakehead
    public List<Vector2Int> CalculatePath(Vector2Int position)
    {
        //Clear previous calculation
        tileList.Clear();
        path.Clear();
        currentPos = position;
        sourcePos = position;
        //Marks the snakehead as origin
        AddInitialTile(position);
        //Finds position of fruit
        Vector2Int _target = FindFruit();
        //Calculate costs of neighboring tiles
        CheckNeighbors(position, _target);

        //Find the path
        while (currentPos != _target)
        {
            //Checks if there is any tile left to calculate, returns the lowest cost tile.
            if (FindNextTile(out AStarTile _tile))
            {
                CheckNeighbors(_tile.position, _target);
            }
            //If path to fruit cannot be found, reset the list and try to find a path to the tail
            else
            {
                tileList.Clear();
                currentPos = position;
                AddInitialTile(position);
                CheckNeighbors(position, data.snakeList.Last.position);
                while (currentPos != data.snakeList.Last.position)
                {
                    if (FindNextTile(out _tile))
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
        }

        //Takes the last tile and tracks back to the snake
        AStarTile _currentTile = tileList[currentPos];
        while (_currentTile.previousTile != null)
        {
            path.Add(_currentTile.position);
            _currentTile = _currentTile.previousTile;
        }
        path.Add(position);
        path.Reverse();
        
        //Return the per-tile path that the snake is to take
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

    //Update costs of neihboring tiles
    private void CheckNeighbors(Vector2Int position, Vector2Int targetPosition)
    {
        UpdateTile(position, position + Vector2Int.up, targetPosition);
        UpdateTile(position, position + Vector2Int.down, targetPosition);
        UpdateTile(position, position + Vector2Int.right, targetPosition);
        UpdateTile(position, position + Vector2Int.left, targetPosition);
    }

    private void UpdateTile(Vector2Int currentPosition, Vector2Int newPosition, Vector2Int targetPosition)
    {
        //Checks wether tile already has been calculated and added to the dictionary
        if (!tileList.TryGetValue(newPosition, out AStarTile _tile))
        {
            _tile = new AStarTile(newPosition);
            tileList.Add(newPosition, _tile);
        }
        else
        {
            //If the tile has been calculated, check wether this would be a cheaper alternative.
            if (tileList[currentPosition].DistanceFromSource() + 1 + _tile.HCost > _tile.FCost)
                return;
        }

        if(newPosition.x == 0 || newPosition.x == data.fieldWidth || newPosition.y == 0 || newPosition.y == data.fieldHeight)
        {
            _tile.explored = true;
            return;
        }
        else if (data.tiles[newPosition].occupied && !data.tiles[newPosition].isFruit)
        {
            _tile.explored = true;
            return;
        }
        
        //Makes sure that the tile isn't the source tile to prevent a chain loop when looping through previousTile
        else if(_tile.position != sourcePos)
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

    private bool FindNextTile(out AStarTile _tile)
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

        //Return false if no unexplored tile could be found
        if (_tile.position == Vector2Int.zero)
            return false;
        _tile.explored = true;
        currentPos = _tile.position;
        return true;
    }
}
