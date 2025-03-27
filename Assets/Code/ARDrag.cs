using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ARDrag : MonoBehaviour
{
    float rotSpeed = 1;
    float scaleSpeed = .001f;
    Vector2 startPos1;
    Vector2 startPos2;
    float startDist;

    Transform selected;
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new();


    public LayerMask selectableLayer;
    Camera mainCam;

    void Start()
    {
        EnhancedTouchSupport.Enable();
        raycastManager = GetComponent<ARRaycastManager>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Touch.activeTouches.Count == 1)
        {
            Touch touch1 = Touch.activeTouches[0];

            if (touch1.phase == TouchPhase.Began)
            {
                Ray ray = mainCam.ScreenPointToRay(touch1.screenPosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 20, selectableLayer))
                {
                    if (selected != null)
                    {
                        selected.gameObject.GetComponent<Highlight>().Selected(false);
                    }
                    selected = hit.collider.transform;
                    selected.gameObject.GetComponent<Highlight>().Selected(true);
                }

            else if (selected != null)
            {
                selected.gameObject.GetComponent<Highlight>().Selected(false);
                selected = null;
            }
            
            }
            else if (touch1.phase == TouchPhase.Moved)
            {
                if (selected == null) return;

                DragObj(touch1.screenPosition);
            }
        }
        else if (Input.touchCount == 2)
        {
            if (selected == null) return;

            Touch touch1 = Touch.activeTouches[0];
            Touch touch2 = Touch.activeTouches[1];

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                startPos1 = touch1.screenPosition;
                startPos2 = touch2.screenPosition;
                startDist = Vector2.Distance(startPos1, startPos2);
            }

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                if (Vector2.Angle(touch1.screenPosition - startPos1, touch2.screenPosition - startPos2) < 60)
                {
                    RotObj(touch1.delta.x);
                }
                else
                {
                    ScaleObject(touch1.screenPosition, touch2.screenPosition);
                }
                startDist = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);
            }
        }
    }

    void RotObj(float deltaX)
    {
        selected.Rotate(0, -deltaX * rotSpeed, 0, Space.Self);
    }

    void ScaleObject(Vector2 pos1, Vector2 pos2)
    {
        float curDist = Vector2.Distance(pos1, pos2);
        selected.localScale += (curDist - startDist) * scaleSpeed * Vector3.one;
        if (selected.localScale.x < .01f)
        {
            selected.localScale = .01f * Vector3.one;
        }
    }

    void DragObj(Vector2 pos)
    {
        if (raycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            selected.position = hits[0].pose.position;
        }
    }

    public void DeleteItem()
    {
        if (selected != null)
        {
            Destroy(selected.gameObject);
        }
    }
}