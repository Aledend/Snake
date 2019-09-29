using UnityEngine;

public class Tile : Object
{
    public bool occupied = false, isFruit = false;
    public Vector2Int position;

    public GameObject go;
    private Data data;
    private MeshRenderer renderer;

    public Tile(Vector2Int position, Transform parent, Material mat, Data data)
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.transform.parent = parent;
        go.transform.position = position.ToVector3();
        this.position = position;
        renderer = go.GetComponent<MeshRenderer>();
        renderer.material = mat;
        this.data = data;
    }

    //Used in game
    public void DestroyTile()
    {
        Destroy(go);
        Destroy(this);
    }

    //Used in editor
    public void DestroyTileImmediate()
    {
        DestroyImmediate(go);
        DestroyImmediate(this);
    }

    //Becomes wall or snake
    public void Occupy()
    {
        occupied = true;
        renderer.material = data.occupiedMat;
    }

    //Becomes fruit
    public void AddFruit()
    {
        renderer.material = data.fruitMat;
        isFruit = true;
    }

    //Becomes snake
    public void RemoveFruit()
    {
        renderer.material = data.occupiedMat;
    }

    //Becomes empty 
    public void UnOccupy()
    {
        occupied = false;
        isFruit = false;
        renderer.material = data.normalMat;
    }
}
