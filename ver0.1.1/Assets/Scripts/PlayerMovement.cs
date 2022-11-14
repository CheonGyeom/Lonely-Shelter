using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float Speed = 4.5f;
    public float walkSpeed = 4.5f;
    public float runSpeed = 7.5f;
    public float jumpForce = 10f;

    private bool isJump = false;

    Vector3 originPos = new Vector3(); // 추락하고 나면 원래 위치로 부활시켜야 하므로 기억하기 위해

    private Animator playerAnimator;
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Speed = runSpeed;
        }
        else
        {
            Speed = walkSpeed;
        }
        // 상대적으로 이동할 거리 계산
        Vector3 ZmoveDistance =
            playerInput.Zmove * transform.forward * Speed * Time.deltaTime;
        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + ZmoveDistance);

        // 상대적으로 이동할 거리 계산
        Vector3 XmoveDistance =
            playerInput.Xmove * transform.right * Speed * Time.deltaTime;
        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + XmoveDistance);
    }

    private void Jump()
    {
        if(playerInput.jump && !isJump)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
        
    }

    public void ComeBackHome()
    {
        transform.position = originPos;
    }

}
