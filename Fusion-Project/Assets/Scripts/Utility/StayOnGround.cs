using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnGround : MonoBehaviour
{
    [SerializeField] float yOffset = 1f;
    [SerializeField] float timer = 0.7f;
    private void Update()
    {   
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0f, 100f, 0f), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + yOffset, transform.position.z);
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab)) timer = 0.7f;


    }
}
