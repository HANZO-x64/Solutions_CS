/*
    Script task:
    Shows the player's path along a bezier curve.
*/
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints; // Array of points.
    private Vector2 gizmosPosition;                     // Bezier arc.

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            // Bezier formula:
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.1f);
        }

        // Drawing a straight line:
        // 1. Climb.
        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
            new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

        // 2. Descent.
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
            new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }
}
