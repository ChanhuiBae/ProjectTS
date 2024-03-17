using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Attack,
    Roll
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
    private Vector3 look;
    private float lookAngle;

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
            dash.onClick.AddListener(Roll);
        }
        isControll = true;
        state = State.Idle;
    }

    private void ChangeState(State state)
    {
        if(this.state == state)
        {
            return;
        }
        switch (this.state)
        {
            case State.Idle:
                break;
            case State.Attack: 
                break;
            case State.Roll: 
                break;
        }
        this.state = state;
        switch(this.state)
        {
            case State.Idle:
                anim.SowrdAttack(false);
                break;
            case State.Attack:
                break;
            case State.Roll:
                anim.Roll(true);
                transform.LookAt(transform.position + direction);
                anim.MoveDir(direction.x, direction.z);
                StartCoroutine(RollDelay());
                break;
        }
    }

    private void Update()
    {
        if (isControll)
        {
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            look = Vector3.forward * normalAttack.Vertical + Vector3.right * normalAttack.Horizontal;

            if(direction != Vector3.zero && state != State.Roll)
            {
                rig.MovePosition(transform.position + direction * speed * Time.deltaTime);
                anim.MeleeMove(true);
                float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
                if (Mathf.Abs(angle) > 90)
                {
                    anim.MoveDir(-direction.x, -direction.z);
                }
                else
                {
                    anim.MoveDir(direction.x, direction.z);
                }
            }
            else
            {
                anim.MeleeMove(false);
            }

            if (look != Vector3.zero)
            {
                Quaternion current = transform.rotation;
                transform.LookAt(transform.position + look);
                lookAngle = Quaternion.Angle(transform.rotation, current);
                if(lookAngle > 0)
                {
                    anim.TurnLeft(true);
                    StartCoroutine(TurnDelay());
                }
                else
                {
                    anim.TurnRight(true);
                    StartCoroutine(TurnDelay());
                }
                if (Mathf.Abs(lookAngle) < 0.01f)
                {
                    anim.TurnLeft(false);
                    anim.TurnRight(false);
                    StartCoroutine(MeleeAttackDelay());
                }
            }
        }
    }

    private IEnumerator MeleeAttackDelay()
    {
        while (look != Vector3.zero && lookAngle < 0.01f)
        {
            anim.SowrdAttack(true);
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
        }
        anim.TurnLeft(false);
        anim.TurnRight(false);
        anim.SowrdAttack(false);
    }

    private void Roll()
    {
        if(isControll && state != State.Roll)
            ChangeState(State.Roll);
    }

    private IEnumerator RollDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        rig.MovePosition(transform.position + transform.forward * dashDistance * Time.deltaTime);
        anim.Roll(false);
        ChangeState(State.Idle);
    }

    private IEnumerator TurnDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        anim.TurnLeft(false);
        anim.TurnRight(false);
    }
}
