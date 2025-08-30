using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    Vector3 lastPosition;
    [SerializeField] LayerMask placementLayerMask;

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos=Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray=sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100,placementLayerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
