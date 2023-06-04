using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallMonster : MonoBehaviour
{   
    [Header("Line Renderer")]
    [SerializeField] private GameObject eyeLineSight;
    private LineRenderer lr;
    [SerializeField] private Transform[] eyes;
    [SerializeField] private Transform[] heads;

    [Header("Settings")]
    [SerializeField] private float range = 50f;


    private void Update()
    {
        for(int i = 0; i < eyes.Length; i++)
        {
            lr = Instantiate(eyeLineSight, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            lr.transform.parent = eyes[i];
            lr.positionCount = 2;
            lr.SetPosition(0, eyes[i].position);
            lr.SetPosition(1, heads[i].position + (heads[i].forward * range));
            Destroy(lr.gameObject, 0.1f);
        }
    }

}
