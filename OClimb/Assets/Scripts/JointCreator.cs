using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> allBones;

    [ContextMenu("CreateJoints")]
    private void CreateJoints()
    {
        allBones = new List<GameObject>();


        GetBone(transform);

        Rigidbody previousRB = null;

        for (int i =0; i < allBones.Count; ++i)
        {
            if (allBones[i].GetComponent<HingeJoint>() != null)
                Destroy(allBones[i].GetComponent<HingeJoint>());
            HingeJoint joint = allBones[i].AddComponent<HingeJoint>();
            Rigidbody rigidbody = allBones[i].GetComponent<Rigidbody>();
            joint.axis = new Vector3(0, 1, 0);
            // Broken do not use
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;

            if (previousRB != null)
                joint.connectedBody = previousRB;

            previousRB = rigidbody;
        }
    }

    private void GetBone(Transform bone)
    {
        allBones.Add(bone.gameObject);

        foreach(Transform child in bone)
        {
            GetBone(child);
        }
    }
}
