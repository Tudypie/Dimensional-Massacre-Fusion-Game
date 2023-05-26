using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangePosition : MonoBehaviour
{   
    public Transform Player;
    public void ChangePosition(Transform target)
    {
        Player.transform.position = target.position;
    }
}
