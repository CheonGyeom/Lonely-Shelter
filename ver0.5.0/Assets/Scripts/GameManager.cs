using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks

{  
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    //public List<Player> playerList = new List<Player>()
    //{
    //    PhotonNetwork.PlayerList
    //}

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹

    public Transform[] spawnPoints; // 플레이어 스폰 위치 배열

    public Text exitMasterClientText; // 마스터 클라이언트가 나가면 뜨는 경고 문구


    public bool isGameover { get; private set; } // 게임 오버 상태


    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayerSpawn();

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    void Update()
    {
        // 죽지 않은 상태에서 ESC 키를 눌렀다면 
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameover)
        {
            // ESC 메뉴가 켜져있지 않다면
            if (!UIManager.instance.escMenu.activeSelf)
            {
                // ESC 메뉴르 켠다
                UIManager.instance.SetActiveEscMenu(true);

                // 마우스 커서 잠금 해제, 커서 보이게
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // 아니라면
            else
            {
                // ESC 메뉴를 닫는다
                UIManager.instance.SetActiveEscMenu(false);

                // 마우스 커서 잠금, 커서 숨김
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }



    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
        // 마우스 커서 잠금 해제, 커서 보이게
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PlayerSpawn()
    {
        // 스폰 포인트
        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 플레이어 생성, 네트워크 상의 모든 클라이언트들에게 생성 됨.
        // GameObject createdPlayer = 
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);

        
    }

}
