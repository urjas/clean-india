using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class Click : MonoBehaviour {

    public Camera FirstPersonCamera;
    public GameObject DefaultEnv;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                // Instantiate Andy model at the hit pose.
                var andyObject = Instantiate(DefaultEnv, hit.Pose.position, hit.Pose.rotation);

                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                andyObject.transform.Rotate(0, 0, 0, Space.Self);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                // world evolves.
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make Andy model a child of the anchor.
                andyObject.transform.parent = anchor.transform;
            }
        }

    }
}
