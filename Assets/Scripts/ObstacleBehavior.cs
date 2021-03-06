using UnityEngine;
public class ObstacleBehavior : MonoBehaviour
{
    int direction = 1;
    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x >= 1.33 && direction == 1)
            direction = -1;
        else if (pos.x <= -1.2 && direction == -1)
            direction = 1;

        transform.position = new Vector3(pos.x + Time.deltaTime * direction, pos.y, pos.z);
    }
}