using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GameObject obj = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (obj == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit hit);
                if (hit.collider != null)
                {
                    obj = hit.collider.gameObject;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            obj = null;
        }

        if(obj != null)
        {
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            obj.transform.position = new Vector3(mousePos.x, mousePos.y, obj.transform.position.z);
        }
    }
}
