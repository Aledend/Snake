using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject settings;
    [SerializeField]
    DrawField drawField;
    [SerializeField]
    Data data;
    [SerializeField]
    SnakeController snakeController;
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    AStar aStar;
    public bool aStarActive = false;
    List<Vector2Int> path = new List<Vector2Int>();

    [System.NonSerialized]
    public bool aStarMoving = false, aStarReady = false, aStarPathed = false;

    public void GameStart()
    {
        foreach(Tile tile in data.tiles.Values)
        {
            tile.UnOccupy();
        }
        data.snakeList.Clear();
        data.snakeSpeed = 1;
        scoreText.text = "0";

        drawField.GameStart();
        snakeController.GameStart();
        SpawnPlayer();
        SpawnFruit();
    }

    private void Update()
    {
        if (aStarActive)
        {
            if (!aStarMoving)
            {
                path = new List<Vector2Int>(aStar.CalculatePath(data.snakeList[0].position));
                aStarMoving = true;
                if (path.Count < 2)
                    snakeController.SetMoveDirection(Vector2Int.up);
                else
                    snakeController.SetMoveDirection(path[1] - path[0]);
                aStarReady = true;
            }
        }

        
    }
    private void SpawnPlayer()
    {
        Vector2Int _pos = new Vector2Int(Random.Range(1, (int)drawField.fieldWidth - 1), Random.Range(1, (int)drawField.fieldHeight - 1));
        data.snakeList.Append(new SnakePart(_pos));
        data.tiles[_pos].Occupy();
    }

    public void SpawnFruit()
    {

        //Finds an empty spot, ends if the snake has filled the playfield
        List<Vector2Int> _tilePositionList = new List<Vector2Int>();
        foreach(Tile tile in data.tiles.Values)
        {
            if (!tile.occupied)
                _tilePositionList.Add(tile.position);
        }
        if(_tilePositionList.Count == 0)
        {
            WinGame();
            return;
        }

        //Places a fruit on one of the empty tiles.
        Vector2Int _pos = _tilePositionList[Random.Range(0, _tilePositionList.Count - 1)];
        data.tiles[_pos].AddFruit();
    }

    //Used by UI buttons to clear the board.
    public void ClearBoard()
    {
        foreach (Tile tile in data.tiles.Values)
        {
            tile.UnOccupy();
        }
    }

    //Used by UI buttons to activate the AI.
    public void ActivateAI()
    {
        aStarActive = true;
    }

    //Used by Unity buttons to deactivate the AI.
    public void DeactivateAI()
    {
        aStarActive = false;
    }

    //Called when playfield is filled, will cause the snake to collide, thus calling Game Over.
    private void WinGame()
    {
        aStarMoving = true;
        snakeController.SetMoveDirection(Vector2Int.up);
        aStarReady = true;
    }
}
