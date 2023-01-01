using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ExitGames.Client.Photon;
using Photon.Pun;

public class LevelManager : MonoBehaviourPun//, IPunObservable
{
    public static LevelManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ LevelManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<LevelManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static LevelManager m_instance;

    public GameObject zombiePrefab;

    public GameObject rangeObject;
    TerrainCollider rangeCollider;

    Vector3 respawnPosition;

    private List<Zombie> zombies = new List<Zombie>(); // ������ ������� ��� ����Ʈ


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // ���� ������Ʈ��� ���� �κ��� �����
    //    if (stream.IsWriting)
    //    {
    //        // ���� ���� ��ġ ��Ʈ��ũ�� ������
    //        stream.SendNext(respawnPosition);
    //    }
    //    else
    //    {
    //        // ����Ʈ ������Ʈ��� �б� �κ��� �����

    //        // ���� ���� ��ġ ��Ʈ��ũ�� �ޱ�
    //        respawnPosition = (Vector3)stream.ReceiveNext();
    //    }
    //}


    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<TerrainCollider>();
    }

    void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    // Update is called once per frame
    void Update()
    {
        // ȣ��Ʈ�� ���� ���� ������ �� ����
        // �ٸ� Ŭ���̾�Ʈ���� ȣ��Ʈ�� ������ ���� ����ȭ�� ���� �޾ƿ�
        if (PhotonNetwork.IsMasterClient)
        {
            // ��ħ�� ���� ���� �������� ����
            // BLABLA

            Return_RandomPosition();
        }

        
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 RandomPos = Random.insideUnitSphere * 250.0f;
        NavMeshHit hit;
        NavMesh.SamplePosition(RandomPos, out hit, 1000.0f, NavMesh.AllAreas);


        Vector3 RandomPostion = hit.position;

        respawnPosition = RandomPostion;
        return respawnPosition;
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            // ���� ��ġ �κп� ������ ���� �Լ� Return_RandomPosition() �Լ� ����
            GameObject instantCapsul = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, Return_RandomPosition(), Quaternion.identity);
        }
    }


}
