using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private DynamicJoystick joystick;
    [SerializeField]
    private float speed;
    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }

        if(!GameObject.Find("Dynamic Joystick").TryGetComponent<DynamicJoystick>(out joystick))
        {
            Debug.Log("PlayerController - Awake - Joystick");
        }
    }

    private void Update()
    {

        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;
        //rig.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Debug.Log(touch.phase);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
                transform.position = Vector3.Lerp(transform.position, new Vector3(touchPosition.x, transform.position.y, touchPosition.z), Time.deltaTime * 10f);

            }
        }*/
    }
}
