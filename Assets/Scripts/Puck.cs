using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class Puck : MonoBehaviour
{
    private KeepInBound _keepInBound;
    private Rigidbody _rigidBody;
    private Vector3 _positionStart;
    private TheWorld _theWorld;
    [SerializeField] private GameObject _puckTimeFrame;
    public bool slapshot;
    public float power;
    public UnityEvent goalScored;
    void Start()
    {
        _keepInBound = GetComponent<KeepInBound>();
        _rigidBody = GetComponent<Rigidbody>();
        _theWorld = GameObject.Find("TheWorld").GetComponent<TheWorld>();
        SetPower(GameObject.Find("PuckPowerSlider").GetComponent<Slider>().value);
        _positionStart = transform.localPosition;
    }
    void Update()
    {
        _puckTimeFrame.SetActive(slapshot);
        if (slapshot) _keepInBound.enabled = false;
        else _keepInBound.enabled = true;
        if (slapshot && _rigidBody.velocity.magnitude <= float.Epsilon) ResetPuckAndPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StickCollider")
        {
            slapshot = true;
            _rigidBody.AddForce(other.transform.forward * power);
            _theWorld.SlapshotAttempted();
        }
        else if (other.gameObject.name == "GoalCollider")
        {
            _theWorld.ScoreGoal();
        }
    }
    public void SetPower(float p)
    {
        power = p;
    }
    public void ResetPuckAndPlayer()
    {
        if (slapshot) slapshot = false;
        _theWorld.ResetPlayerPosition();
    }
    public void ResetPuck()
    {
        slapshot = false;
        _positionStart.y = 0.05f;
        transform.localPosition = _positionStart;
        transform.rotation = Quaternion.identity;
        _rigidBody.velocity = Vector3.zero;
    }
}