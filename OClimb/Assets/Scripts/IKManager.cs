using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    private const float SAMPLING_DISTANCE = 0.05f;
    private const float LEARNING_RATE = 0.05f;

    public static void InverseKinematics (Vector3 target, JointCreator tentacle, float[] angles)
    {
        for (int i = 0; i < tentacle.AllJoints.Count; ++i)
        {
            float gradient = PartialGradient(target, tentacle, angles, i);
            angles[i] -= LEARNING_RATE * gradient;
            Joint joint = tentacle.AllJoints[i];
            joint.transform.localEulerAngles = new Vector3(joint.transform.localEulerAngles.x, joint.transform.localEulerAngles.y, angles[i]);
        }
    }

    public static float PartialGradient (Vector3 target, JointCreator tentacle, float[] angles, int i)
    {
        float angle = angles[i];
        float fX = DistanceFromTarget(target, tentacle, angles);

        angles[i] += SAMPLING_DISTANCE;
        float fXPlusD = DistanceFromTarget(target, tentacle, angles);

        angles[i] = angle;

        float gradient = (fXPlusD - fX) / SAMPLING_DISTANCE;
        
        return gradient;
    }

    public static float DistanceFromTarget(Vector3 target, JointCreator tentacle, float[] angles)
    {
        Vector3 point = ForwardKinematics(tentacle, angles);
        return Vector3.Distance(point, target);
    }

    public static Vector3 ForwardKinematics (JointCreator tentacle, float[] angles)
    {
        ReadOnlyCollection<Joint> joints = tentacle.AllJoints;
        Vector3 previousPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for(int i = 1; i < joints.Count; ++i)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].Axis);
            previousPoint += rotation * joints[i].Offset;
        }
        return previousPoint;
    }
}
