using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class Data : ScriptableObject
{
    [System.NonSerialized]
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    [System.NonSerialized]
    public LinkedList<SnakePart> snakeList = new LinkedList<SnakePart>();
    public Material occupiedMat, fruitMat, normalMat;
    [System.NonSerialized]
    public int fruitEaten = 0;
    [System.NonSerialized]
    public float snakeSpeed = 1.0f;

    [SerializeField]
    private GameObject gameOverUI;

}
