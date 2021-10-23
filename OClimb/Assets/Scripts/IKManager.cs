using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    private const float SAMPLING_DISTANCE = 0.1f;
    private const float LEARNING_RATE = 0.1f;

    public static float ErrorFunction(Tentacle tentacle, float[] angles)
    {
        PositionAndRotation result = ForwardKinematics(tentacle, angles);
        return Vector3.Distance(result.position, tentacle.TargetTransform.position);

        /*
        PositionAndRotation result = ForwardKinematics(tentacle, angles);
        float torsion = 0;
        for (int i = 0; i < angles.Length; ++i)
        {
            float absoluteAngle = Mathf.Abs(angles[i]);
            torsion += absoluteAngle * tentacle.TorsionPenalty.x;
            torsion += absoluteAngle * tentacle.TorsionPenalty.y;
            torsion += absoluteAngle * tentacle.TorsionPenalty.z;
        }
        float distance = Vector3.Distance(tentacle.TargetTransform.position, result.position);
        distance += Mathf.Abs(Quaternion.Angle(result.rotation, tentacle.TargetTransform.rotation) / 180f) * tentacle.OrientationWeight;
        distance += (torsion / angles.Length) * tentacle.TorsionWeight;
        */

        //return distance;
    }

    public static void ApproachTarget(Tentacle tentacle)
    {
        List<TentacleJoint> joints = tentacle.TentacleJoints;
        float[] angles = tentacle.Angles;

        for(int i = joints.Count -1; i>=0; --i)
        {
            float error = ErrorFunction(tentacle, angles);
            float slowdown = Mathf.Clamp01((error - tentacle.StopDistance) / (tentacle.SlowdownDistance - tentacle.StopDistance));

            // Gradient descent
            float gradient = CalculateGradient(tentacle, tentacle.Angles, i, SAMPLING_DISTANCE);
            angles[i] -= LEARNING_RATE * gradient * slowdown;
            angles[i] = joints[i].ClampAngle(angles[i]);
            //if (ErrorFunction(tentacle, angles) <= tentacle.StopDistance)
                //break;
        }

        for(int i = 0; i < joints.Count - 1; ++i)
        {
            joints[i].MoveArm(angles[i]);
        }
    }

    private static float CalculateGradient(Tentacle tentacle, float[] angles, int i, float delta)
    {
        float saveAngle = angles[i];

        float f_x = ErrorFunction(tentacle, angles);

        angles[i] += delta;
        float f_x_h = ErrorFunction(tentacle, angles);

        float gradient = (f_x_h - f_x) / delta;

        angles[i] = saveAngle;
        return gradient;
    }

    private static PositionAndRotation ForwardKinematics(Tentacle tentacle, float[] angles)
    {
        List<TentacleJoint> joints = tentacle.TentacleJoints;
        Vector3 previousPoint = joints[0].transform.position;

        Quaternion rotation = tentacle.transform.rotation;
        for(int i = 1; i < joints.Count; ++i)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            previousPoint += rotation * joints[i].InitialPosition;
        }

        return new PositionAndRotation(previousPoint, rotation);
    }

    public struct PositionAndRotation
    {
        public Vector3 position;
        public Quaternion rotation;
        public PositionAndRotation(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
