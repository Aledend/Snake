using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class Tile : Object
{
    public bool occupied = false, isFruit = false;
    public Vector2Int position;

    public GameObject go;
    Data data;
    MeshRenderer renderer;

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

    public void DestroyTile()
    {
        Destroy(go);
        Destroy(this);
    }

    public void DestroyTileImmediate()
    {
        DestroyImmediate(go);
        DestroyImmediate(this);
    }

    public void Occupy()
    {
        occupied = true;
        renderer.material = data.occupiedMat;
    }

    public void AddFruit()
    {
        renderer.material = data.fruitMat;
        isFruit = true;
    }
    public void RemoveFruit()
    {
        renderer.material = data.occupiedMat;
    }

    public void UnOccupy()
    {
        occupied = false;
        isFruit = false;
        renderer.material = data.normalMat;
    }
}
