using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private VariableJoystick joystick;
    private FixedJoystick normalAttack;
    [SerializeField]
    private float speed;
    private PlayerAnimationController anim;
    private bool attackflag;

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
        if (!GameObject.Find("NormalAttack").TryGetComponent<FixedJoystick>(out normalAttack))
        {
            Debug.Log("PlayerController - Awake - FixedJoystick");
        }
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }
        attackflag = false;
    }

    private void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;
        float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
        if (angle > 90 || angle < -90)
        {
            anim.SetX(-direction.x);
            anim.SetY(-direction.z);
        }
        else
        {
            anim.SetX(direction.x);
            anim.SetY(direction.z);
        }
 
        Vector3 look = Vector3.forward * normalAttack.Vertical + Vector3.right * normalAttack.Horizontal;
        if (look != Vector3.zero)
        {
            transform.LookAt(transform.position + look);
            attackflag = true;
        }
        else
        {
            if(attackflag)
            {
                anim.NormalAttack();
                attackflag = false;
            }
        }
    }
}
