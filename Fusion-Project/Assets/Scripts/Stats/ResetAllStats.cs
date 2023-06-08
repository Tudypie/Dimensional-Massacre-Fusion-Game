using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAllStats : MonoBehaviour
{
    
    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

}
