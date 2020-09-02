
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class TrackableHold : MonoBehaviour
{
    private TrackableBehaviour trackableBehaviour;

    public Transform contentParent;

    public float TimeToHold = 2;

    void Start()
    {
        trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
            trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
    }

    void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        TrackableBehaviour.Status NewStatus = statusChangeResult.NewStatus;
        if (NewStatus == TrackableBehaviour.Status.DETECTED ||
            NewStatus == TrackableBehaviour.Status.TRACKED ||
            NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            TrackingFound();
        }
        else
        {
            TrackingLost();
        }

    }

    private void TrackingFound()
    {
        if (trackableBehaviour)
        {
            var rendererComponents = trackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = trackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = trackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
        }

        DragController dc = FindObjectOfType<DragController>();
        dc.holdingAllowed = true;
        dc.timeToChange = TimeToHold;
        for (int i = 0; i < transform.childCount; i++)
        {
            dc.CurrentHeights.Add(i * 0.0001f);
            Transform child = contentParent.GetChild(i);
            child.localPosition = new Vector3(child.localPosition.x, dc.CurrentHeights[i], child.localPosition.z);
        } 


    }

    private void TrackingLost()
    {

        if (trackableBehaviour)
        {

            FindObjectOfType<DragController>().holdingAllowed = false;

            var rendererComponents = trackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = trackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = trackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }
    }


}
