using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ
    private MouseLook mouseLook; // �÷��̾� ȭ�� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ
    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��

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
        // ���� ������� ���� ��쿡�� �ǰ� ȿ�� ���
        if (!dead)
        {
            // ���� ���� ������ �������� ��ƼŬ ȿ���� ���
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation
                = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            playerAnimator.SetTrigger("isHit");

            // �ǰ� ȿ���� ���
            //zombieAudioPlayer.PlayOneShot(hitSound);
        }

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
        base.Die();

        // �ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���
        playerAnimator.SetBool("Death", true);
        playerAnimator.SetTrigger("isDie");


        // �÷��̾� ������ �޴� ������Ʈ�� ��Ȱ��ȭ
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        playerShooter.enabled = false;


        // ü�� �����̴� ��Ȱ��ȭ
        //healthSlider.gameObject.SetActive(false);

        // ����� ���
        //playerAudioPlayer.PlayOneShot(deathClip);



    }
}