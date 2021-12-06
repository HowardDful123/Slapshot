using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeepInBound : MonoBehaviour
{
    public enum LockAxis { x, y, z }
    [System.Serializable]
    public struct BoundLock
    {
        public LockAxis lockAxis;
        public float lowerBound, upperBound;
    }
    public BoundLock[] boundLocks;
    private float _x, _y, _z;
    private void Start()
    {
        _x = transform.localPosition.x;
        _y = transform.localPosition.y;
        _z = transform.localPosition.z;
    }
    void Update()
    {
        Vector3 newPos = transform.localPosition;
        for (int i = 0; i < boundLocks.Length; i++)
        {
            if (boundLocks[i].lockAxis == LockAxis.x)
            {
                if (transform.localPosition.x > boundLocks[i].upperBound) newPos.x = boundLocks[i].upperBound;
                if (transform.localPosition.x < boundLocks[i].lowerBound) newPos.x = boundLocks[i].lowerBound;
            }
            if (boundLocks[i].lockAxis == LockAxis.y)
            {
                if (transform.localPosition.y > boundLocks[i].upperBound) newPos.y = boundLocks[i].upperBound;
                if (transform.localPosition.y < boundLocks[i].lowerBound) newPos.y = boundLocks[i].lowerBound;
            }
            if (boundLocks[i].lockAxis == LockAxis.z)
            {
                if (transform.localPosition.z > boundLocks[i].upperBound) newPos.z = boundLocks[i].upperBound;
                if (transform.localPosition.z < boundLocks[i].lowerBound) newPos.z = boundLocks[i].lowerBound;
            }
        }
        transform.localPosition = newPos;
    }
}