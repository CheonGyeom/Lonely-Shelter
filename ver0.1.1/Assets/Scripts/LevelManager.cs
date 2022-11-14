using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public GameObject zombiePrefab;

    public GameObject rangeObject;
    TerrainCollider rangeCollider;

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트

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
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        Return_RandomPosition();
    }

    Vector3 Return_RandomPosition()
    {
        //Vector3 originPosition = rangeObject.transform.position;
        //// 콜라이더의 사이즈를 가져오는 bound.size 사용
        //float range_X = rangeCollider.bounds.size.x;
        //float range_Z = rangeCollider.bounds.size.z;
        //float range_Y = rangeCollider.bounds.size.y;

        //range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        //range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        //range_Y = Random.Range(range_Y - 215f, range_Y - 220f);

        Vector3 RandomPos = Random.insideUnitSphere * 250.0f;
        NavMeshHit hit;
        NavMesh.SamplePosition(RandomPos, out hit, 1000.0f, NavMesh.AllAreas);


        Vector3 RandomPostion = hit.position;

        Vector3 respawnPosition = RandomPostion;
        return respawnPosition;
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            GameObject instantCapsul = Instantiate(zombiePrefab, Return_RandomPosition(), Quaternion.identity);
        }
    }
}
