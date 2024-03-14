using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idel,
    Attack,
    Dash
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private FixedJoystick normalAttack;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dashDistance;
    private PlayerAnimationController anim;
    private Button dash;
    private bool isControll;
    private State state;
    

    private Vector3 direction;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }

        if(!GameObject.Find("Floating Joystick").TryGetComponent< FloatingJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("NormalAttack").TryGetComponent<FixedJoystick>(out normalAttack))
        {
            Debug.Log("PlayerController - Awake - FixedJoystic");
        }

        if (!GameObject.Find("Dash").TryGetComponent<Button>(out dash))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        else
        {
            dash.onClick.AddListener(Dash);
        }
        isControll = true;
        state = State.Idel;
    }

    private void ChangeState(State state)
    {
        if(this.state == state)
        {
            return;
        }
        switch (this.state)
        {
            case State.Idel:
                break;
            case State.Attack: 
                break;
            case State.Dash: 
                break;
        }
        this.state = state;
        switch(this.state)
        {
            case State.Idel:
                break;
            case State.Attack:
                anim.NormalAttack();
                StartCoroutine(normalAttackDelay());
                break;
            case State.Dash:
                anim.Dash();
                transform.LookAt(transform.position + direction);
                StartCoroutine(dashDelay());
                break;
        }
    }

    private void Update()
    {
        if (isControll)
        {
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            Vector3 look = Vector3.forward * normalAttack.Vertical + Vector3.right * normalAttack.Horizontal;

            if(direction != Vector3.zero)
            {
                transform.LookAt(transform.position + direction);
                rig.MovePosition(transform.position + direction * speed * Time.deltaTime);
                anim.SetRun(true);
            }
            else
            {
                anim.SetRun(false);
            }

            if (look != Vector3.zero)
            {
                transform.LookAt(transform.position + look);
                ChangeState(State.Attack);
            }
        }
    }

    private IEnumerator normalAttackDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.8f);
        ChangeState(State.Idel);
    }

    private void Dash()
    {
        ChangeState(State.Dash);
    }

    private IEnumerator dashDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        rig.MovePosition(transform.position + transform.forward * dashDistance * Time.deltaTime);
        yield return YieldInstructionCache.WaitForSeconds(0.8f);
        ChangeState(State.Idel);
    }
}
