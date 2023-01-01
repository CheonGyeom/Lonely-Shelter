using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // 플레이어의 애니메이터
    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private MouseLook mouseLook; // 플레이어 화면 돌리기 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
    private AudioSource  playerAudioPlayer; // 플레이어 소리 재생기
    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathClip; // 사망시 재생할 사운드
    public AudioClip hitSound; // 공격당할시 재생할 사운드
    public Rigidbody playerRigidbody; // 플레이어 리지드 바디

    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAudioPlayer = GetComponent<AudioSource>();
        mouseLook = GetComponent<MouseLook>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerAnimator.SetBool("Death", false);
    }
    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        // 체력 슬라이더 활성화
        healthSlider.gameObject.SetActive(true);

        // 체력 슬라이더의 최대값을 기본 체력값으로 변경
        healthSlider.maxValue = startingHealth;

        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value = health;

        // 플레이어 조작을 받는 컴포넌트들 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }


    //데미지 처리
    [PunRPC]
    public override void OnDamage(float damage,
        Vector3 hitPoint, Vector3 hitNormal)
    {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        if (!dead)
        {
            // 공격 받은 지점과 방향으로 파티클 효과를 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation
                = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            playerAnimator.SetTrigger("isHit");

            // 피격 효과음 재생
            playerAudioPlayer.PlayOneShot(hitSound);
        }

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
        // 갱신된 체력을 체력 슬라이더에 반영
        healthSlider.value = health;
    }

    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        playerAnimator.SetBool("Death", true);
        playerAnimator.SetTrigger("isDie");


        // 플레이어 조작을 받는 컴포넌트들 비활성화
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        playerShooter.enabled = false;

        // 땅 밑으로 떨어지지 않도록 고정
        playerRigidbody.isKinematic = true;

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        Collider[] Colliders = GetComponents<Collider>();
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i].enabled = false;
        }

        // 사망음 재생
        playerAudioPlayer.PlayOneShot(deathClip);



    }
}