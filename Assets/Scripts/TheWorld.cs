using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheWorld : MonoBehaviour
{
    public SceneNode TheRoot;
    public CameraManipulation cam;
    private void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
    }
    public void resetAllTransform()
    {
        TheRoot.resetTransform();
        cam.resetToOrg();
    }
}