using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : MonoBehaviour
{
    [SerializeField] private uint CountOfLines;
    [SerializeField] private int Resolution;
    [SerializeField] private float Length;
    [SerializeField] private GameObject Line;
    [SerializeField] private Transform Minus;
    [SerializeField] private Transform PlusP;
    [SerializeField] private float MinusMass;
    [SerializeField] private float PlusMass;
    [SerializeField] private float MyMass;
    [SerializeField] private float MassMultiplier;

    private List<LineRenderer> lines;
    private int currentHold = 0;
    private Vector2 mouseWorldPos;
    private Camera mainCamera;
    private Transform[] transforms = new Transform[3];

    void Start()
    {
        lines = new List<LineRenderer>();
        mainCamera = Camera.main;

        transforms[0] = transform;
        transforms[1] = PlusP;
        transforms[2] = Minus;

        for (uint i = 0; i < CountOfLines; i++)
        {
            float f = (float)i / (float)CountOfLines * Mathf.PI * 2f;
            Vector3 pos = new Vector3(Mathf.Sin(f) * 0.5f, Mathf.Cos(f) * 0.5f, 0);
            GameObject line = Instantiate(Line, pos, Quaternion.identity, transform);
            lines.Add(line.GetComponent<LineRenderer>());
            line.GetComponent<LineRenderer>().positionCount = Resolution;
        }
    }
    

    private void ProccessInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            mainCamera.orthographicSize *= 1.1f;
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            mainCamera.orthographicSize *= 0.9f;

        if (currentHold == 0)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    currentHold = i;
                    break;
                }
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Alpha0 + currentHold))
                currentHold = 0;
        }

        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        ProccessInput();

        if (currentHold > 0)
        {
            Vector3 pos = mouseWorldPos;
            pos.z = 0;

            transforms[currentHold - 1].position = pos;
        }
    }

    private void FixedUpdate()
    {
        float delta = Length / Resolution;

        for (int i = 0; i < CountOfLines; i++)
        {
            Vector3 pos = lines[i].transform.position;
            Vector3 vel = Vector3.Normalize(pos - transform.position);

            for (int j = 0; j < Resolution; j++)
            {
                ApplyGravity(ref vel, pos, Minus.transform.position, MassMultiplier, MinusMass, delta, true);
                ApplyGravity(ref vel, pos, PlusP.transform.position, MassMultiplier, PlusMass, delta, false);
                ApplyGravity(ref vel, pos, transform.position, MassMultiplier, MyMass, delta, false);

                vel = vel.normalized;
                pos += vel * delta;

                if ((Minus.transform.position - pos).magnitude < 0.5f)
                {
                    pos = Minus.transform.position;
                }

                lines[i].SetPosition(j, pos);
            }
        }
    }

    private void ApplyGravity(ref Vector3 velocity, Vector3 position, Vector3 target, float multiplier, float mass, float delta, bool isPlus)
    {
        Vector3 dir = (target - position).normalized;
        float dis = Mathf.Clamp((target - position).magnitude, 1f, 9999f);
        float force = multiplier * mass / dis;
        velocity += (isPlus ? 1 : -1) * dir * force * delta;
    }
}
