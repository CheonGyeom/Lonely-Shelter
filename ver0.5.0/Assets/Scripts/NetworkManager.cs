using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0.2.0"; // ���� ����

    public Text startMenuNetworkStatusText; // ��ŸƮ �޴� ��Ʈ��ũ ���� �ؽ�Ʈ
    public Text networkStatusText; // �� ����� �޴� ��Ʈ��ũ ���� �ؽ�Ʈ
    public Text lobbyNetworkStatusText; // �κ� �ؿ� ��Ʈ��ũ ���� �ؽ�Ʈ
    public Text setNicknameMenuPopUpText; // �� �г��� �޴��� �˾� �ؽ�Ʈ
    public Text roomPopUpText; // �� �޴��� �˾� �ؽ�Ʈ
    public Text gameVersionText; // ���� ���� �ؽ�Ʈ
    public Text roomPlayerNumberText; // �� �÷��̾� �ο� �ؽ�Ʈ
    public Text roomTitleText; // �� ���� �ؽ�Ʈ
    public Text[] nicknameText; // �г��� �ؽ�Ʈ �迭
    public GameObject[] nicknamePanel; // �г��� ��� �г�
    public InputField startMenuRoomInput, nicknameInput; // ��ŸƮ �޴� ���̸�, �г��� ��ǲ�ʵ�
    public InputField roomInput; // �� ����� �޴� ���̸�
    public Dropdown peapleDropdown; // �ο� �� ��Ӵٿ�
    public Button makeRoomButton; // �� ����� ��ư
    public Button joinButton; // �� ���� ��ư
    public Button startButton; // ����Ʈ �޴� ���� ��ư
    public Button gameStartButton; // ���ӽ�ŸƮ ��ư
    public GameObject room; // �� 
    public GameObject startMenu, makeRoomMenu, setNicknameMenu; // ��ŸƮ�޴�, �� ����� �޴�, �� �г��� �޴�
    public GameObject inGameStartButton; // �ΰ��� ��ŸƮ ��ư
    public RectTransform contents; // �÷��̾���� �θ� ��ü
    public PhotonView PV; // ���� ��

    private int roomMaxPlayer; // �� �ִ� �ο�

    private bool isNickname; // ���� �г����� ���ߴ���

    private void Awake()
    {
        // ȭ����� ����
        Screen.SetResolution(1920, 1080, true);

        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // ���� ��� Ŭ���̾�Ʈ�� ������ Ŭ���̾�Ʈ�� ������ ������ �ڵ����� �ε���. �� true�� ���� ����ȭ.
        PhotonNetwork.AutomaticallySyncScene = true;

        // �г��� �ִ� ���� = 35��, �� �̸� �ִ� ���� = 18�ڷ� ����
        nicknameInput.characterLimit = 35;
        roomInput.characterLimit = 18;

        // ��ŸƮ ��ư�� ��� ��Ȱ��ȭ
        startButton.interactable = false;
        // ������ �õ� ������ �ؽ�Ʈ�� ǥ��
        lobbyNetworkStatusText.text = "������ ������ ���� ��...";
        // ���� ���� ǥ��
        gameVersionText.text = "���� " + gameVersion;

        // ���� �ٽ� �ε�Ǿ �г����� �ִ��� ������ üũ
        if (PhotonNetwork.NickName == "")
        {
            isNickname = false;
        }
        else
        {
            isNickname = true;
        }
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NickName);
    }

    public override void OnConnectedToMaster()
    {
        // ��ŸƮ ��ư�� Ȱ��ȭ
        startButton.interactable = true;
        // ���� ���� ǥ��
        lobbyNetworkStatusText.text = "�¶��� : ������ ������ �����";
    }

    // ������ ���� ���� ����, ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư�� ��Ȱ��ȭ
        startButton.interactable = false;
        // ���� ���� ǥ��
        lobbyNetworkStatusText.text = "�������� : ������ ������ ������� ����\n������ �ٽ� �õ��ϴ� ��...";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ����� �õ�
    public void Connect()
    {
        // �ߺ� ���� �õ��� ���� ����, ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ������ ������ �������̶��
        if (PhotonNetwork.IsConnected)
        {
            // �� �̸��̳� �г����� ����ִٸ�
            if (startMenuRoomInput.text.Equals("") || nicknameInput.text.Equals(""))
            {
                joinButton.interactable = true;
                // info�� �Է����ּ���
                startMenuNetworkStatusText.text = "�г��� �Ǵ� �� ������ �Է��ϼ���";
            }
            else
            {
                // �� ���� ����
                startMenuNetworkStatusText.text = "�濡 �����ϴ� ��...";
                PhotonNetwork.JoinRoom(startMenuRoomInput.text);
            }


        }
        else
        {
            // ������ ������ �������� �ƴ϶��, ������ ������ ���� �õ�
            startMenuNetworkStatusText.text = "�������� : ������ ������ ������� ����\n������ �ٽ� �õ��ϴ� ��...";
            // ������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // (�´� �� �̸��� ���� Or ���� �� ����) �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ��ư �ٽ� Ȱ��ȭ
        joinButton.interactable = true;

        // ���� ���� ǥ��
        startMenuNetworkStatusText.text = "���� �������� �ʰų� �̹� �ο��� ���� �� ���Դϴ�";
    }

    public void MakeRoom()
    {
        // �ߺ� �� ���� ����
        makeRoomButton.interactable = false;

        // �ӽ� ��Ʈ�� �����üĿ�� Ư�����ڸ� ������ ���ڿ��� �ִ´�
        string roomNameChecker = Regex.Replace(roomInput.text, @"[^0-9a-zA-Z��-�R]{1,10}", "", RegexOptions.Singleline);

        // �� �̸��� ����ִٸ�
        if (roomInput.text.Equals(""))
        {
            makeRoomButton.interactable = true;
            // info�� �Է����ּ���
            networkStatusText.text = "�� ������ �Է��ϼ���";
        }
        // ����ǲ.�ؽ�Ʈ�� �����üĿ�� �ٸ��ٸ�
        else if (roomInput.text.Equals(roomNameChecker) == false)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "�� ���� Ư�����ڰ� ���ԵǾ� �ֽ��ϴ�";
        }
        // �� �̸��� 18�ں��� ũ�ٸ�
        else if (roomInput.text.Length > 35)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "�� ������ �ʹ� ��ϴ�\n�ִ� 18�ڱ��� �� �� �ֽ��ϴ�";
        }
        // �� �̸��� �� ���ڶ��
        else if (roomInput.text.Length < 2)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "�� ������ �ּ� �� ���� �̻��̾�� �մϴ�";
        }
        else
        {
            // ���� ���� ǥ��
            networkStatusText.text = "���� �����ϴ� ��...";
            //��� �ٿ� ����� 4(�ּ��ο�) �����ֱ�
            roomMaxPlayer = peapleDropdown.value + 4;
            // �� �����(���̸�, ��ɼ�{ �� �ִ��ο� = ����Ʈ ����ȯ roomMaxPlayer})
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = byte.Parse(roomMaxPlayer.ToString()) });
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        networkStatusText.text = "���� �̸��� ���� �̹� �����մϴ�";

        makeRoomButton.interactable = true;
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // �� �޴� ǥ��
        room.SetActive(true);

        // ���� �޴�(��ŸƮ �޴�, �� ����� �޴�, �˾��ؽ�Ʈ) ����
        startMenu.SetActive(false);
        makeRoomMenu.SetActive(false);
        roomPopUpText.text = "";


        // ���� �����̶��
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }

        // ���ӽ��� ��ư, �� ����� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        makeRoomButton.interactable = true;

        // ��ŸƮ �޴� �ݱ�
        startMenu.SetActive(false);

        // �� �̸� ����
        roomTitleText.text = "�� ���� : " + PhotonNetwork.CurrentRoom.Name;

        if(PhotonNetwork.IsMasterClient)
        {
            // ���� ���� ��ư Ȱ��ȭ
            inGameStartButton.SetActive(true);


        }
        else
        {
            inGameStartButton.SetActive(false);
        }

        
    }

    public void LeaveRoom()
    {
        // �� ������
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
        //{
        //    roomPopUpText.text = "�ּ� ���� �ο��� 4���Դϴ�";
        //}
        //else
        {
            // �ߺ� ���� �õ��� ���� ����, ���ӽ��� ��ư ��� ��Ȱ��ȭ
            gameStartButton.interactable = false;
            // ��� �� �����ڵ��� InGame ���� �ε��ϰ� ��
            PhotonNetwork.LoadLevel("InGame");
        }

        
    }

    // �÷��̾ �濡 �����ϸ� �ڵ� ȣ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }
    }

    // �÷��̾ �濡�� ������ �ڵ� ȣ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
            // ���� ���� ��ư Ȱ��ȭ
            inGameStartButton.SetActive(true);
        }

    }

    [PunRPC]
    void NicknameRPC()
    {
        // �� ���� �÷��̾�/ �ִ� �÷��̾� ����
        roomPlayerNumberText.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // �� �г��ӵ� û�����ֱ�
        for (int i = 0; i < 12; i++)
        {
            nicknamePanel[i].SetActive(false);
            nicknameText[i].text = "";
        }

        // �г��� ���ֱ�
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // �÷��̾ ���� �����̾�
            if (PhotonNetwork.PlayerList[i].IsMasterClient && PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (��)" + "</color>" + "<color=#E5B400>" + " (����)" + "</color>";
            }
            // �÷��̾ �����̶��
            else if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#E5B400>" + " (����)" +"</color>";
            }
            // �÷��̾ �����
            else if (PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (��)" + "</color>";
            }
            else
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName;

            }
        }
    }

    public void DeleteStateText()
    {
        networkStatusText.text = "";
    }

    // �κ� ���� ���� ��ư�� ������ ��
    public void StartMenuClick()
    {
        // �г����� ���ߴٸ�
        if (isNickname)
        {
            // ȯ�� �޽���
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
            // ��ŸƮ �޴��� ����
            startMenu.SetActive(true);
        }
        // �г����� ������ �ʾҴٸ�
        else
        {
            // �̸��� ���´�
            setNicknameMenuPopUpText.text = "����� �̸��� �����ΰ���?";
            // �� �г��� �޴��� ����
            setNicknameMenu.SetActive(true);
        }
    }

    // �г��� Ȯ�� ��ư�� ������ ��
    public void SetNickname()
    {
        // �ӽ� ��Ʈ�� �г���üĿ�� Ư�����ڿ� ���ڸ� ������ ���ڿ��� �ִ´�
        string nicknameChecker = Regex.Replace(nicknameInput.text, @"[^a-zA-Z��-�R]", "", RegexOptions.Singleline);


        // �г����� �� ��ٸ�
        if (nicknameInput.text.Equals(""))
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "�г����� �Է����ּ���";
        }
        // �г�����ǲ.�ؽ�Ʈ�� �г���üĿ�� �ٸ��ٸ�
        else if (nicknameInput.text.Equals(nicknameChecker) == false)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "�г��ӿ� ���� �Ǵ� Ư�����ڰ� ���ԵǾ� �ֽ��ϴ�";
        }
        // �г����� 35�ں��� ũ�ٸ�
        else if (nicknameInput.text.Length > 35)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "�г����� �ʹ� ��ϴ�\n�ִ� 35�ڱ��� �� �� �ֽ��ϴ�";
        }
        // �г����� �� ���ڶ��
        else if (nicknameInput.text.Length < 2)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "�г����� �ּ� �� ���� �̻��̾�� �մϴ�";
        }
        else
        {
            // ���� �÷��̾��� �̸��� �г��� ��ǲ �ؽ�Ʈ�� �Ѵ�
            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
            // isNickname�� true�� �Ѵ�
            isNickname = true;
            // ȯ�� �޽���
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
            // �� �г��� �޴��� ����
            setNicknameMenu.SetActive(false);
            // ��ŸƮ �޴��� �Ҵ�
            startMenu.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
