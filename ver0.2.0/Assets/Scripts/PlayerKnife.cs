using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnife : MonoBehaviour
{

    public Transform KnifePivot; // ������ ��ġ�� ������
    public Transform rightHandMount; // �������� ������ ������, �������� ��ġ�� ����

    private PlayerInput playerInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ
    void Start()
    {
        // ����� ������Ʈ���� ��������
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

   

    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // �������� ������ KnifePivot�� 3D ���� ������ �Ȳ�ġ ��ġ�� �̵�
        KnifePivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK�� ����Ͽ� �������� ��ġ�� ȸ���� �������� �����̿� �����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
