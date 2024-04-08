using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doller : MonoBehaviour
{
    [SerializeField] private float centerX;
    [SerializeField] private float centerY;
    [SerializeField] private float radius;

    void Start()
    {
        Vector2 randomPosition = GetRandomPositionInCircle();
        Debug.Log("Random Position: " + randomPosition);
    }

    Vector2 GetRandomPositionInCircle()
    {
        float angle = Random.Range(0, Mathf.PI * 2);

        // ������ ������ ���� �ﰢ�Լ� ���� ����Ͽ� ���� ��ġ ���
        float x = centerX + Mathf.Cos(angle) * radius;
        float y = centerY + Mathf.Sin(angle) * radius;

        return new Vector2(x, y);
    }

}
