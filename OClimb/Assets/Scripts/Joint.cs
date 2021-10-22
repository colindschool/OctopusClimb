using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private Vector3 offset;

    public Vector3 Axis { get => axis; }
    public Vector3 Offset { get => offset; }

    private void Awake()
    {
        offset = transform.localPosition;
        axis = Vector3.forward;
    }
}
