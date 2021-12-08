using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TheWorld : MonoBehaviour
{
    private Puck _puck;
    public SceneNode TheRoot;
    public CameraManipulation cam;
    private UnityEvent _goalScored;
    public int slapshotsAttempted, goalsScored;
    public Text shotAttempts, goals;
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
        _puck.ResetPuck();
    }
}