using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{

    [SerializeField] private GameObject laser;
    [SerializeField] private float yOffset = 4f;
    [SerializeField] private float healAmount = 10f;
    private bool isHealing = false;
        
    private Transform bossTransform;
    private MutantJump mutantJump;
    public LineRenderer lr;

    void Start()
    {   
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        mutantJump = bossTransform.GetComponent<MutantJump>();

        lr = Instantiate(laser, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        lr.enabled = true;

    }

    void Update()
    {   

        if(mutantJump.shield.enabled)
        {
            lr.SetPositions(new Vector3[] { transform.position + new Vector3(0f, yOffset, 0f),
            bossTransform.position + new Vector3(0f, 8f, 0f) });
            if(!isHealing)
                InvokeRepeating("Heal", 0f, 1f);
        }    
        else
        {
            Destroy(lr.gameObject);
            Destroy(gameObject);
        }
    }

    void Heal()
    {
        isHealing = true;
        mutantJump.shield.shieldHealth += healAmount;
    }

}
