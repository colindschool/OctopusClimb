using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    [SerializeField] private GameObject headBoneStretching;
    [SerializeField] private float allowedStretchDistance;

    private bool isSelected = false;
    private Collider headCollider;

    // Fling variables
    private const float DISTANCE_CLAMP = 2.5f;
    private Vector3 originalLocalPosition = Vector3.zero;
    private Vector3 clickLocation;
    private Vector3 targetLocation;
    private float distance;

    private void Awake()
    {
        headCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if(hit.collider == headCollider)
            {
                isSelected = true;
                clickLocation = hit.point;
                
            }
        }

        if(isSelected)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.Set(mousePos.x, mousePos.y, clickLocation.z);
            distance = Vector3.Distance(clickLocation, mousePos);
            Mathf.Clamp(distance, 0, DISTANCE_CLAMP);
            Vector3 direction = (mousePos - clickLocation).normalized;
            targetLocation = mousePos;

            headBoneStretching.transform.position = targetLocation;
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(isSelected)
            {
                isSelected = false;
                Fling();
                headBoneStretching.transform.localPosition = originalLocalPosition;
            }
        }
    }

    private void Fling()
    {

    }
}
