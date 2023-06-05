using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMonster : MonoBehaviour
{   
    private Animator anim;
    
    public FOVDamage[] fovDamage;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Die()
    {
        foreach(FOVDamage fov in fovDamage)
        {
            fov.fovEnabled = true;
        }
       
        anim.Play("Death");
        PlayerStats.Instance.AddKill();
        Destroy(gameObject, 1.5f);
    }

    public void TakeDamage()
    {
        anim.Play("GetHit");
    }

}
