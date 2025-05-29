using UnityEngine;

public class PlayerFloorChanger : MonoBehaviour
{
    public float topY = 1.2f;         // Y верхнего этажа
    public float bottomY = -1.5f;     // Y нижнего этажа
    public float leftEdgeX = -7.5f;   // Левая граница (X)
    public float rightEdgeX = 9f;     // Правая граница (X)

    void Update()
    {
        Vector3 pos = transform.position;

        // Верхний этаж → вправо → низ слева
        if (pos.y > 0.5f && pos.x > rightEdgeX)
        {
            transform.position = new Vector3(leftEdgeX, bottomY, pos.z);
        }

        // Верхний этаж → влево → низ справа
        else if (pos.y > 0.5f && pos.x < leftEdgeX)
        {
            transform.position = new Vector3(rightEdgeX, bottomY, pos.z);
        }

        // Нижний этаж → вправо → верх слева
        else if (pos.y < 0 && pos.x > rightEdgeX)
        {
            transform.position = new Vector3(leftEdgeX, topY, pos.z);
        }

        // Нижний этаж → влево → верх справа
        else if (pos.y < 0 && pos.x < leftEdgeX)
        {
            transform.position = new Vector3(rightEdgeX, topY, pos.z);
        }
    }
}

