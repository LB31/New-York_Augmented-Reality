using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private Transform hitObject;

    public List<float> CurrentHeights;

    private Vector3 startPosObj;
    private float t;
    public float timeToChange = 2;
    private float allowedDistance = 1;

    public float MaxZoom = 3;
    public float MinZoom = 0.5f;
    public float ZoomSensitivity = 0.001f;

    public bool zoomAllowed;
    public bool holdingAllowed;

    // For testing
    //public Transform contentParent;

    private void Start()
    {
        // For testing
        /*
        for (int i = 0; i < contentParent.childCount; i++)
        {
            CurrentHeights.Add(i * 0.0001f);
            Transform child = contentParent.GetChild(i);
            child.localPosition = new Vector3(child.localPosition.x, CurrentHeights[i], child.localPosition.z);
        } */
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (Input.GetMouseButtonDown(0))
        {
            hitObject = Array.Find(hits, element => element.transform.tag.Equals("moveable")).transform;
            if (hitObject)
                startPosObj = hitObject.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (hitObject != null && timeToChange < 100 && CurrentHeights.Count > 0)
                hitObject.localPosition = new Vector3(hitObject.localPosition.x, CurrentHeights[hitObject.GetSiblingIndex()], hitObject.localPosition.z);

            hitObject = null;
            t = 0;
            startPosObj = Vector3.zero;

        }

        if (zoomAllowed)
            zoom(Input.GetAxis("Mouse ScrollWheel") * 0.5f);

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            if (zoomAllowed)
                zoom(difference * ZoomSensitivity);
        }
        else if (Input.GetMouseButton(0))
        {
            if (Array.Find(hits, element => element.transform.tag.Equals("moveable")).transform)
                t += Time.deltaTime;

            // Hold object for specific time
            if (holdingAllowed && t >= timeToChange && hitObject && Vector3.Distance(hitObject.position, startPosObj) < allowedDistance)
            {
                hitObject.SetAsLastSibling();
                for (int i = 0; i < hitObject.parent.childCount; i++)
                {
                    Transform child = hitObject.parent.GetChild(i);
                    child.localPosition = new Vector3(child.localPosition.x, CurrentHeights[i], child.localPosition.z);
                }
                t = 0;
            }

            Vector3 hitPoint = Array.Find(hits, element => element.transform.name.Equals("PlaneToDraw")).point;

            if (hitPoint != Vector3.zero && hitObject != null)
            {
                hitObject.position = hitPoint;
            }
        }

    }

    public void zoom(float increment)
    {
        if (!hitObject) return;
        Vector3 curScale = hitObject.localScale;
        Vector3 curPos = hitObject.localPosition;
        if (curScale.y + increment > MinZoom && curScale.y + increment < MaxZoom)
        {
            hitObject.localScale = new Vector3(curScale.x, curScale.y + increment, curScale.z);
            hitObject.localPosition = new Vector3(curPos.x, curScale.y * 0.5f, curPos.z);
        }


    }


}
