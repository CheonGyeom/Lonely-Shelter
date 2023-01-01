using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun, IPunObservable
{
    private bool Ready; // �߻� �غ� ����
    
    public Transform fireTransform; // �Ѿ��� �߻�� ��ġ

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    public AudioClip shotClip; // �߻� �Ҹ�

    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������

    private float lastFireTime; // ���� ���������� �߻��� ����

    private float fireDistance = 50f; // �����Ÿ�

    public float fireRate; // �Ѿ� �߻� ����

    public int damage;

    // �ֱ������� �ڵ� ����Ǵ�, ����ȭ �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ���� ������Ʈ��� ���� �κ��� �����
        if (stream.IsWriting)
        {
            // ���� ���� ���¸� ��Ʈ��ũ�� ���� ������
            stream.SendNext(Ready);
        }
        else
        {
            // ����Ʈ ������Ʈ��� �б� �κ��� �����
            // ���� ���� ���¸� ��Ʈ��ũ�� ���� �ޱ�
            Ready = (bool)stream.ReceiveNext();
        }
    }

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
        // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        Ready = true;
        // ���������� ���� �� ������ �ʱ�ȭ
        lastFireTime = 0;
    }


    // �߻� �õ�
    public void Fire()
    {
        // ���� ���°� �߻� ������ ����
        // && ������ �� �߻� �������� timeBetFire �̻��� �ð��� ����
        if (Ready && Time.time >= lastFireTime + fireRate)
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
        // ���� �߻� ó���� ȣ��Ʈ���� �븮
        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient);
    }

    // ȣ��Ʈ���� ����Ǵ�, ���� �߻� ó��
    [PunRPC]
    public void ShotProcessOnServer()
    {
        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
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

        // �߻� ����Ʈ ���, ����Ʈ ����� ��� Ŭ���̾�Ʈ�鿡�� ����
        photonView.RPC("ShotEffectPrccessOnClients", RpcTarget.All, hitPosition);
    }

    // ����Ʈ ��� �ڷ�ƾ�� �����ϴ� �޼���
    [PunRPC]
    public void ShotEffectPrccessOnClients(Vector3 hitPosition)
    {
        StartCoroutine(ShotEffect(hitPosition));
    }


    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // �ѱ� ȭ�� ȿ�� ���
        muzzleFlashEffect.Play();

        // �Ѱ� �Ҹ� ���
        gunAudioPlayer.PlayOneShot(shotClip);

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
