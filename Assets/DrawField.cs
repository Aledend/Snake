using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DrawField : MonoBehaviour
{
    public uint fieldHeight = 10;
    public uint fieldWidth = 10;
    private uint heightCheck;
    private uint widthCheck;
    [SerializeField]
    private Material tileMat;
    [SerializeField]
    private InputField widthInput, heightInput;
    [SerializeField]
    private Slider widthSlider, heightSlider;


    // Start is called before the first frame update
    private void Awake()
    {
        heightCheck = fieldHeight;
        widthCheck = fieldWidth;
        for (int i = 0; i < fieldWidth; i++)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.transform.parent = gameObject.transform;
                go.transform.localPosition = new Vector3(i, j);
               // go.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
                // 
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
        int _count = gameObject.transform.childCount;
        Debug.Log("b4: " + gameObject.transform.childCount);
        for (int i = 0; i < _count; i++)
        {
            if (Application.isPlaying)
            {

                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            else if (Application.isEditor)
                DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
        Debug.Log("after: " + gameObject.transform.childCount);

        heightCheck = fieldHeight;
        widthCheck = fieldWidth;
        for (int i = 0; i < fieldWidth; i++)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.transform.parent = gameObject.transform;
                go.transform.localPosition = new Vector3(i, j);
                go.GetComponent<MeshRenderer>().material = tileMat;
            }
        }
    }

    private void MoveCamera()
    {
        transform.parent.Find("PlayerViewCamera").position = new Vector3(fieldWidth / 2f - fieldWidth/5f, fieldHeight / 2f,  fieldWidth >= fieldHeight ? -fieldWidth*1.3f : -fieldHeight - (0.3f*fieldWidth));
    }

    public void SendBoxData()
    {
        uint _widthValue;
        uint _heightValue;
        try
        {
            _widthValue = Convert.ToUInt32(widthInput.text);
        }
        catch { _widthValue = 10; }

        try
        {
            _heightValue = Convert.ToUInt32(heightInput.text);
        }
        catch { _heightValue = 10; }


        if (_widthValue < 10)
        {
            _widthValue = 10;
        }
        else if (_widthValue > 40)
        {
            _widthValue = 40;
        }
            

        if (_heightValue < 10)
        {
            _heightValue = 10;
        }
            
        else if (_heightValue > 40)
        {
            _heightValue = 40;
        }
            

        widthSlider.value = _widthValue;
        heightSlider.value = _heightValue;

        fieldWidth = _widthValue;
        fieldHeight = _heightValue;

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
}
