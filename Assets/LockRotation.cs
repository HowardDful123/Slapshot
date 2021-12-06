using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LockRotation : MonoBehaviour
{
    public bool x, y, z;
    private float _x, _y, _z;
    private void Start()
    {
        _x = transform.localPosition.x;
        _y = transform.localPosition.y;
        _z = transform.localPosition.z;
    }
    void Update()
    {
        Quaternion newPos = transform.localRotation;
        if (x) { newPos.x = _x; }
        if (y) { newPos.y = _y; }
        if (z) { newPos.z = _z; }
        transform.localRotation = newPos;
    }
}