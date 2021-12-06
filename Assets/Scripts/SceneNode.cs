using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour
{

    private Matrix4x4 mCombinedParentXform;
    private Matrix4x4 originalXform;
    //private Transform axisFrame;
    private Quaternion originRotation;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public bool isSelected = false;
    public float moveSen = 0.5f, rotateSen = 0.5f;


    public Matrix4x4 MCombinedParentXform { get => mCombinedParentXform; set => mCombinedParentXform = value; }


    // Use this for initialization
    protected void Start()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        //axisFrame = GameObject.Find("AxisFrame").GetComponent<Transform>();
        originRotation = transform.localRotation;
        originalXform = mCombinedParentXform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Quaternion turn = Quaternion.AngleAxis(rotateSen, transform.right);

                transform.localRotation = turn * transform.localRotation;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Quaternion turn = Quaternion.AngleAxis(-rotateSen, transform.right);

                transform.localRotation = turn * transform.localRotation;
            }

            if (Input.GetKey(KeyCode.W)) 
            {
                transform.localPosition += Vector3.forward * moveSen;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition += -Vector3.forward * moveSen;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition += -Vector3.right * moveSen;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += Vector3.right * moveSen;
            }

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