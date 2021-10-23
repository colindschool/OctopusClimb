using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    private bool doIK = true;
    private bool doHinge = true;

    [SerializeField] private GameObject target;
    [SerializeField] private List<TentacleJoint> tentacleJoints;
    public List<TentacleJoint> TentacleJoints { get => tentacleJoints; }
    public Transform TargetTransform { get => target.transform; }

    [SerializeField] private Vector2 minAngles;
    [SerializeField] private Vector2 maxAngles;
    [SerializeField] private List<Vector3> axis;

    [SerializeField] private float slowdownDistance = 0.25f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float speedDegrees;
    [SerializeField] private float orientationWeight = 0.5f;
    [SerializeField] private float torsionWeight = 0.5f;
    [SerializeField] private Vector3 torsionPenalty = new Vector3(1, 0, 0);
    public float SlowdownDistance { get => slowdownDistance; }
    public float StopDistance { get => stopDistance; }
    public float SpeedDegrees { get => speedDegrees; }
    public float OrientationWeight { get => orientationWeight; }
    public float TorsionWeight { get => torsionWeight; }
    public Vector3 TorsionPenalty { get => torsionPenalty; }

    private float[] angles;
    public float[] Angles { get => angles; }

    private void Awake()
    {
        SetAngleAndAxis();
        angles = new float[tentacleJoints.Count];
    }

    private void Update()
    {
        if(doIK)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            if (IKManager.ErrorFunction(this, angles) > stopDistance)
            {
                IKManager.ApproachTarget(this);
            }
        }
    }

    private void SetAngleAndAxis()
    {
        for(int i = 0; i < tentacleJoints.Count; ++i)
        {
            float angleRatio = i / (tentacleJoints.Count - 1);
            tentacleJoints[i].minAngle = Mathf.Lerp(minAngles.x, minAngles.y, angleRatio);
            tentacleJoints[i].maxAngle = Mathf.Lerp(maxAngles.x, maxAngles.y, angleRatio);
            tentacleJoints[i].axis = axis[i % axis.Count];
        }
    }
}
