using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private VariableJoystick joystick;
    [SerializeField]
    private float speed;
    private PlayerAnimationController anim;
    private Button normalAttack;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }

        if(!GameObject.Find("Variable Joystick").TryGetComponent<VariableJoystick>(out joystick))
        {
            Debug.Log("PlayerController - Awake - VariableJoystick");
        }
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("NormalAttack").TryGetComponent<Button>(out normalAttack))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        else
        {
            normalAttack.onClick.AddListener(NormalAttack);
        }
    }

    private void NormalAttack()
    {
        anim.NormalAttack();
    }

    private void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;

        if (direction != Vector3.zero)
        {
            transform.LookAt(transform.position + direction);
            anim.SetRun(true);
        }
        else
        {
            anim.SetRun(false);
        }

        
    }
}
