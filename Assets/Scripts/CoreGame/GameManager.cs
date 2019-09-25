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


    private void SpawnPlayer()
    {
        Vector2Int _pos = new Vector2Int(Random.Range(1, (int)drawField.fieldWidth - 1), Random.Range(1, (int)drawField.fieldHeight - 1));
        data.snakeList.Append(new SnakePart(_pos));
        data.tiles[_pos].Occupy();
    }

    public void SpawnFruit()
    {
        while (true)
        {
            Vector2Int pos = new Vector2Int(Random.Range(1, (int)drawField.fieldWidth-1), Random.Range(1, (int)drawField.fieldHeight-1));

            if (!data.tiles[pos].occupied)
            {
                data.tiles[pos].AddFruit();
                break;
            }
        }
    }

    public void ClearBoard()
    {
        foreach (Tile tile in data.tiles.Values)
        {
            tile.UnOccupy();
        }
    }
}
