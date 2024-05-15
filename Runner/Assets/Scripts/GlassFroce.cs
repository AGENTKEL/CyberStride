using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFroce : MonoBehaviour
{
    public float forceAmount = 5f; // Adjust the force amount as needed
    public float forceAngle = 0f; // Angle in degrees, adjustable in the inspector

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate direction from angle
            Vector2 forceDirection = CalculateDirectionFromAngle(forceAngle);

            // Apply the force in the calculated direction
            rb.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);
        }
    }

    // Converts an angle in degrees to a 2D directional vector
    Vector2 CalculateDirectionFromAngle(float angleInDegrees)
    {
        // Convert degrees to radians and calculate direction
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        return direction;
    }
}
