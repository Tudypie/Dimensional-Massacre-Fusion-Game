using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    [SerializeField] private Transform pointToFollow;
    void Update()
    {
        transform.position = pointToFollow.position;
    }
}
