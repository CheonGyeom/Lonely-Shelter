using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // 플레이어의 애니메이터
    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private MouseLook mouseLook; // 플레이어 화면 돌리기 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        mouseLook = GetComponent<MouseLook>();

        playerAnimator.SetBool("Death", false);
    }

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
            //zombieAudioPlayer.PlayOneShot(hitSound);
        }

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
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


        // 체력 슬라이더 비활성화
        //healthSlider.gameObject.SetActive(false);

        // 사망음 재생
        //playerAudioPlayer.PlayOneShot(deathClip);



    }
}