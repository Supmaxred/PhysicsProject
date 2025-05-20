using TMPro;
using UnityEngine;

public class CubeCollisions : MonoBehaviour
{
    [SerializeField] private Transform cube1;
    [SerializeField] private Transform cube2;
    [SerializeField] private float mass1 = 1f;
    [SerializeField] private float mass2 = 10000f;
    [SerializeField] private float size1 = 1f;
    [SerializeField] private float size2 = 1f;
    [SerializeField] private float velocity1 = 0f;
    [SerializeField] private float velocity2 = -1f;
    private int collisionCount = 0;
    [SerializeField] public float timeScale = 1f;
    [SerializeField] public int res = 100;

    [SerializeField] private TMP_Text collisionText;

    private bool cubesAreTouching = false;
    private bool wallIsTouching = false;

    void Update()
    {
        float dt = Time.deltaTime * timeScale / res;

        for (int i = 0; i < res; i++)
        {
            cube1.position += new Vector3(velocity1 * dt, 0, 0);
            cube2.position += new Vector3(velocity2 * dt, 0, 0);

            if (cube1.position.x + size1 >= cube2.position.x)
            {
                if (!cubesAreTouching)
                {
                    float newV1 = GetNewVelocity(velocity1, mass1, velocity2, mass2);
                    float newV2 = GetNewVelocity(velocity2, mass2, velocity1, mass1);

                    velocity1 = newV1;
                    velocity2 = newV2;

                    collisionCount++;
                    cubesAreTouching = true;
                }
            }
            else
            {
                cubesAreTouching = false;
            }

            if (cube1.position.x <= -5f)
            {
                if (!wallIsTouching)
                {
                    cube1.position = new Vector3(-5f, cube1.position.y, cube1.position.z);
                    velocity1 *= -1;
                    collisionCount++;
                    wallIsTouching = true;
                }
            }
            else
            {
                wallIsTouching = false;
            }
        }

        collisionText.text = $"{collisionCount}";
    }

    float GetNewVelocity(float v1, float m1, float v2, float m2) =>
        ((m1 - m2) * v1 + 2 * m2 * v2) / (m1 + m2);
}
