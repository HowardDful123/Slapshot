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
        if (slapshot)
        {
            _keepInBound.enabled = false;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (newPos.y < 0.03f) newPos.y = 0.03f;
            transform.position = newPos;
        }
        //if (slapshot) _keepInBound.enabled = false;
        else _keepInBound.enabled = true;
        if (slapshot && _rigidBody.velocity.magnitude <= float.Epsilon) ResetPuckRandom();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StickCollider")
        {
            slapshot = true;
            _rigidBody.AddForce(other.transform.forward * power);
            _theWorld.SlapshotAttempted();
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            _theWorld.SlapshotAttempted();
            ResetPuckRandom();
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
        //_theWorld.ResetPlayerPosition();
    }
    public void ResetPuck()
    {
        slapshot = false;
        _positionStart.y = 0.05f;
        transform.localPosition = _positionStart;
        transform.rotation = Quaternion.identity;
        _rigidBody.velocity = Vector3.zero;
    }

    public void ResetPuckRandom()
    {
        slapshot = false;
        _positionStart.y = 0.05f;
        transform.localPosition = new Vector3(Random.Range(-4f, 4f) , _positionStart.y, Random.Range(0f, 6f));
        transform.rotation = Quaternion.identity;
        _rigidBody.velocity = Vector3.zero;
    }
}