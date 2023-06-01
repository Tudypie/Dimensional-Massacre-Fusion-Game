using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    private void Update()
    {
        transform.rotation = target.rotation;
    }
}
