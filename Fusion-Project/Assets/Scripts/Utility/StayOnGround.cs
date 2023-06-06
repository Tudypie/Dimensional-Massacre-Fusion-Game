using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class StayOnGround : MonoBehaviour
{
    [SerializeField] float yOffset = 1f;
    [SerializeField] float startTimer = 1.3f;
    private float timer = 0f;

    Component component;

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

        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            timer = startTimer;

            if(GetComponent<NavMeshAgent>() != null)
            {
                component = GetComponent<NavMeshAgent>();
                DisableComponent();
                Invoke("ActivateComponent", startTimer);
            }

            if(GetComponent<Rigidbody>() != null)
            {
                DisableRigidbody();
                Invoke("ActivateRigidbody", startTimer);
            }
        }

    }

    private void ActivateComponent()
    {
        PropertyInfo enabledProperty = component.GetType().GetProperty("enabled");
        if (enabledProperty != null && enabledProperty.PropertyType == typeof(bool))
        {
            enabledProperty.SetValue(component, true, null);
        }
    }

    private void DisableComponent()
    {
        PropertyInfo enabledProperty = component.GetType().GetProperty("enabled");
        if (enabledProperty != null && enabledProperty.PropertyType == typeof(bool))
        {
            enabledProperty.SetValue(component, false, null);
        }
    }

    private void ActivateRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void DisableRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
