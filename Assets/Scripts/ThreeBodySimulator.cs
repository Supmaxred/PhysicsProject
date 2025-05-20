using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Body
{
    public Transform transform;
    public float mass;
    public Vector3 velocity;
}

public class ThreeBodySimulator : MonoBehaviour
{
    [SerializeField] private List<Body> bodies;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float speed = 0.01f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        foreach (Body body in bodies)
        {
            body.velocity = Random.insideUnitSphere * 1f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            mainCamera.orthographicSize *= 1.1f;
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            mainCamera.orthographicSize *= 0.9f;

        if (Input.GetKey(KeyCode.F))
        {
            Vector2 pos = new Vector2(0f, 0f);

            foreach (Body body in bodies)
            {
                pos += (Vector2)body.transform.position;
            }

            pos /= bodies.Count;

            mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        Vector3[] accelerations = new Vector3[bodies.Count];

        for (int i = 0; i < bodies.Count; i++)
        {
            Vector3 acceleration = Vector3.zero;
            for (int j = 0; j < bodies.Count; j++)
            {
                if (i == j) continue;

                Vector3 direction = bodies[j].transform.position - bodies[i].transform.position;
                float distanceSqr = direction.sqrMagnitude + Mathf.Epsilon;
                acceleration += gravity * bodies[j].mass * direction.normalized / distanceSqr;
            }
            accelerations[i] = acceleration;
        }

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].velocity += accelerations[i] * speed;
            bodies[i].transform.position += bodies[i].velocity * speed;
        }
    }
}
