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
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 LevelManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<LevelManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static LevelManager m_instance;

    public GameObject zombiePrefab;

    public GameObject rangeObject;
    TerrainCollider rangeCollider;

    Vector3 respawnPosition;

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // 로컬 오브젝트라면 쓰기 부분이 실행됨
    //    if (stream.IsWriting)
    //    {
    //        // 좀비 스폰 위치 네트워크로 보내기
    //        stream.SendNext(respawnPosition);
    //    }
    //    else
    //    {
    //        // 리모트 오브젝트라면 읽기 부분이 실행됨

    //        // 좀비 스폰 위치 네트워크로 받기
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
        // 호스트만 좀비를 직접 생성할 수 있음
        // 다른 클라이언트들은 호스트가 생성한 좀비를 동기화를 통해 받아옴
        if (PhotonNetwork.IsMasterClient)
        {
            // 아침일 때는 좀비를 생성하지 않음
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

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            GameObject instantCapsul = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, Return_RandomPosition(), Quaternion.identity);
        }
    }


}
