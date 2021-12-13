using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TheWorld : MonoBehaviour
{
    private Puck _puck;
    public SceneNode TheRoot, TheArm;
    public CameraManipulation cam;
    private UnityEvent _goalScored;
    public int slapshotsAttempted, goalsScored;
    public Text shotAttempts, goals;
    public GameObject slapshotAnimationPrefab, stickTip;
    private void Start()
    {
        _puck = GameObject.Find("Puck").GetComponent<Puck>();
    }
    private void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
        shotAttempts.text = $"Attempts: {slapshotsAttempted}";
        goals.text =$"Goals: {goalsScored}";
    }
    public void resetAllTransform()
    {
        TheRoot.resetTransform();
        cam.resetToOrg();
    }
    public void ResetPlayerPosition()
    {
        TheRoot.ResetPosition();
        cam.resetToOrg();
    }
    public void SlapshotAttempted()
    {
        slapshotsAttempted++;
    }
    public void ScoreGoal()
    {
        goalsScored++;
        TheRoot.ResetPosition();
        _puck.ResetPuckRandom();
    }
    public void ShowSlapshotAnimation()
    {
        GameObject.Instantiate(slapshotAnimationPrefab, GameObject.Find("Slapshot_Button").transform);
    }

    public void SlapShot() 
    {
        StartCoroutine(Rotate(0.5f));
    }

    IEnumerator Rotate(float num)
    {
        stickTip.transform.tag = "StickCollider";
        
        float t = 0;
        while (t < num)
        {
            t += Time.deltaTime;
            Quaternion turn = Quaternion.AngleAxis(-(360*Time.deltaTime*2), Vector3.right);
            TheArm.transform.localRotation = turn * TheArm.transform.localRotation;
            yield return null;
        }
        
        stickTip.transform.tag = "Untagged";
    }
}