using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NodePrimitive : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public bool rotate = false;
    public float speed = 1f;

    public bool yAxis = true;
    // public float speed = 0.5f;
    public float rate = 1f;
    public float angle = 45f;
    



    float t = 1f;

    // Use this for initialization
    void Start()
    {
        t = rate;
    }

    void Update()
    {
        if (rotate)
        {
            //t = t + Time.deltaTime;
            //float phase = Mathf.Sin(t / rate);
            //if(yAxis)
            //    transform.localRotation = Quaternion.Euler(new Vector3(0, phase * angle, 0));
            //else
            //{
            //    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, phase * angle));
            //}



            Quaternion q = Quaternion.AngleAxis(speed, Vector3.up);
            if(!yAxis)
                q = Quaternion.AngleAxis(speed, Vector3.forward);
            if (t >= 0)
            {
                Quaternion qr;
                qr = transform.localRotation * q;

                transform.localRotation = qr;
                t -= Time.deltaTime;
            }
            else
            {
                speed *= -1;
                t = rate;
            }
        }
    }


    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }
}