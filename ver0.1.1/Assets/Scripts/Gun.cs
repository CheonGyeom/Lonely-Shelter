using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State // ���� ����
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }

    public State state { get; private set; } // ���� ���� ����
    
    public Transform fireTransform; // �Ѿ��� �߻�� ��ġ

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����

    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������

    private float lastFireTime; // ���� ���������� �߻��� ����

    private float fireDistance = 50f; // �����Ÿ�

    public float fireRate; // �Ѿ� �߻� ����
    public float reloadTime; // ������ �ð�

    public int damage;

    public int reloadBulletCount; // �� źâ
    public int currentBulletCount; // ���� źâ�� �����ִ� �Ѿ� ��
    public int maxBulletCount = 100; // �ִ� ������ �� �ִ� �Ѿ� ��
    public int carryBulletCount; // ���� ���� �Ѿ� ��

    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // ����� ���� �ΰ��� ����
        bulletLineRenderer.positionCount = 2;
        // ���� �������� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // ��ü ���� ź�� ���� �ʱ�ȭ
        carryBulletCount = maxBulletCount;
        // ���� źâ�� ����ä���
        currentBulletCount = reloadBulletCount;

        // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        state = State.Ready;
        // ���������� ���� �� ������ �ʱ�ȭ
        lastFireTime = 0;
    }


    // �߻� �õ�
    public void Fire()
    {
        Debug.Log("������2");
        // ���� ���°� �߻� ������ ����
        // && ������ �� �߻� �������� timeBetFire �̻��� �ð��� ����
        if (state == State.Ready && Time.time >= lastFireTime + fireRate)
        {
            // ������ �� �߻� ������ ����
            lastFireTime = Time.time;
            // ���� �߻� ó�� ����
            Shot();
        }
    }

    // ���� �߻� ó��
    private void Shot()
    {
        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
        Debug.Log("������1");
        // �Ѿ��� ���� ���� ������ ����
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ(��������, ����, �浹 ���� �����̳�, �����Ÿ�)
        if (Physics.Raycast(fireTransform.position,
            fireTransform.forward, out hit, fireDistance))
        {
            // ���̰� � ��ü�� �浹�� ���

            // �浹�� �������κ��� IDamageable ������Ʈ�� �������� �õ�
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();

            // �������� ���� IDamageable ������Ʈ�� �������µ� �����ߴٸ�
            if (target != null)
            {
                // ������ OnDamage �Լ��� ������Ѽ� ���濡�� ������ �ֱ�
                target.OnDamage(damage, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;
        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            // �Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� �浹 ��ġ�� ���
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance;
        }

        // �߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitPosition));

        // ���� źȯ�� ���� -1
        currentBulletCount--;
        if (currentBulletCount <= 0)
        {
            // źâ�� ���� ź���� ���ٸ�, ���� ���� ���¸� Empty���� ����
            state = State.Empty;
        }
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // �ѱ� ȭ�� ȿ�� ���
       // muzzleFlashEffect.Play();
        // ź�� ���� ȿ�� ���
       // shellEjectEffect.Play();

        // �Ѱ� �Ҹ� ���
       //gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // ���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        bulletLineRenderer.enabled = true;
        

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
        bulletLineRenderer.enabled = false;
    }
}
