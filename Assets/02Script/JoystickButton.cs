using UnityEngine;

public class JoystickButton
{
    // �����̴� �� ���� ����
    public Vector3 dir;

    // ���̽�ƽ ��� �̹���
    public RectTransform joystickBG;
    // ���̽�ƽ ��ƽ
    public RectTransform stick;

    // ó�� ���콺�� ���� ��ġ
    Vector3 basePos;
    // �巡�� �� ���� ��ġ
    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        // ���̽�ƽ�� ���̰� �����Ѵ�
        joystickBG.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);
    }

    // ó�� ���콺�� ������ ��
    public void OnStickDown()
    {
        // �� ��ġ�� ���������� ���
        basePos = Input.mousePosition;

        // ���̽�ƽ�� �� ��ġ�� �����Ѵ�
        stick.anchoredPosition = basePos;

        // ���̽�ƽ�� ���̰� �����Ѵ�
        joystickBG.gameObject.SetActive(true);
        stick.gameObject.SetActive(true);

        // ������ �ʱ�ȭ�Ѵ�
        dir = Vector3.zero;
    }

    // �巡���ϸ�
    public void OnStickDrag()
    {
        // ���� ��ġ�� �����ϰ�
        currentPos = Input.mousePosition;

        // ���������κ����� �Ÿ��� 80���� �����Ѵ�
        Vector3 v = Vector3.ClampMagnitude(currentPos - basePos, 80f);

        // ��ƽ�� ��ġ�� ���������� v�� ���� ��ġ�� �����Ѵ�
        stick.anchoredPosition = basePos + v;

        // ������ �����Ѵ�
        dir = v.normalized;
    }

    public void OnStickUp()
    {
        // ���̽�ƽ�� ������ �ʰ� �����Ѵ�
        joystickBG.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);

        // ������ �ʱ�ȭ�Ѵ�
        dir = Vector3.zero;
    }
}

