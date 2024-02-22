using UnityEngine;
using BoardGame.Util;

public class DiceGague : MonoBehaviour
{
    private int GRADE;
    [SerializeField] private Transform center; 
    [SerializeField] private Transform parent;
    [SerializeField] private Transform gagueTrm;

    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform visual;

    [SerializeField] private GameObject graduation;

    private bool isLeftMove;
    private float curAngle;
    private float radius;
    [SerializeField]private float angle;

    private void Awake()
    {
        GRADE = GameManager.Instance.GRADE;
    }

    private void Start()
    {
        radius = Vector2.Distance(gagueTrm.position, center.position);

        Vector3 directions = gagueTrm.position - center.position;
        //angle = 90f - Mathf.Atan2(directions.y, directions.x);

        for (int i = 1; i < GRADE; i++)
        {
            GameObject newObj = Instantiate(graduation, parent);

            newObj.transform.position = GetPos(-angle + (angle * 2 / GRADE) * i);
            newObj.transform.up = GetDirection(-angle + (angle * 2 / GRADE) * i);
            newObj.SetActive(true);
        }
    }

    private void Update()
    {
        curAngle += Time.deltaTime * speed;

        if (curAngle > angle || curAngle < -angle)
        {
            ChangeDirection(!isLeftMove);
        }

        visual.position = GetPos(curAngle);
        visual.up = GetDirection(curAngle);
    }

    // ���� angle�� ����
    private Vector3 GetDirection(float angle)
    {
        Vector3 direction = Vector3.zero;
        direction.x = Mathf.Sin(angle * Mathf.Deg2Rad);
        direction.y = Mathf.Cos(angle * Mathf.Deg2Rad);

        return direction;
    }

    // ���� angle�� ��ġ
    private Vector3 GetPos(float angle)
    {
        return center.position + GetDirection(angle) * radius;
    }

    // ���� ������ �ٲٴ� �Լ�
    private void ChangeDirection(bool isLeftMove)
    {
        this.isLeftMove = isLeftMove;

        if (isLeftMove)
            curAngle = angle;
        else
            curAngle = -angle;

        speed *= -1;
    }

    // ���(�ܰ�) �Ǵ� �Լ�
    public int CaculateGrade()
    {
        float rate = (curAngle + angle) / (angle * 2f);
        // -angle���� angle���� curAngle�� ����
        int grade = (int)(rate / (1 / (float)GRADE)) + 1;

        return grade;
    }
}