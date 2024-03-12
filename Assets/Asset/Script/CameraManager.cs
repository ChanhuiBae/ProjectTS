using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private DynamicJoystick joystick;
    [SerializeField]
    private float speed;

    private void Awake()
    {
        if (!GameObject.Find("Dynamic Joystick").TryGetComponent<DynamicJoystick>(out joystick))
        {
            Debug.Log("CameraManager - Awake - Joystick");
        }
    }

    private void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;
    }
}
