using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour
{

    private Matrix4x4 mCombinedParentXform;
    private Matrix4x4 originalXform;
    private Transform axisFrame;
    private Quaternion originRotation;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public bool isSelected = false;

    public Matrix4x4 MCombinedParentXform { get => mCombinedParentXform; set => mCombinedParentXform = value; }


    // Use this for initialization
    protected void Start()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        axisFrame = GameObject.Find("AxisFrame").GetComponent<Transform>();
        originRotation = transform.localRotation;
        originalXform = mCombinedParentXform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) 
        {
            axisFrame.localPosition = MCombinedParentXform.GetColumn(3);
            Vector3 x = MCombinedParentXform.GetColumn(0);
            Vector3 y = MCombinedParentXform.GetColumn(1);
            Vector3 z = MCombinedParentXform.GetColumn(2);
            Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
            axisFrame.localScale = size;


            // Align rotation
            // WorldTransform.localRotation = Quaternion.LookRotation(z / size.z, y / size.y);
            // OR
            y.Normalize();
            z.Normalize();
            // First, align up
            float angle = Mathf.Acos(Vector3.Dot(Vector3.up, y)) * Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(Vector3.up, y);
            axisFrame.localRotation = Quaternion.AngleAxis(angle, axis);
            // Now, align forward
            angle = Mathf.Acos(Vector3.Dot(axisFrame.forward, z)) * Mathf.Rad2Deg;
            axis = Vector3.Cross(axisFrame.forward, z);
            axisFrame.localRotation = Quaternion.AngleAxis(angle, axis) * axisFrame.localRotation;
        }
        
    }

    private void InitializeSceneNode()
    {
        MCombinedParentXform = Matrix4x4.identity;
    }

    public void resetTransform() 
    {
        transform.localPosition = originalXform.GetColumn(3);
        Vector3 x = originalXform.GetColumn(0);
        Vector3 y = originalXform.GetColumn(1);
        Vector3 z = originalXform.GetColumn(2);
        transform.localScale = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        /*Quaternion rotation = Quaternion.LookRotation(originalXform.GetColumn(2), originalXform.GetColumn(1));
        transform.localRotation = rotation;*/

        transform.localRotation = originRotation;

        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                //Debug.Log(cn.transform);
                cn.resetTransform();
            }
        }
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        MCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }
}