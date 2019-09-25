using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    Data data;
    [SerializeField]
    private GameObject gameOver, inGame;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Text scoreText, gameOverScore;

    private string moveDirection = "";
    float tick = 0;

    public void GameStart()
    {
        moveDirection = "";
        tick = 0;
        enabled = true;
    }

    void FixedUpdate()
    {
        if(tick > 1)
        {
            Move();
            tick = 0;
        }
        tick += data.snakeSpeed / 10f;
    }
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(moveDirection == "right")
            {
                moveDirection = "right";
            }
            else
            {
                moveDirection = "left";
                tick = 2;
                FixedUpdate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveDirection == "left")
            {
                moveDirection = "left";
            }
            else
            {
                moveDirection = "right";
                tick = 2;
                FixedUpdate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (moveDirection == "down")
            {
                moveDirection = "down";
            }
            else
            {
                moveDirection = "up";
                tick = 2;
                FixedUpdate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = moveDirection == "up" ? "up" : "down";
            if (moveDirection == "up")
            {
                moveDirection = "up";
            }
            else
            {
                moveDirection = "down";
                tick = 2;
                FixedUpdate();
            }
        }
    }

    private void Move()
    {
        switch (moveDirection)
        {
            case "left":
                Grow(Vector2Int.left);
                break;
            case "right":
                Grow(Vector2Int.right);
                break;
            case "up":
                Grow(Vector2Int.up);
                break;
            case "down":
                Grow(Vector2Int.down);
                break;
        }
    }

    private void Grow(Vector2Int v)
    {
        Vector2Int _newPos = data.snakeList[0].position + v;
        if(data.tiles[_newPos].isFruit)
        {
            data.fruitEaten++;
            data.snakeSpeed *= 1.1f;
            data.tiles[_newPos].isFruit = false;
            data.tiles[_newPos].Occupy();
            gameManager.SpawnFruit();
            scoreText.text = (Convert.ToInt32(scoreText.text) + 1).ToString();
        }
        else if(data.tiles[_newPos].occupied)
        {
            gameOver.SetActive(true);
            enabled = false;
            gameOverScore.text = scoreText.text;
        }
        else
        {
            data.tiles[data.snakeList.Last.position].UnOccupy();
            data.snakeList.RemoveLast();
            data.tiles[_newPos].Occupy();
        }
        data.snakeList.Push(new SnakePart(_newPos));

    }
}
