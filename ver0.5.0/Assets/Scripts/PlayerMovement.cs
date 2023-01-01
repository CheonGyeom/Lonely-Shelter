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

    Vector3 originPos = new Vector3(); // �߶��ϰ� ���� ���� ��ġ�� ��Ȱ���Ѿ� �ϹǷ� ����ϱ� ����

    private Animator playerAnimator;
    private AudioSource playerAudioPlayer; // ����� �÷��̾�
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    public AudioClip stepSound; // ���ڱ� Ŭ��
    public AudioClip runningStepSound; // �ٴ� ���ڱ� Ŭ��


    private void Awake()
    {
        Cursor.visible = false; // ���콺 Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ����

        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAudioPlayer = GetComponent<AudioSource>();

    }
    private void FixedUpdate()
    {
        // ���� �÷��̾ ���� ��ġ�� ���� ����
        if (!photonView.IsMine)
        {
            return;
        }

        Move();
        Jump();


        // �Է°��� ���� �ִϸ������� Zmove �Ķ���� ���� ����
        playerAnimator.SetFloat("Zmove", playerInput.Zmove);

        // �Է°��� ���� �ִϸ������� Xmove �Ķ���� ���� ����
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

        // ���� �÷��̾ �������� �ʴٸ� 
        if (playerInput.Zmove != 0f || playerInput.Xmove != 0f)
        {
            // ���� �÷��̾������� ������� �ʰ� �ִٸ�
            if (!playerAudioPlayer.isPlaying)
            {
                // ���� �� �ϰ� �ִٸ�
                if (isJump == false)
                {
                    // ���� �÷��̾ �ٰ� �ִٸ�
                    if (isRun)
                    {
                        // �ٴ� �߼Ҹ� ���
                        playerAudioPlayer.PlayOneShot(runningStepSound);
                    }
                    // �׷��� �ʴٸ�
                    else
                    {
                        // �߼Ҹ� ���
                        playerAudioPlayer.PlayOneShot(stepSound);
                    }
                }
                
            }
        }

        // ��������� �̵��� �Ÿ� ���
        Vector3 ZmoveDistance =
            playerInput.Zmove * transform.forward * Speed * Time.deltaTime;
        // ������ٵ� ���� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + ZmoveDistance);

        // ��������� �̵��� �Ÿ� ���
        Vector3 XmoveDistance =
            playerInput.Xmove * transform.right * Speed * Time.deltaTime;
        // ������ٵ� ���� ���� ������Ʈ ��ġ ����
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
