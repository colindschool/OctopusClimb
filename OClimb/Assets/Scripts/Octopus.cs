using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    [SerializeField] private GameObject headBoneStretching;
    [SerializeField] private List<Tentacle> tentacles;
    [SerializeField] private float allowedStretchDistance;

    private bool isSelected = false;
    private Collider headCollider;
    private Rigidbody octopusRigidbody;

    // Fling variables
    [SerializeField] private float maxPower;
    private const float DISTANCE_CLAMP = 1.5f;
    private Vector3 originalLocalPosition = Vector3.zero;
    private Vector3 clickLocation;
    private Vector3 targetLocation;
    private float distance;

    private void Awake()
    {
        tentacles = new List<Tentacle>();
        foreach(Tentacle t in GetComponentsInChildren<Tentacle>())
        {
            tentacles.Add(t);
        }
        headCollider = GetComponent<Collider>();
        octopusRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate()
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
        // Calculate the mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.Set(mousePos.x, mousePos.y, clickLocation.z);
        // Calculate the direction and force to apply to the octopus
        Vector3 direction = clickLocation - mousePos;
        float distancePowerRatio = Vector3.Distance(clickLocation, mousePos) / DISTANCE_CLAMP;
        distancePowerRatio = Mathf.Clamp01(distancePowerRatio);
        float force = distancePowerRatio * maxPower;
        DisconnectTentacles();
        // Apply force to the Octopus rigidbody
        octopusRigidbody.AddForce(force * direction, ForceMode.Impulse);
        Debug.Log(force * direction);
    }

    private void DisconnectTentacles()
    {

    }
}
