using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGrenadesThrowing : MonoBehaviour
{   
    [SerializeField] float ThrowForce;
    [SerializeField] ThrowObject threeDthrowObject;
    [SerializeField] ThrowObject twoDthrowObject;
    ThrowObject throwObject;

    private void Update()
    {   
        if(Camera.main.orthographic)
        {
            throwObject = twoDthrowObject;
        }
        else
        {
            throwObject = threeDthrowObject;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (PlayerStats.Instance.playerGrenades.grenades > 0)
            {
                throwObject.throwForce = ThrowForce;
                throwObject.Throw();
                PlayerStats.Instance.playerGrenades.RemoveGrenade();
            }
        }
    }
}
