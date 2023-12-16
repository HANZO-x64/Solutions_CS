/*
    Script task:
    Move the player along a specific path using the bezier formula.
*/
using System.Collections;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private Transform[] routes; // Array of points.
    private int routeToGo;                       // Variable for introducing new motion arcs from other objects.
    private float tParam;                        // Parameter T for the Bezier formula.
    private bool coroutineAllowed;               // Start of curutina.

    [Header("Search for a player")]
    private GameObject Capybara;         // -----------------
    private Rigidbody2D rbCapybara;      // Player detection.
    private Collider2D colliderCapybara; // -----------------
    private Vector2 capybaraPosition;   
    public float speedModifier = 2.5f;   



    private void Start()
    {
        // Basics settings:
        routeToGo = 0;
        tParam = 0;

        // Check player:
        Capybara = GameObject.Find("Player");
        rbCapybara = Capybara.GetComponent<Rigidbody2D>();
        colliderCapybara = Capybara.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (coroutineAllowed)
            StartCoroutine(GoByTheRoute(routeToGo));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Start coroutine..
        if (collision.gameObject.tag == "Player")
        {
            coroutineAllowed = true;

            // Freez Y and Z:
            rbCapybara.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        // The player will go through a water collider.
        colliderCapybara.enabled = false;

        // Pos points:
        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            // Formula bezier
            capybaraPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            Capybara.transform.position = capybaraPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        // The player can interact with water
        colliderCapybara.enabled = true;

        // If a player has walked along an arc and stood on another object,
        // then he will begin to pass a new arc.
        if (routeToGo > routes.Length - 1)
            routeToGo = 0;

        // End freeze:
        rbCapybara.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
    }
}
