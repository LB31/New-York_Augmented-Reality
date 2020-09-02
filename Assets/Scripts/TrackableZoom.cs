
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class TrackableZoom : MonoBehaviour
{
    private TrackableBehaviour trackableBehaviour;

    public float minZoom = 0.1f;
    public float maxZoom = 2;


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
        dc.zoomAllowed = true;
        dc.MinZoom = minZoom;
        dc.MaxZoom = maxZoom;
        dc.timeToChange = 10000;
        


    }

    private void TrackingLost()
    {

        if (trackableBehaviour)
        {

            FindObjectOfType<DragController>().zoomAllowed = false;

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
