using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraManipulation : MonoBehaviour
{
    public Transform LookAtPosition = null;
    // public Transform smallViewCam = null;
    // public SceneNode head = null;
    public float zoomSensitivity = 5f, panSensitivity = 0.5f;
    private Vector3 originMouse;
    Quaternion originalLookAt;
    Vector3 originalLocation;
    void Start()
    {
        originMouse = Input.mousePosition;
        originalLookAt = LookAtPosition.localRotation;
        originalLocation = transform.localPosition;
    }
    void Update()
    {
        /*smallViewCam.localPosition = head.MCombinedParentXform.GetColumn(3);
        Vector3 y = head.MCombinedParentXform.GetColumn(1);
        Vector3 z = head.MCombinedParentXform.GetColumn(2);
        y.Normalize();
        z.Normalize();

        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, -z)) * Mathf.Rad2Deg;
        Vector3 axis = Vector3.Cross(Vector3.up, -z);
        smallViewCam.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align forward
        angle = Mathf.Acos(Vector3.Dot(smallViewCam.forward, y)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(smallViewCam.forward, y);
        smallViewCam.localRotation = Quaternion.AngleAxis(angle, axis) * smallViewCam.localRotation;*/

        // Viewing vector is from transform.localPosition to the lookat position
        Vector3 V = LookAtPosition.localPosition - transform.localPosition;
        Vector3 W = Vector3.Cross(-V, Vector3.up);
        Vector3 U = Vector3.Cross(W, -V);
        transform.localRotation = Quaternion.FromToRotation(Vector3.up, U);
        Quaternion alignU = Quaternion.FromToRotation(transform.forward, V);
        transform.localRotation = alignU * transform.localRotation;

        //transform.LookAt(LookAtPosition);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetMouseButton(0))
            {
                Tumble();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                originMouse = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                Panning();
            }
            Zoom();
        }
    }
    void Tumble() 
    {
        // orbit with respect to the transform.right and transform.up axis

        // 1. Rotation of the viewing direction by right axis
        Quaternion camTurnY = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * 5f, transform.right);
        Quaternion camTurnX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5f, Vector3.up);

        // 2. we need to rotate the camera position
        Matrix4x4 r = Matrix4x4.Rotate(camTurnX);
        Matrix4x4 s = Matrix4x4.Rotate(camTurnY);
        Matrix4x4 invP = Matrix4x4.TRS(-LookAtPosition.localPosition, Quaternion.identity, Vector3.one);
        r = invP.inverse * s * r * invP;
        Vector3 newCameraPos = r.MultiplyPoint(transform.localPosition);

        transform.localPosition = newCameraPos;

        //transform.LookAt(LookAtPosition);
        transform.localRotation = camTurnX * camTurnY * transform.localRotation;

        /*if (Mathf.Abs(Vector3.Dot(newCameraPos.normalized, Vector3.up)) < 0.9071f)
        {
            
        }*/
    }
    void Panning() 
    {
        /*Vector3 delta = originMouse - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        LookAtPosition.position += delta;
        transform.position += delta;*/

        float dx = Input.GetAxis("Mouse X") * panSensitivity;
        float dy = Input.GetAxis("Mouse Y") * panSensitivity;

        // First, align up
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, transform.up)) * Mathf.Rad2Deg;
        Vector3 axis = Vector3.Cross(Vector3.up, transform.up);
        LookAtPosition.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align right
        angle = Mathf.Acos(Vector3.Dot(LookAtPosition.right, transform.right)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(LookAtPosition.right, transform.right);
        LookAtPosition.localRotation =  Quaternion.AngleAxis(angle, axis) * LookAtPosition.localRotation;

        LookAtPosition.localPosition += LookAtPosition.right * dx;
        LookAtPosition.localPosition += LookAtPosition.up * dy;

        transform.localPosition += transform.right * dx;
        transform.localPosition += transform.up * dy;
        LookAtPosition.localRotation = originalLookAt;
    }
    void Zoom() 
    {
        float delta = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        transform.localPosition += transform.forward * delta;
    }
    public void resetToOrg() 
    {
        LookAtPosition.position = new Vector3(0, 0, 0);
        transform.localPosition = originalLocation;
    }
}