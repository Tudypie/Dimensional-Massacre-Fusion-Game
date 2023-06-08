using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour
{
    Transform player;
    
    public float minDistanceFromPlayer = 300f;
    
    float timer = 1.6f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        minDistanceFromPlayer = 150f;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < minDistanceFromPlayer)
        {
            GetComponent<Light>().enabled = true;
        }
        else
        {
            GetComponent<Light>().enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(GetComponent<FOVDamage>() != null)
            {
                GetComponent<FOVDamage>().enabled = false;
                Invoke("ActivateFOVDamage", timer);
            }
        }
    }

    void ActivateFOVDamage()
    {
        GetComponent<FOVDamage>().enabled = true;
    }
}
