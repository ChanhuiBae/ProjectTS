using UnityEngine;

public class JoystickButton
{
    // 움직이는 데 사용될 방향
    public Vector3 dir;

    // 조이스틱 배경 이미지
    public RectTransform joystickBG;
    // 조이스틱 스틱
    public RectTransform stick;

    // 처음 마우스를 누른 위치
    Vector3 basePos;
    // 드래그 시 현재 위치
    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        // 조이스틱을 보이게 설정한다
        joystickBG.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);
    }

    // 처음 마우스를 눌렀을 때
    public void OnStickDown()
    {
        // 그 위치를 기준점으로 잡고
        basePos = Input.mousePosition;

        // 조이스틱을 그 위치로 지정한다
        stick.anchoredPosition = basePos;

        // 조이스틱을 보이게 설정한다
        joystickBG.gameObject.SetActive(true);
        stick.gameObject.SetActive(true);

        // 방향을 초기화한다
        dir = Vector3.zero;
    }

    // 드래그하면
    public void OnStickDrag()
    {
        // 현재 위치를 저장하고
        currentPos = Input.mousePosition;

        // 기준점으로부터의 거리를 80으로 제한한다
        Vector3 v = Vector3.ClampMagnitude(currentPos - basePos, 80f);

        // 스틱의 위치를 기준점에서 v를 더한 위치로 지정한다
        stick.anchoredPosition = basePos + v;

        // 뱡항을 갱신한다
        dir = v.normalized;
    }

    public void OnStickUp()
    {
        // 조이스틱을 보이지 않게 설정한다
        joystickBG.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);

        // 방향을 초기화한다
        dir = Vector3.zero;
    }
}

