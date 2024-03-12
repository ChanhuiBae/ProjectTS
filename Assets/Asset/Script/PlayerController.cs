using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 movePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
             transform.position = Vector3.Lerp(transform.position, new Vector3(movePos.x, transform.position.y, movePos.z), Time.deltaTime * 100f);

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Debug.Log(touch.phase);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
                transform.position = Vector3.Lerp(transform.position, new Vector3(touchPosition.x, transform.position.y, touchPosition.z), Time.deltaTime * 10f);

            }
        }
    }
}
