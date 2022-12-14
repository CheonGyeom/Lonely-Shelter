using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0.2.0"; // 게임 버전

    public Text startMenuNetworkStatusText; // 스타트 메뉴 네트워크 상태 텍스트
    public Text networkStatusText; // 방 만들기 메뉴 네트워크 상태 텍스트
    public Text lobbyNetworkStatusText; // 로비 밑에 네트워크 상태 텍스트
    public Text setNicknameMenuPopUpText; // 셋 닉네임 메뉴의 팝업 텍스트
    public Text roomPopUpText; // 룸 메뉴의 팝업 텍스트
    public Text gameVersionText; // 게임 버전 텍스트
    public Text roomPlayerNumberText; // 룸 플레이어 인원 텍스트
    public Text roomTitleText; // 룸 제목 텍스트
    public Text[] nicknameText; // 닉네임 텍스트 배열
    public GameObject[] nicknamePanel; // 닉네임 배경 패널
    public InputField startMenuRoomInput, nicknameInput; // 스타트 메뉴 방이름, 닉네임 인풋필드
    public InputField roomInput; // 방 만들기 메뉴 방이름
    public Dropdown peapleDropdown; // 인원 수 드롭다운
    public Button makeRoomButton; // 방 만들기 버튼
    public Button joinButton; // 방 접속 버튼
    public Button startButton; // 디폴트 메뉴 시작 버튼
    public Button gameStartButton; // 게임스타트 버튼
    public GameObject room; // 룸 
    public GameObject startMenu, makeRoomMenu, setNicknameMenu; // 스타트메뉴, 방 만들기 메뉴, 셋 닉네임 메뉴
    public GameObject inGameStartButton; // 인게임 스타트 버튼
    public RectTransform contents; // 플레이어네임 부모 객체
    public PhotonView PV; // 포톤 뷰

    private int roomMaxPlayer; // 룸 최대 인원

    private bool isNickname; // 현재 닉네임을 정했는지

    private void Awake()
    {
        // 화면비율 설정
        Screen.SetResolution(1920, 1080, true);

        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보를 가지고 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 방의 모든 클라이언트가 마스터 클라이언트와 동일한 레벨을 자동으로 로드함. 즉 true시 레벨 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;

        // 닉네임 최대 길이 = 35자, 방 이름 최대 길이 = 18자로 설정
        nicknameInput.characterLimit = 35;
        roomInput.characterLimit = 18;

        // 스타트 버튼을 잠시 비활성화
        startButton.interactable = false;
        // 접속을 시도 중임을 텍스트로 표시
        lobbyNetworkStatusText.text = "마스터 서버에 연결 중...";
        // 게임 버전 표시
        gameVersionText.text = "버젼 " + gameVersion;

        // 씬이 다시 로드되어도 닉네임이 있는지 없는지 체크
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
        // 스타트 버튼을 활성화
        startButton.interactable = true;
        // 접속 정보 표시
        lobbyNetworkStatusText.text = "온라인 : 마스터 서버에 연결됨";
    }

    // 마스터 서버 접속 실패, 연결 끊길 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼을 비활성화
        startButton.interactable = false;
        // 접속 정보 표시
        lobbyNetworkStatusText.text = "오프라인 : 마스터 서버에 연결되지 않음\n연결을 다시 시도하는 중...";

        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 만들기 시도
    public void Connect()
    {
        // 중복 접속 시도를 막기 위해, 접속 버튼 잠시 비활성화
        joinButton.interactable = false;

        // 마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            // 방 이름이나 닉네임이 비어있다면
            if (startMenuRoomInput.text.Equals("") || nicknameInput.text.Equals(""))
            {
                joinButton.interactable = true;
                // info를 입력해주세요
                startMenuNetworkStatusText.text = "닉네임 또는 방 제목을 입력하세요";
            }
            else
            {
                // 룸 접속 실행
                startMenuNetworkStatusText.text = "방에 접속하는 중...";
                PhotonNetwork.JoinRoom(startMenuRoomInput.text);
            }


        }
        else
        {
            // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            startMenuNetworkStatusText.text = "오프라인 : 마스터 서버에 연결되지 않음\n연결을 다시 시도하는 중...";
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // (맞는 방 이름이 없어 Or 방이 꽉 차서) 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 버튼 다시 활성화
        joinButton.interactable = true;

        // 접속 상태 표시
        startMenuNetworkStatusText.text = "방이 존재하지 않거나 이미 인원이 가득 찬 방입니다";
    }

    public void MakeRoom()
    {
        // 중복 방 생성 방지
        makeRoomButton.interactable = false;

        // 임시 스트링 룸네임체커에 특수문자를 제외한 문자열을 넣는다
        string roomNameChecker = Regex.Replace(roomInput.text, @"[^0-9a-zA-Z가-힣]{1,10}", "", RegexOptions.Singleline);

        // 방 이름이 비어있다면
        if (roomInput.text.Equals(""))
        {
            makeRoomButton.interactable = true;
            // info를 입력해주세요
            networkStatusText.text = "방 제목을 입력하세요";
        }
        // 룸인풋.텍스트가 룸네임체커와 다르다면
        else if (roomInput.text.Equals(roomNameChecker) == false)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "방 제목에 특수문자가 포함되어 있습니다";
        }
        // 방 이름이 18자보다 크다면
        else if (roomInput.text.Length > 35)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "방 제목이 너무 깁니다\n최대 18자까지 쓸 수 있습니다";
        }
        // 방 이름이 한 글자라면
        else if (roomInput.text.Length < 2)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "방 제목은 최소 두 글자 이상이어야 합니다";
        }
        else
        {
            // 접속 상태 표시
            networkStatusText.text = "방을 생성하는 중...";
            //드롭 다운 밸류에 4(최소인원) 더해주기
            roomMaxPlayer = peapleDropdown.value + 4;
            // 방 만들기(방이름, 방옵션{ 방 최대인원 = 바이트 형변환 roomMaxPlayer})
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = byte.Parse(roomMaxPlayer.ToString()) });
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        networkStatusText.text = "같은 이름의 방이 이미 존재합니다";

        makeRoomButton.interactable = true;
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        // 방 메뉴 표시
        room.SetActive(true);

        // 이전 메뉴(스타트 메뉴, 방 만들기 메뉴, 팝업텍스트) 끄기
        startMenu.SetActive(false);
        makeRoomMenu.SetActive(false);
        roomPopUpText.text = "";


        // 만약 방장이라면
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }

        // 게임시작 버튼, 방 만들기 버튼 활성화
        joinButton.interactable = true;
        makeRoomButton.interactable = true;

        // 스타트 메뉴 닫기
        startMenu.SetActive(false);

        // 룸 이름 설정
        roomTitleText.text = "방 제목 : " + PhotonNetwork.CurrentRoom.Name;

        if(PhotonNetwork.IsMasterClient)
        {
            // 게임 시작 버튼 활성화
            inGameStartButton.SetActive(true);


        }
        else
        {
            inGameStartButton.SetActive(false);
        }

        
    }

    public void LeaveRoom()
    {
        // 방 나가기
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
        //{
        //    roomPopUpText.text = "최소 시작 인원은 4명입니다";
        //}
        //else
        {
            // 중복 접속 시도를 막기 위해, 게임시작 버튼 잠시 비활성화
            gameStartButton.interactable = false;
            // 모든 룸 참가자들이 InGame 씬을 로드하게 함
            PhotonNetwork.LoadLevel("InGame");
        }

        
    }

    // 플레이어가 방에 참여하면 자동 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }
    }

    // 플레이어가 방에서 나가면 자동 호출
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
            // 게임 시작 버튼 활성화
            inGameStartButton.SetActive(true);
        }

    }

    [PunRPC]
    void NicknameRPC()
    {
        // 룸 현재 플레이어/ 최대 플레이어 설정
        roomPlayerNumberText.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // 방 닉네임들 청소해주기
        for (int i = 0; i < 12; i++)
        {
            nicknamePanel[i].SetActive(false);
            nicknameText[i].text = "";
        }

        // 닉네임 써주기
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 플레이어가 난데 방장이야
            if (PhotonNetwork.PlayerList[i].IsMasterClient && PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (나)" + "</color>" + "<color=#E5B400>" + " (방장)" + "</color>";
            }
            // 플레이어가 방장이라면
            else if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#E5B400>" + " (방장)" +"</color>";
            }
            // 플레이어가 나라면
            else if (PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (나)" + "</color>";
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

    // 로비 게임 시작 버튼을 눌렀을 때
    public void StartMenuClick()
    {
        // 닉네임을 정했다면
        if (isNickname)
        {
            // 환영 메시지
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
            // 스타트 메뉴를 띄운다
            startMenu.SetActive(true);
        }
        // 닉네임을 정하지 않았다면
        else
        {
            // 이름을 묻는다
            setNicknameMenuPopUpText.text = "당신의 이름은 무엇인가요?";
            // 셋 닉네임 메뉴를 띄운다
            setNicknameMenu.SetActive(true);
        }
    }

    // 닉네임 확인 버튼을 눌렀을 때
    public void SetNickname()
    {
        // 임시 스트링 닉네임체커에 특수문자와 숫자를 제외한 문자열을 넣는다
        string nicknameChecker = Regex.Replace(nicknameInput.text, @"[^a-zA-Z가-힣]", "", RegexOptions.Singleline);


        // 닉네임을 안 썼다면
        if (nicknameInput.text.Equals(""))
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "닉네임을 입력해주세요";
        }
        // 닉네임인풋.텍스트가 닉네임체커와 다르다면
        else if (nicknameInput.text.Equals(nicknameChecker) == false)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "닉네임에 숫자 또는 특수문자가 포함되어 있습니다";
        }
        // 닉네임이 35자보다 크다면
        else if (nicknameInput.text.Length > 35)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "닉네임이 너무 깁니다\n최대 35자까지 쓸 수 있습니다";
        }
        // 닉네임이 한 글자라면
        else if (nicknameInput.text.Length < 2)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "닉네임은 최소 두 글자 이상이어야 합니다";
        }
        else
        {
            // 로컬 플레이어의 이름을 닉네임 인풋 텍스트로 한다
            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
            // isNickname을 true로 한다
            isNickname = true;
            // 환영 메시지
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
            // 셋 닉네임 메뉴를 끈다
            setNicknameMenu.SetActive(false);
            // 스타트 메뉴를 켠다
            startMenu.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
