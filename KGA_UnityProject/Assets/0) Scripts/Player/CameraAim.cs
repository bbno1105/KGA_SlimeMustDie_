using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAim : MonoBehaviour
{
    public GameObject hitObject;
    public Vector3 hitPos;
    public RaycastHit hit;
    
    public bool isTarget;
    public bool isTrap;

    Vector3 screenCenter;

    public bool IsTrapOn = false;

    void Start()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }


    void Update()
    {
        FindTarget();
    }

    void FindTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        LayerMask targetLayer = LayerMask.NameToLayer("Tile");
        int targetLayMask = 1 << targetLayer.value;

        if (Physics.Raycast(ray, out hit, 10f, targetLayMask))
        {
            isTarget = true;
            hitObject = hit.collider.gameObject;
            hitPos = hit.collider.gameObject.transform.position;
        }
        else
        {
            isTarget = false;
        }
    }
}
