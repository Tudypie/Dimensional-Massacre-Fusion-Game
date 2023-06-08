using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    private Health health;
    private GameObject healLineRenderer;
    public float healAmountPerSecond = 10f;

    void Start()
    {
        health = GameObject.Find("Character2_Reference").GetComponent<Health>();
        healLineRenderer = GameObject.Find("Boss").GetComponent<MutantJump>().healLineRenderer;
        
        LineRenderer lr = Instantiate(healLineRenderer, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, health.transform.position);

    }

}
