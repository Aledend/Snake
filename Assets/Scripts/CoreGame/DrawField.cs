using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DrawField : MonoBehaviour
{
    public uint fieldHeight = 10;
    public uint fieldWidth = 10;
    private uint heightCheck;
    private uint widthCheck;
    private uint minWidth = 10;
    private uint minHeight = 10;
    private uint maxWidth = 40;
    private uint maxHeight = 40;
    [SerializeField]
    private Material tileMat;
    [SerializeField]
    private InputField widthInput, heightInput;
    [SerializeField]
    private Slider widthSlider, heightSlider;

    [SerializeField]
    Data data;



    // Start is called before the first frame update
    private void Awake()
    {
        int _count = gameObject.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            if (Application.isPlaying)
                Destroy(gameObject.transform.GetChild(i).gameObject);
            else if (Application.isEditor)
                DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
            data.tiles.Clear();
        }
        
        heightCheck = fieldHeight;
        widthCheck = fieldWidth;
        for (int i = 0; i < fieldWidth; i++)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                Tile t = new Tile(pos, transform, tileMat, data);
                data.tiles.Add(pos, t);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)
        {
            if (heightCheck != fieldHeight || widthCheck != fieldWidth)
            {
                if (fieldHeight > 40)
                    fieldHeight = 40;
                else if (fieldHeight < 10)
                    fieldHeight = 10;
                if (fieldWidth > 40)
                    fieldWidth = 40;
                else if (fieldWidth < 10)
                {
                    fieldWidth = 10;
                }
                UpdateField();
                MoveCamera();
            }
        }
        
    }

    private void UpdateField()
    {

        //Remove rows
        if(widthCheck > fieldWidth)
        {
            for (int i = (int)fieldWidth; i < widthCheck; i++)
            {
                for (int j = 0; j < fieldHeight; j++)
                {
                    Vector2Int _pos = new Vector2Int(i, j);
                    if (Application.isPlaying)
                    {
                        data.tiles[_pos].DestroyTile();
                        data.tiles.Remove(_pos);
                    }
                    else if (Application.isEditor)
                    {
                        data.tiles[_pos].DestroyTileImmediate();
                        data.tiles.Remove(_pos);
                    }
                }
            }
        }
        else if(heightCheck > fieldHeight)
        {
            for (int i = (int)fieldHeight; i < heightCheck; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    Vector2Int _pos = new Vector2Int(j, i);
                    if (Application.isPlaying)
                    {
                        data.tiles[_pos].DestroyTile();
                        data.tiles.Remove(_pos);
                    }
                    else if (Application.isEditor)
                    {
                        data.tiles[_pos].DestroyTileImmediate();
                        data.tiles.Remove(_pos);
                    }
                }
            }
        }
        

        //Add rows
        else if(widthCheck < fieldWidth)
        {
            for (int i = (int)widthCheck; i < fieldWidth; i++)
            {
                for (int j = 0; j < fieldHeight; j++)
                {
                    Vector2Int _pos = new Vector2Int(i, j);
                    Tile t = new Tile(_pos, transform, tileMat, data);
                    data.tiles.Add(_pos, t);
                }
            }
        }
        else if(heightCheck < fieldHeight)
        {
            for (int i = (int)heightCheck; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    Vector2Int _pos = new Vector2Int(j, i);
                    Tile t = new Tile(_pos, transform, tileMat, data);
                    data.tiles.Add(_pos, t);
                }
            }
        }
        heightCheck = fieldHeight;
        widthCheck = fieldWidth;
    }

    private void MoveCamera()
    {
        transform.parent.Find("PlayerViewCamera").position = new Vector3(3 * fieldWidth / 10f, fieldHeight / 2f,  fieldWidth >= fieldHeight ? -fieldWidth*1.3f : -fieldHeight - (0.3f*fieldWidth));
    }

    public void SendBoxData()
    {
        uint _widthValue;
        uint _heightValue;
        try
        {
            _widthValue = Convert.ToUInt32(widthInput.text);
        }
        catch { _widthValue = minWidth; }

        try
        {
            _heightValue = Convert.ToUInt32(heightInput.text);
        }
        catch { _heightValue = minHeight; }

        //Keep width and height within bounds
        fieldWidth = _widthValue < minWidth ? minWidth : _widthValue > maxWidth ? maxWidth : _widthValue;
        fieldHeight = _heightValue < minHeight ? minHeight : _heightValue > maxHeight ? maxHeight : _heightValue;

        //Update sliders
        widthSlider.value = fieldWidth;
        heightSlider.value = fieldHeight;

        UpdateField();
        MoveCamera();
    }

    public void SendSliderData()
    {
        fieldWidth = (uint)widthSlider.value;
        fieldHeight = (uint)heightSlider.value;

        widthInput.text = fieldWidth.ToString();
        heightInput.text = fieldHeight.ToString();

        UpdateField();
        MoveCamera();
    }

    public void GameStart()
    {
        DrawWalls();
    }

    private void DrawWalls()
    {
        //Draw horizontal walls
        for (int i = 0; i < fieldWidth; i++)
        {
            data.tiles[new Vector2Int(i, 0)].Occupy();
            data.tiles[new Vector2Int(i, (int)fieldHeight - 1)].Occupy();
        }

        //Draw vertical walls
        for (int i = 1; i < fieldHeight - 1; i++)
        {
            data.tiles[new Vector2Int(0, i)].Occupy();
            data.tiles[new Vector2Int((int)fieldWidth - 1, i)].Occupy();
        }
    }
}
