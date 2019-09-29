using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DrawField : MonoBehaviour
{
    private uint heightCheck;
    private uint widthCheck;
    private uint minWidth = 10;
    private uint minHeight = 10;
    private uint maxWidth = 40;
    private uint maxHeight = 40;
    [SerializeField]
    private Material tileMat = null;
    [SerializeField]
    private InputField widthInput = null, heightInput = null;
    [SerializeField]
    private Slider widthSlider = null, heightSlider = null;

    [SerializeField]
    private Data data = null;

 
    private void Awake()
    {
        //Destroy all tiles that make up the playfield
        int _count = gameObject.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            if (Application.isPlaying)
                Destroy(gameObject.transform.GetChild(i).gameObject);
            else if (Application.isEditor)
                DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
            data.tiles.Clear();
        }
        
        //Generate new tiles
        heightCheck = data.fieldHeight;
        widthCheck = data.fieldWidth;
        for (int i = 0; i < data.fieldWidth; i++)
        {
            for (int j = 0; j < data.fieldHeight; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                Tile t = new Tile(pos, transform, tileMat, data);
                data.tiles.Add(pos, t);
            }
        }
    }
    
    void Update()
    {
        if(!Application.isPlaying)
        {
            if (heightCheck != data.fieldHeight || widthCheck != data.fieldWidth)
            {
                if (data.fieldHeight > 40)
                    data.fieldHeight = 40;
                else if (data.fieldHeight < 10)
                    data.fieldHeight = 10;
                if (data.fieldWidth > 40)
                    data.fieldWidth = 40;
                else if (data.fieldWidth < 10)
                {
                    data.fieldWidth = 10;
                }
                UpdateField();
                MoveCamera();
            }
        }
        
    }


    //Called upon changins the size of the playfield.
    private void UpdateField()
    {

        //Remove rows
        if(widthCheck > data.fieldWidth)
        {
            for (int i = (int)data.fieldWidth; i < widthCheck; i++)
            {
                for (int j = 0; j < data.fieldHeight; j++)
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
        else if(heightCheck > data.fieldHeight)
        {
            for (int i = (int)data.fieldHeight; i < heightCheck; i++)
            {
                for (int j = 0; j < data.fieldWidth; j++)
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
        else if(widthCheck < data.fieldWidth)
        {
            for (int i = (int)widthCheck; i < data.fieldWidth; i++)
            {
                for (int j = 0; j < data.fieldHeight; j++)
                {
                    Vector2Int _pos = new Vector2Int(i, j);
                    Tile t = new Tile(_pos, transform, tileMat, data);
                    data.tiles.Add(_pos, t);
                }
            }
        }
        else if(heightCheck < data.fieldHeight)
        {
            for (int i = (int)heightCheck; i < data.fieldHeight; i++)
            {
                for (int j = 0; j < data.fieldWidth; j++)
                {
                    Vector2Int _pos = new Vector2Int(j, i);
                    Tile t = new Tile(_pos, transform, tileMat, data);
                    data.tiles.Add(_pos, t);
                }
            }
        }
        heightCheck = data.fieldHeight;
        widthCheck = data.fieldWidth;
    }

    private void MoveCamera()
    {
        transform.parent.Find("PlayerViewCamera").position = new Vector3(3 * data.fieldWidth / 10f, data.fieldHeight / 2f,  data.fieldWidth >= data.fieldHeight ? -data.fieldWidth*1.3f : -data.fieldHeight - (0.3f*data.fieldWidth));
    }

    //Called upon changing the value of a box in the settings
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
        data.fieldWidth = _widthValue < minWidth ? minWidth : _widthValue > maxWidth ? maxWidth : _widthValue;
        data.fieldHeight = _heightValue < minHeight ? minHeight : _heightValue > maxHeight ? maxHeight : _heightValue;

        //Update sliders
        widthSlider.value = data.fieldWidth;
        heightSlider.value = data.fieldHeight;

        UpdateField();
        MoveCamera();
    }

    //Called upon changing the value of a slider in settings
    public void SendSliderData()
    {
        data.fieldWidth = (uint)widthSlider.value;
        data.fieldHeight = (uint)heightSlider.value;

        widthInput.text = data.fieldWidth.ToString();
        heightInput.text = data.fieldHeight.ToString();

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
        for (int i = 0; i < data.fieldWidth; i++)
        {
            data.tiles[new Vector2Int(i, 0)].Occupy();
            data.tiles[new Vector2Int(i, (int)data.fieldHeight - 1)].Occupy();
        }

        //Draw vertical walls
        for (int i = 1; i < data.fieldHeight - 1; i++)
        {
            data.tiles[new Vector2Int(0, i)].Occupy();
            data.tiles[new Vector2Int((int)data.fieldWidth - 1, i)].Occupy();
        }
    }
}
