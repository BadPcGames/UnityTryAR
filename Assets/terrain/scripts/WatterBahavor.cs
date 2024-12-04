using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatterBahavor : MonoBehaviour
{
    [SerializeField]
    private float watterHight=0f;
    [SerializeField]
    private float watterColor = 1f;
    [SerializeField]
    private float watterTransposetive=0.7f;
    [SerializeField]
    private Material material;

    public void setColor(float value)
    {
            watterColor = value;
    }

    public void setHight(float value)
    {
        watterHight = value;
    }
    public void setTrans(float value)
    {
            watterTransposetive = value;
    }

    // Start is called before the first frame update
    public void Start()
    {
        var planeRenderer = gameObject.GetComponent<Renderer>();
        Color customColor = new Color(0.0f, 0.0f, watterColor, watterTransposetive);
        material.color=customColor;
        planeRenderer.material=material;

        //planeRenderer.transform.SetLocalPositionAndRotation(new Vector3(0,watterHight,0),new Quaternion());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
