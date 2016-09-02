using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    public enum Gesture
    {
        OPEN,
        GRAB
    };

    public OVRInput.Controller Controller;
    public Gesture gesture;

    // Use this for initialization
    void Start () {
        gesture = Gesture.OPEN;
	}

    // Update is called once per frame
    void Update() {
        transform.localPosition = OVRInput.GetLocalControllerPosition(Controller);
        transform.localRotation = OVRInput.GetLocalControllerRotation(Controller);

        OVRInput.RawAxis1D triggerAxis = Controller == OVRInput.Controller.LTouch ? OVRInput.RawAxis1D.LHandTrigger : OVRInput.RawAxis1D.RHandTrigger;
        float grabStrength = OVRInput.Get(triggerAxis);
        Gesture oldGesture = gesture;
        if (grabStrength > 0.8f)
        {
            gesture = Gesture.GRAB;
        } else
        {
            gesture = Gesture.OPEN;
        }

        if (oldGesture != gesture)
        {
            BroadcastMessage("OnHandGestureChanged", this);
        }
    }


    void OnHandGestureChanged() { }
}
