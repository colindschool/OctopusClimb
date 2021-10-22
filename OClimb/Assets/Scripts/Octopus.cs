using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    [SerializeField] private GameObject headBoneStretching;
    [SerializeField] private float allowedStretchDistance;

    private bool isSelected = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if(hit.collider == transform.collider)
            {
                isSelected = true;
            }
        }

        if(isSelected)
        {
            
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(isSelected)
            {
                isSelected = false;
                Fling();
            }
        }
    }

    private void Fling()
    {

    }
}
