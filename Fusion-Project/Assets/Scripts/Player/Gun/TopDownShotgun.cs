using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShotgun : MonoBehaviour
{

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.y - Camera.main.transform.position.y));

        // Calculate the direction from the object to the mouse position
        Vector3 direction = mousePosition - transform.position;
        direction.y = 0f; // Keep the object's rotation only on the Y-axis

        // Rotate the object to face the mouse position
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }
}
