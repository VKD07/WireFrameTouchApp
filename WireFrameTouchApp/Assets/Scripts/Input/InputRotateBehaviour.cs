using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRotateBehaviour : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200.0f;
    [Range(0,1)]
    [SerializeField] float rotationDamping = 0.95f; // Adjust this value to control damping
    [SerializeField] Vector2 lastInputPosition;
    [SerializeField] float currentRotationSpeed;

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check for the beginning of a swipe
            if (touch.phase == TouchPhase.Began)
            {
                lastInputPosition = touch.position;
                currentRotationSpeed = 0.0f; // Reset rotation speed
            }
            // Check for the end of a swipe
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the swipe direction
                Vector2 swipeDirection = touch.position - lastInputPosition;

                // Calculate rotation based on swipe direction
                float rotationAmount = -swipeDirection.x * rotationSpeed * Time.deltaTime;

                // Add the rotation to the current speed (dampened)
                currentRotationSpeed += rotationAmount;

                // Dampen the rotation speed
                currentRotationSpeed *= rotationDamping;

                // Rotate the object around its up axis (Y-axis)
                transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime, Space.World);

                // Update the last input position
                lastInputPosition = touch.position;
            }
        }
        // Check for mouse input
        else if (Input.GetMouseButtonDown(0))
        {
            lastInputPosition = Input.mousePosition;
            currentRotationSpeed = 0.0f; // Reset rotation speed
        }
        else if (Input.GetMouseButton(0))
        {
            // Calculate the mouse swipe direction
            Vector2 mouseSwipeDirection = (Vector2)Input.mousePosition - lastInputPosition;

            // Calculate rotation based on mouse swipe direction
            float rotationAmount = -mouseSwipeDirection.x * rotationSpeed * Time.deltaTime;

            // Add the rotation to the current speed (dampened)
            currentRotationSpeed += rotationAmount;

            // Dampen the rotation speed
            currentRotationSpeed *= rotationDamping;

            // Rotate the object around its up axis (Y-axis)
            transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime, Space.World);

            // Update the last input position
            lastInputPosition = Input.mousePosition;
        }
    }
}