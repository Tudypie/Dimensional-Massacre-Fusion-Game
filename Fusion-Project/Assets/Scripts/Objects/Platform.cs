using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerStay(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(SetParentToNull(other));
    }

    IEnumerator SetParentToNull(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.transform.parent = null;
    }
}
