using UnityEngine;
using UnityEngine.UI;
public class Puck : MonoBehaviour
{
    private KeepInBound _keepInBound;
    private Rigidbody _rigidBody;
    public bool slapshot;
    public float power;
    void Start()
    {
        _keepInBound = GetComponent<KeepInBound>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    { 
        if (slapshot) _keepInBound.enabled = false;
        else _keepInBound.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StickCollider")
        {
            slapshot = true;
            _rigidBody.AddForce(other.transform.forward * power);
        }
    }
    public void setPower(float p)
    {
        power = 100 * p;
    }
}