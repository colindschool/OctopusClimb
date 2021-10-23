using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleJoint : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;
    public Vector3 axis;

    private Vector3 initialPosition;
    public Vector3 InitialPosition { get => initialPosition; }
    private Vector3 initialRotation;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }

    private float SetAngle(float angle)
    {
        angle = ClampAngle(angle);
        transform.localEulerAngles = axis * angle;
        return angle;
    }

    public float MoveArm(float angle)
    {
        return SetAngle(angle);
    }

    public float ClampAngle(float angle)
    {
        return Mathf.Clamp(angle, minAngle, maxAngle);
    }
}
