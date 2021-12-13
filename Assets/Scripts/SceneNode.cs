using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class SceneNode : MonoBehaviour
{
    private Matrix4x4 mCombinedParentXform;
    private Matrix4x4 originalXform;
    //private Transform axisFrame;
    private Quaternion originRotation;
    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public bool isSelected = false;
    public float moveSen = 0.5f, rotateSen, maxAngle = 45f;
    public Matrix4x4 MCombinedParentXform { get => mCombinedParentXform; set => mCombinedParentXform = value; }
    public Text ctrlText;
    protected void Start()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        //axisFrame = GameObject.Find("AxisFrame").GetComponent<Transform>();
        originRotation = transform.localRotation;
        originalXform = mCombinedParentXform;
    }
    void Update()
    {
        ctrlText.gameObject.SetActive(isSelected);
        if (isSelected)
        {
            float rotateSenTemp = 0f;
            float moveSenTemp = 0f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rotateSenTemp = (rotateSen / 2f) * Time.fixedDeltaTime;
                moveSenTemp = (moveSen / 2f);
            }
            else { rotateSenTemp = rotateSen * Time.fixedDeltaTime; moveSenTemp = moveSen; }

            if (transform.tag == "Leg")
            {
                LegManipulation(rotateSenTemp, moveSenTemp);
            }
            else if (transform.tag == "Torso")
            {
                TorsoManipulation(rotateSenTemp);
            }
            else if (transform.tag == "Arm")
            {
                ArmManipulation(rotateSenTemp);
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
    public void ResetPosition()
    {
        transform.localPosition = originalXform.GetColumn(3);
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

    void LegManipulation(float tempRSen, float tempMSen)
    {
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion turn = Quaternion.AngleAxis(tempRSen, Vector3.up);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempRSen, Vector3.up);
            transform.localRotation = turn * transform.localRotation;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localPosition += Vector3.forward * tempMSen;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localPosition += -Vector3.forward * tempMSen;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localPosition += -Vector3.right * tempMSen;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localPosition += Vector3.right * tempMSen;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetTransform();
        }
    }

    void TorsoManipulation(float tempSen)
    {
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion turn = Quaternion.AngleAxis(tempSen, Vector3.up);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempSen, Vector3.up);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Quaternion turn = Quaternion.AngleAxis(tempSen, transform.right);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempSen, transform.right);
            transform.localRotation = turn * transform.localRotation;
        }
    }

    void ArmManipulation(float tempSen)
    {
        Quaternion temp;
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion turn = Quaternion.AngleAxis(tempSen, transform.up);
            temp = turn * transform.localRotation;
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempSen, transform.up);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Quaternion turn = Quaternion.AngleAxis(tempSen, transform.right);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempSen, transform.right);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Quaternion turn = Quaternion.AngleAxis(tempSen, transform.forward);
            transform.localRotation = turn * transform.localRotation;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Quaternion turn = Quaternion.AngleAxis(-tempSen, transform.forward);
            transform.localRotation = turn * transform.localRotation;
        }

    }
}