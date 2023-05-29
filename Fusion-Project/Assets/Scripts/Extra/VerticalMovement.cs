using UnityEngine;
using UnityEngine.SceneManagement;

public class VerticalMovement : MonoBehaviour
{
    public float speed = 5f; 
    public float duration = 3f; 
    public float startAfter = 3f;

    private float initialY; 
    private float elapsedTime; 

    private void Start()
    {
        initialY = transform.position.y; 
        elapsedTime = 0f; 
    }

    private void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        
        if(startAfter > 0f)
        {
            startAfter -= Time.deltaTime;
            return;
        }

        if (elapsedTime < duration)
        {
            float newY = initialY + (speed * elapsedTime);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            elapsedTime += Time.deltaTime;
        }
    }
}
