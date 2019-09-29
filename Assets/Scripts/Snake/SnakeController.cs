using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    private Data data = null;
    [SerializeField]
    private GameObject gameOver = null;
    [SerializeField]
    private GameManager gameManager = null;
    [SerializeField]
    private Text scoreText = null;

    Stopwatch sw = new Stopwatch();
    private string moveDirection = "";
    private string lastDirection = "";
    float tick = 0;

    public void GameStart()
    {
        moveDirection = "";
        tick = 0;
        enabled = true;
        sw.Start();
    }

    void FixedUpdate()
    {
        //Moves the snake at a set interval
        if(tick > 10)
        {
            if (!gameManager.aStarActive)
                Move();
            else if (gameManager.aStarReady)
            {
                Move();
                gameManager.aStarReady = false;
            }
            tick = 0;
        }
        tick += data.snakeSpeed;
    }

    void Update()
    {
        if(!gameManager.aStarActive)
            UpdateMovement();
    }

    private void UpdateMovement()
    {
        //Prevents player from making immediate 180 turns, calling FixedUpdate on input allows for more interactive and precise movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(lastDirection == "right")
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
            if (lastDirection == "left")
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
            if (lastDirection == "down")
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
            if (lastDirection == "up")
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
        lastDirection = moveDirection;
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
        gameManager.aStarMoving = false;
    }

    public void SetMoveDirection(Vector2Int v)
    {
        if (v == Vector2Int.left)
            moveDirection = "left";
        else if (v == Vector2Int.right)
            moveDirection = "right";
        else if (v == Vector2Int.up)
            moveDirection = "up";
        else if (v == Vector2Int.down)
            moveDirection = "down";
    }

    //Grows the snake in the given direction and removes the end of the tail if no fruit is eaten.
    private void Grow(Vector2Int v)
    {
        Vector2Int _newPos = data.snakeList[0].position + v;
        //Check if new tile contains fruit
        if(data.tiles[_newPos].isFruit)
        {
            gameManager.aStarMoving = false;
            gameManager.aStarReady = false;
            data.fruitEaten++;
            data.snakeSpeed *= data.snakeSpeedMultiplier;
            data.tiles[_newPos].isFruit = false;
            data.tiles[_newPos].Occupy();
            gameManager.SpawnFruit();
            scoreText.text = (Convert.ToInt32(scoreText.text) + 1).ToString();
        }
        //Call Game Over if the new tile is occupied, disables this script
        else if(data.tiles[_newPos].occupied)
        {
            gameOver.SetActive(true);
            enabled = false;
        }
        //Removes last part of the snake and occupies the new tile
        else
        {
            data.tiles[data.snakeList.Last.position].UnOccupy();
            data.snakeList.RemoveLast();
            data.tiles[_newPos].Occupy();
        }
        data.snakeList.Push(new SnakePart(_newPos));
    }
}
