using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 0)]
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
    public float snakeSpeed = 1.0f, snakeSpeedMultiplier = 1.1f;

    public uint fieldHeight = 10;
    public uint fieldWidth = 10;


}
