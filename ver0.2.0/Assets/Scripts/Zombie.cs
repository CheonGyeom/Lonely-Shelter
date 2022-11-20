using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // ���� ��� ���̾�

    private LivingEntity targetEntity; // ������ ���
    private NavMeshAgent navMeshAgent; // ��ΰ�� AI ������Ʈ

    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ����� ����� �Ҹ�
    public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    private Rigidbody zombieRigidbody; // �����ٵ� ������Ʈ
    private Animator zombieAnimator; // �ִϸ����� ������Ʈ
    private AudioSource zombieAudioPlayer; // ����� �ҽ� ������Ʈ

    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {

                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private void Awake()
    {
        // ���� ������Ʈ�κ��� ����� ������Ʈ���� ��������
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();
        zombieRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��θ� ����
    private IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            if(!navMeshAgent.isOnNavMesh)
            {
                yield return null;

                continue;
            }

            if (hasTarget)
            {
                // ���� ��� ���� : ��θ� �����ϰ� AI �̵��� ��� ����

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
                zombieAnimator.SetBool("isWalk", true);
            }
            else
            {
                // ���� ��� ���� : AI �̵� ����
                navMeshAgent.isStopped = true;

                // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
                zombieAnimator.SetBool("isWalk", false);


                // 20 ������ �������� ���� ������ ���� �׷�����, ���� ��ġ�� ��� �ݶ��̴��� ������
                // ��, whatIsTarget ���̾ ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 40f, whatIsTarget);

                // ��� �ݶ��̴����� ��ȸ�ϸ鼭, ����ִ� LivingEntity ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� LivingEntity ������Ʈ ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ����ִٸ�,
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // ���� ����� �ش� LivingEntity�� ����
                        targetEntity = livingEntity;

                        // for�� ���� ��� ����
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
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

            // �ǰ� ȿ���� ���
            zombieAudioPlayer.PlayOneShot(hitSound);

            // Hit �ִϸ��̼� ���
            zombieAnimator.SetTrigger("isHit");

            Debug.Log("���� �ƾ�");
        }

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        Debug.Log("���� ���");

        // �� ������ �������� �ʵ��� ����
        zombieRigidbody.isKinematic = true;

        // �ٸ� AI���� �������� �ʵ��� �ڽ��� ��� �ݶ��̴����� ��Ȱ��ȭ
        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }

        

        // AI ������ �����ϰ� ����޽� ������Ʈ�� ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // ��� �ִϸ��̼� ���
        zombieAnimator.SetTrigger("isDie");
        // ��� ȿ���� ���
        zombieAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        // �ڽ��� ������� �ʾ�����,
        // �ֱ� ���� �������� timeBetAttack �̻� �ð��� �����ٸ� ���� ����
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            // �������κ��� LivingEntity Ÿ���� �������� �õ�
            LivingEntity attackTarget
                = other.GetComponent<LivingEntity>();

            // ������ LivingEntity�� �ڽ��� ���� ����̶�� ���� ����
            if (attackTarget != null && attackTarget == targetEntity)
            {

                Debug.Log("����");
                // �ֱ� ���� �ð��� ����
                lastAttackTime = Time.time;

                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                Vector3 hitPoint
                    = other.ClosestPoint(transform.position);
                Vector3 hitNormal
                    = transform.position - other.transform.position;

                // ���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
                zombieAnimator.SetTrigger("isAttack");
            }
        }
    }

}
