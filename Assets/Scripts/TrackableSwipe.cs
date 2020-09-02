
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class TrackableSwipe : MonoBehaviour
{
    private TrackableBehaviour trackableBehaviour;

    public Transform swipeImage;
    public Transform planeToDraw;
    public Color colorForMarker;
    public int maxVerticesToRemove;

    private BrushController brushController;

    private bool firstTime = true;

    void Start()
    {
        brushController = FindObjectOfType<BrushController>();

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

        
        brushController.PlaneToDraw = planeToDraw;
        brushController.SwipeImage = swipeImage;
        brushController.swipingAway = true;
        brushController.color = colorForMarker;
        brushController.maxVertices = maxVerticesToRemove;

        // the marker was found for the first time
        if (firstTime)
        {
            planeToDraw.gameObject.SetActive(false);
            firstTime = false;
        }
        else
        {
            planeToDraw.gameObject.SetActive(true);
        }

        brushController.initializeSwiping();
    }

    private void TrackingLost()
    {

        if (trackableBehaviour)
        {


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
