using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class JointCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> allBones;
    [SerializeField] private List<Joint> allJoints;
    [SerializeField] private GameObject targetPoint;
    private float[] angles;

    public ReadOnlyCollection<Joint> AllJoints { get => allJoints.AsReadOnly(); }

    private void Awake()
    {
        angles = new float[allJoints.Count];
        for(int i = 0; i < allBones.Count; ++i)
        {
            Rigidbody boneRigidBody = allBones[i].GetComponent<Rigidbody>();
            if (boneRigidBody != null)
                Destroy(boneRigidBody);
        }
    }

    private void Update()
    {
        if(targetPoint != null)
        {
            IKManager.InverseKinematics(targetPoint.transform.position, this, angles);
        }
    }

    [ContextMenu("CreateJoints")]
    private void CreateJoints()
    {
        allBones = new List<GameObject>();
        allJoints = new List<Joint>();

        GetBone(transform);

        for(int i = 0; i < allBones.Count; ++i)
        {
            Joint boneJoint = allBones[i].GetComponent<Joint>();
            if (boneJoint == null)
            {
                boneJoint = allBones[i].AddComponent<Joint>();
            }
            allJoints.Add(boneJoint);
        }
        angles = new float[allJoints.Count];
    }

    private void GetBone(Transform bone)
    {
        allBones.Add(bone.gameObject);

        foreach (Transform child in bone)
        {
            GetBone(child);
        }
    }
}