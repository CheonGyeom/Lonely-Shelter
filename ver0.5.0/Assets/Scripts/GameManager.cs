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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    //public List<Player> playerList = new List<Player>()
    //{
    //    PhotonNetwork.PlayerList
    //}

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public GameObject playerPrefab; // ������ �÷��̾� ĳ���� ������

    public Transform[] spawnPoints; // �÷��̾� ���� ��ġ �迭

    public Text exitMasterClientText; // ������ Ŭ���̾�Ʈ�� ������ �ߴ� ��� ����


    public bool isGameover { get; private set; } // ���� ���� ����


    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayerSpawn();

        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    void Update()
    {
        // ���� ���� ���¿��� ESC Ű�� �����ٸ� 
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameover)
        {
            // ESC �޴��� �������� �ʴٸ�
            if (!UIManager.instance.escMenu.activeSelf)
            {
                // ESC �޴��� �Ҵ�
                UIManager.instance.SetActiveEscMenu(true);

                // ���콺 Ŀ�� ��� ����, Ŀ�� ���̰�
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // �ƴ϶��
            else
            {
                // ESC �޴��� �ݴ´�
                UIManager.instance.SetActiveEscMenu(false);

                // ���콺 Ŀ�� ���, Ŀ�� ����
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }



    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        UIManager.instance.SetActiveGameoverUI(true);
        // ���콺 Ŀ�� ��� ����, Ŀ�� ���̰�
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PlayerSpawn()
    {
        // ���� ����Ʈ
        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �÷��̾� ����, ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ��.
        // GameObject createdPlayer = 
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);

        
    }

}
