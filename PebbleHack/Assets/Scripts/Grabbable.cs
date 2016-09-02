using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour
{
    Hand candidateHand = null;

	// Use this for initialization
	void Start () {
	}

    // Only called in relation to the parent hand.
    void OnHandGestureChanged(Hand hand)
    {
        if (hand.gesture == Hand.Gesture.OPEN)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            this.transform.parent = null;
            rigidbody.isKinematic = false;

            // The touch controller's coordinate space doesn't map to Unity's space.
            //Vector3 velocity = OVRInput.GetLocalControllerVelocity(hand.Controller);
			/*float scale = Mathf.Pow(velocity.magnitude, 1.5f);
			float scale = Mathf.Log(velocity.magnitude * 5);
			velocity.Scale(new Vector3(-scale, scale, -scale));
            rigidbody.velocity = velocity;

            // TODO: This isn't right.
            Quaternion handQuaternian = OVRInput.GetLocalControllerAngularVelocity(hand.Controller);
            rigidbody.angularVelocity = handQuaternian.eulerAngles;*/
        }
    }

	// Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        var parent = other.gameObject.transform.parent;
        if (parent == null) {
            return;
        }

        Hand grabbingHand = parent.GetComponent<Hand>();
        if (grabbingHand && grabbingHand.gesture == Hand.Gesture.OPEN && !grabbingHand.GetComponentInChildren<Grabbable>())
        {
            candidateHand = grabbingHand;
        }
	}

    void Update()
    {
        if (candidateHand && candidateHand.gesture == Hand.Gesture.GRAB && !candidateHand.GetComponentInChildren<Grabbable>())
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            this.transform.parent = candidateHand.transform;
			rigidbody.isKinematic = true;

			GrabPosition grabPosition = candidateHand.GetComponentInChildren<GrabPosition>();
			if (grabPosition != null) {
				this.transform.localPosition = grabPosition.offset;
			} else {
				this.transform.localPosition = new Vector3(0f, 0f, 0f);
			}

            candidateHand = null;
        }
    }
}
