using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnife : MonoBehaviour
{

    public Transform KnifePivot; // 나이프 배치의 기준점
    public Transform rightHandMount; // 나이프의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트
    void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

   

    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // 나이프의 기준점 KnifePivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        KnifePivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 오른손의 위치와 회전을 나이프의 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
