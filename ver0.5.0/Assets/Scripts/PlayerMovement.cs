using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMovement : MonoBehaviourPun
{

    public float Speed = 4.5f;
    public float walkSpeed = 4.5f;
    public float runSpeed = 7.5f;
    public float jumpForce = 10f;

    private bool isJump = false;
    private bool isRun = false;

    Vector3 originPos = new Vector3(); // 추락하고 나면 원래 위치로 부활시켜야 하므로 기억하기 위해

    private Animator playerAnimator;
    private AudioSource playerAudioPlayer; // 오디오 플레이어
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    public AudioClip stepSound; // 발자국 클립
    public AudioClip runningStepSound; // 뛰는 발자국 클립


    private void Awake()
    {
        Cursor.visible = false; // 마우스 커서 지우기
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정

        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAudioPlayer = GetComponent<AudioSource>();

    }
    private void FixedUpdate()
    {
        // 로컬 플레이어만 직접 위치를 변경 가능
        if (!photonView.IsMine)
        {
            return;
        }

        Move();
        Jump();


        // 입력값에 따라 애니메이터의 Zmove 파라미터 값을 변경
        playerAnimator.SetFloat("Zmove", playerInput.Zmove);

        // 입력값에 따라 애니메이터의 Xmove 파라미터 값을 변경
        playerAnimator.SetFloat("Xmove", playerInput.Xmove);
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = runSpeed;
            isRun = true;
            playerAnimator.SetBool("isRun", true);
        }
        else
        {
            Speed = walkSpeed;
            isRun = false;
            playerAnimator.SetBool("isRun", false);
        }

        // 만약 플레이어가 멈춰있지 않다면 
        if (playerInput.Zmove != 0f || playerInput.Xmove != 0f)
        {
            // 만약 플레이어오디오가 재생되지 않고 있다면
            if (!playerAudioPlayer.isPlaying)
            {
                // 점프 안 하고 있다면
                if (isJump == false)
                {
                    // 만약 플레이어가 뛰고 있다면
                    if (isRun)
                    {
                        // 뛰는 발소리 재생
                        playerAudioPlayer.PlayOneShot(runningStepSound);
                    }
                    // 그렇지 않다면
                    else
                    {
                        // 발소리 재생
                        playerAudioPlayer.PlayOneShot(stepSound);
                    }
                }
                
            }
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
        if (playerInput.jump && !isJump)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetTrigger("isJump");
            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }

    }


    public void ComeBackHome()
    {
        transform.position = originPos;
    }

}
