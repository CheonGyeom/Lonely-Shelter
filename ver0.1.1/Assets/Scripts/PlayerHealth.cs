using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ

    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
        base.Die();

        // ü�� �����̴� ��Ȱ��ȭ
        //healthSlider.gameObject.SetActive(false);

        // ����� ���
        //playerAudioPlayer.PlayOneShot(deathClip);
        // �ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���
       // playerAnimator.SetTrigger("Die");

        // �÷��̾� ������ �޴� ������Ʈ�� ��Ȱ��ȭ
        playerMovement.enabled = false;
    }
}