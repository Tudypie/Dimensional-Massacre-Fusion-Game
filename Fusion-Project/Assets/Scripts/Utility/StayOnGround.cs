using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class StayOnGround : MonoBehaviour
{
    [SerializeField] private float minDistanceFromPlayer;
    [SerializeField] float yOffset = 1f;
    [SerializeField] float startTimer = 1.4f;
    [SerializeField] bool keepOriginalPos = false;
    public float initialYPos;
    public float timer = 0f;

    Component component;

    private void Start()
    {
        minDistanceFromPlayer = 300f;
    }

    private void Update()
    {  
        /*if(CalculateDistance() > minDistanceFromPlayer)
        {   
            if(SwitchPerspective.Instance.isTopDown)
            {
                if(GetComponent<Rigidbody>() != null)
                {
                    DisableRigidbody();
                }
            }
            else
            {
                if(GetComponent<Rigidbody>() != null)
                {
                    ActivateRigidbody();
                }
            }
            return;
        } */

        RaycastHit aboveHit;
        if (Physics.Raycast(transform.position, Vector3.up, out aboveHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            //Debug.DrawRay(transform.position, Vector3.up * 100f, Color.red, 5f);
            if(!SwitchPerspective.Instance.isTopDown && !SwitchPerspective.Instance.lerpTransitionInProgress)
            {
                keepOriginalPos = true;
                initialYPos = transform.position.y;
            }

        }
        else
        {
            if(!SwitchPerspective.Instance.isTopDown && !SwitchPerspective.Instance.lerpTransitionInProgress)
            {
                keepOriginalPos = false;
            }
        }

        if(timer > 0f)
        {
            if(keepOriginalPos && !SwitchPerspective.Instance.isTopDown)
            {
                transform.position = new Vector3(transform.position.x, initialYPos, transform.position.z);
                timer = 0;  
                return;
            }

            timer -= Time.deltaTime;

            Debug.Log("Below hit");
            RaycastHit belowHit;
            if (Physics.Raycast(transform.position + new Vector3(0f, 100f, 0f), Vector3.down, out belowHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                transform.position = new Vector3(transform.position.x, belowHit.point.y + yOffset, transform.position.z);
            }
            
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab) && SwitchPerspective.Instance.enabled) 
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

    private float CalculateDistance()
    {   
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 playerPosition2D = new Vector2(playerPosition.x, playerPosition.z);
        Vector2 targetPosition2D = new Vector2(transform.position.x, transform.position.z);

        float distance = Vector2.Distance(playerPosition2D, targetPosition2D);
        return distance;
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
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void DisableRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
