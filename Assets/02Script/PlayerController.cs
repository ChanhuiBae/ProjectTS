using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private DynamicJoystick joystick;
    private FixedJoystick normalAttack;
    private bool normalAttackFlag;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dashDistance;
    private PlayerAnimationController anim;
    private Button dash;
    private bool isDash;
    private bool isControll;

    private Vector3 direction;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }

        if(!GameObject.Find("Dynamic Joystick").TryGetComponent< DynamicJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Dynamic Joystick");
        }
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("NormalAttack").TryGetComponent<FixedJoystick>(out normalAttack))
        {
            Debug.Log("PlayerController - Awake - FixedJoystic");
        }
        normalAttackFlag = false;

        if (!GameObject.Find("Dash").TryGetComponent<Button>(out dash))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        else
        {
            dash.onClick.AddListener(Dash);
            isDash = false;
        }
        isControll = true;
    }

    private void NormalAttack()
    {
        anim.NormalAttack();
    }

    private void Dash()
    {
        isDash = true;
        anim.Dash();
        transform.LookAt(transform.position + direction);
        transform.position += transform.forward * dashDistance * Time.deltaTime;
        isDash = false;
    }

    private void Update()
    {
        direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        Vector3 look = Vector3.forward * normalAttack.Vertical + Vector3.right * normalAttack.Horizontal;

        if (isControll && !isDash)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        if(Quaternion.Angle(transform.rotation, Quaternion.identity) > 90 || Quaternion.Angle(transform.rotation, Quaternion.identity) < -90)
        {
            anim.SetX(-direction.x);
            anim.SetY(-direction.z);
        }
        else
        {
            anim.SetX(direction.x);
            anim.SetY(direction.z);
        }

        
        if (look != Vector3.zero)
        {
            transform.LookAt(transform.position + look);
            normalAttackFlag = true;
        }
        else
        {
            if (normalAttackFlag)
            {
                anim.NormalAttack();
                StartCoroutine(normalAttackDelay());
                normalAttackFlag = false;
            }
        }
    }

    private IEnumerator normalAttackDelay()
    {
        isControll = false;
        yield return YieldInstructionCache.WaitForSeconds(1);
        isControll = true;
    }
}
