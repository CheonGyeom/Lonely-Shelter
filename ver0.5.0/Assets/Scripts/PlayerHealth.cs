using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ
    private MouseLook mouseLook; // �÷��̾� ȭ�� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ
    private AudioSource  playerAudioPlayer; // �÷��̾� �Ҹ� �����
    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    public AudioClip deathClip; // ����� ����� ����
    public AudioClip hitSound; // ���ݴ��ҽ� ����� ����
    public Rigidbody playerRigidbody; // �÷��̾� ������ �ٵ�

    public Slider healthSlider; // ü���� ǥ���� UI �����̴�

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
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();

        // ü�� �����̴� Ȱ��ȭ
        healthSlider.gameObject.SetActive(true);

        // ü�� �����̴��� �ִ밪�� �⺻ ü�°����� ����
        healthSlider.maxValue = startingHealth;

        // ü�� �����̴��� ���� ���� ü�°����� ����
        healthSlider.value = health;

        // �÷��̾� ������ �޴� ������Ʈ�� Ȱ��ȭ
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }


    //������ ó��
    [PunRPC]
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
            playerAudioPlayer.PlayOneShot(hitSound);
        }

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
        // ���ŵ� ü���� ü�� �����̴��� �ݿ�
        healthSlider.value = health;
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

        // �� ������ �������� �ʵ��� ����
        playerRigidbody.isKinematic = true;

        // �ٸ� AI���� �������� �ʵ��� �ڽ��� ��� �ݶ��̴����� ��Ȱ��ȭ
        Collider[] Colliders = GetComponents<Collider>();
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i].enabled = false;
        }

        // ����� ���
        playerAudioPlayer.PlayOneShot(deathClip);



    }
}