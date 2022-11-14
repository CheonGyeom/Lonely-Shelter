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

    Vector3 originPos = new Vector3(); // �߶��ϰ� ���� ���� ��ġ�� ��Ȱ���Ѿ� �ϹǷ� ����ϱ� ����

    private Animator playerAnimator;
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�


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
