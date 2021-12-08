using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneNodeInteractor : MonoBehaviour
{
    public GameObject parentSN;
    public Vector3 relativePosition;
    public int forceMagnitude;
    private void Start()
    {
        SNInteractorStart();
    }
    void Update()
    {
        Matrix4x4 parentXform = parentSN.transform.GetComponent<SceneNode>().MCombinedParentXform;
        Quaternion newRotation = parentXform.rotation;
        Vector3 parentPos = parentSN.transform.GetComponent<SceneNode>().MCombinedParentXform.GetColumn(3);
        Vector3 newPos = parentPos + relativePosition;
        transform.SetPositionAndRotation(newPos, newRotation);
    }
    public void SNInteractorStart()
    {
        parentSN.transform.GetComponent<SceneNode>().MCombinedParentXform.MultiplyPoint3x4(transform.localPosition);
    }
}