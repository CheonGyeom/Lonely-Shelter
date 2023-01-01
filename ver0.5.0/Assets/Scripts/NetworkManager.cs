using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0.2.0"; // °ÔÀÓ ¹öÀü

    public Text startMenuNetworkStatusText; // ½ºÅ¸Æ® ¸Ş´º ³×Æ®¿öÅ© »óÅÂ ÅØ½ºÆ®
    public Text networkStatusText; // ¹æ ¸¸µé±â ¸Ş´º ³×Æ®¿öÅ© »óÅÂ ÅØ½ºÆ®
    public Text lobbyNetworkStatusText; // ·Îºñ ¹Ø¿¡ ³×Æ®¿öÅ© »óÅÂ ÅØ½ºÆ®
    public Text setNicknameMenuPopUpText; // ¼Â ´Ğ³×ÀÓ ¸Ş´ºÀÇ ÆË¾÷ ÅØ½ºÆ®
    public Text roomPopUpText; // ·ë ¸Ş´ºÀÇ ÆË¾÷ ÅØ½ºÆ®
    public Text gameVersionText; // °ÔÀÓ ¹öÀü ÅØ½ºÆ®
    public Text roomPlayerNumberText; // ·ë ÇÃ·¹ÀÌ¾î ÀÎ¿ø ÅØ½ºÆ®
    public Text roomTitleText; // ·ë Á¦¸ñ ÅØ½ºÆ®
    public Text[] nicknameText; // ´Ğ³×ÀÓ ÅØ½ºÆ® ¹è¿­
    public GameObject[] nicknamePanel; // ´Ğ³×ÀÓ ¹è°æ ÆĞ³Î
    public InputField startMenuRoomInput, nicknameInput; // ½ºÅ¸Æ® ¸Ş´º ¹æÀÌ¸§, ´Ğ³×ÀÓ ÀÎÇ²ÇÊµå
    public InputField roomInput; // ¹æ ¸¸µé±â ¸Ş´º ¹æÀÌ¸§
    public Dropdown peapleDropdown; // ÀÎ¿ø ¼ö µå·Ó´Ù¿î
    public Button makeRoomButton; // ¹æ ¸¸µé±â ¹öÆ°
    public Button joinButton; // ¹æ Á¢¼Ó ¹öÆ°
    public Button startButton; // µğÆúÆ® ¸Ş´º ½ÃÀÛ ¹öÆ°
    public Button gameStartButton; // °ÔÀÓ½ºÅ¸Æ® ¹öÆ°
    public GameObject room; // ·ë 
    public GameObject startMenu, makeRoomMenu, setNicknameMenu; // ½ºÅ¸Æ®¸Ş´º, ¹æ ¸¸µé±â ¸Ş´º, ¼Â ´Ğ³×ÀÓ ¸Ş´º
    public GameObject inGameStartButton; // ÀÎ°ÔÀÓ ½ºÅ¸Æ® ¹öÆ°
    public RectTransform contents; // ÇÃ·¹ÀÌ¾î³×ÀÓ ºÎ¸ğ °´Ã¼
    public PhotonView PV; // Æ÷Åæ ºä

    private int roomMaxPlayer; // ·ë ÃÖ´ë ÀÎ¿ø

    private bool isNickname; // ÇöÀç ´Ğ³×ÀÓÀ» Á¤Çß´ÂÁö

    private void Awake()
    {
        // È­¸éºñÀ² ¼³Á¤
        Screen.SetResolution(1920, 1080, true);

        // Á¢¼Ó¿¡ ÇÊ¿äÇÑ Á¤º¸(°ÔÀÓ ¹öÀü) ¼³Á¤
        PhotonNetwork.GameVersion = gameVersion;
        // ¼³Á¤ÇÑ Á¤º¸¸¦ °¡Áö°í ¸¶½ºÅÍ ¼­¹ö Á¢¼Ó ½Ãµµ
        PhotonNetwork.ConnectUsingSettings();

        // ¹æÀÇ ¸ğµç Å¬¶óÀÌ¾ğÆ®°¡ ¸¶½ºÅÍ Å¬¶óÀÌ¾ğÆ®¿Í µ¿ÀÏÇÑ ·¹º§À» ÀÚµ¿À¸·Î ·ÎµåÇÔ. Áï true½Ã ·¹º§ µ¿±âÈ­.
        PhotonNetwork.AutomaticallySyncScene = true;

        // ´Ğ³×ÀÓ ÃÖ´ë ±æÀÌ = 35ÀÚ, ¹æ ÀÌ¸§ ÃÖ´ë ±æÀÌ = 18ÀÚ·Î ¼³Á¤
        nicknameInput.characterLimit = 35;
        roomInput.characterLimit = 18;

        // ½ºÅ¸Æ® ¹öÆ°À» Àá½Ã ºñÈ°¼ºÈ­
        startButton.interactable = false;
        // Á¢¼ÓÀ» ½Ãµµ ÁßÀÓÀ» ÅØ½ºÆ®·Î Ç¥½Ã
        lobbyNetworkStatusText.text = "¸¶½ºÅÍ ¼­¹ö¿¡ ¿¬°á Áß...";
        // °ÔÀÓ ¹öÀü Ç¥½Ã
        gameVersionText.text = "¹öÁ¯ " + gameVersion;

        // ¾ÀÀÌ ´Ù½Ã ·ÎµåµÇ¾îµµ ´Ğ³×ÀÓÀÌ ÀÖ´ÂÁö ¾ø´ÂÁö Ã¼Å©
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
        // ½ºÅ¸Æ® ¹öÆ°À» È°¼ºÈ­
        startButton.interactable = true;
        // Á¢¼Ó Á¤º¸ Ç¥½Ã
        lobbyNetworkStatusText.text = "¿Â¶óÀÎ : ¸¶½ºÅÍ ¼­¹ö¿¡ ¿¬°áµÊ";
    }

    // ¸¶½ºÅÍ ¼­¹ö Á¢¼Ó ½ÇÆĞ, ¿¬°á ²÷±æ ½Ã ÀÚµ¿ ½ÇÇà
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ·ë Á¢¼Ó ¹öÆ°À» ºñÈ°¼ºÈ­
        startButton.interactable = false;
        // Á¢¼Ó Á¤º¸ Ç¥½Ã
        lobbyNetworkStatusText.text = "¿ÀÇÁ¶óÀÎ : ¸¶½ºÅÍ ¼­¹ö¿¡ ¿¬°áµÇÁö ¾ÊÀ½\n¿¬°áÀ» ´Ù½Ã ½ÃµµÇÏ´Â Áß...";

        // ¸¶½ºÅÍ ¼­¹ö·ÎÀÇ ÀçÁ¢¼Ó ½Ãµµ
        PhotonNetwork.ConnectUsingSettings();
    }

    // ·ë ¸¸µé±â ½Ãµµ
    public void Connect()
    {
        // Áßº¹ Á¢¼Ó ½Ãµµ¸¦ ¸·±â À§ÇØ, Á¢¼Ó ¹öÆ° Àá½Ã ºñÈ°¼ºÈ­
        joinButton.interactable = false;

        // ¸¶½ºÅÍ ¼­¹ö¿¡ Á¢¼ÓÁßÀÌ¶ó¸é
        if (PhotonNetwork.IsConnected)
        {
            // ¹æ ÀÌ¸§ÀÌ³ª ´Ğ³×ÀÓÀÌ ºñ¾îÀÖ´Ù¸é
            if (startMenuRoomInput.text.Equals("") || nicknameInput.text.Equals(""))
            {
                joinButton.interactable = true;
                // info¸¦ ÀÔ·ÂÇØÁÖ¼¼¿ä
                startMenuNetworkStatusText.text = "´Ğ³×ÀÓ ¶Ç´Â ¹æ Á¦¸ñÀ» ÀÔ·ÂÇÏ¼¼¿ä";
            }
            else
            {
                // ·ë Á¢¼Ó ½ÇÇà
                startMenuNetworkStatusText.text = "¹æ¿¡ Á¢¼ÓÇÏ´Â Áß...";
                PhotonNetwork.JoinRoom(startMenuRoomInput.text);
            }


        }
        else
        {
            // ¸¶½ºÅÍ ¼­¹ö¿¡ Á¢¼ÓÁßÀÌ ¾Æ´Ï¶ó¸é, ¸¶½ºÅÍ ¼­¹ö¿¡ Á¢¼Ó ½Ãµµ
            startMenuNetworkStatusText.text = "¿ÀÇÁ¶óÀÎ : ¸¶½ºÅÍ ¼­¹ö¿¡ ¿¬°áµÇÁö ¾ÊÀ½\n¿¬°áÀ» ´Ù½Ã ½ÃµµÇÏ´Â Áß...";
            // ¸¶½ºÅÍ ¼­¹ö·ÎÀÇ ÀçÁ¢¼Ó ½Ãµµ
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // (¸Â´Â ¹æ ÀÌ¸§ÀÌ ¾ø¾î Or ¹æÀÌ ²Ë Â÷¼­) ·ë Âü°¡¿¡ ½ÇÆĞÇÑ °æ¿ì ÀÚµ¿ ½ÇÇà
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ¹öÆ° ´Ù½Ã È°¼ºÈ­
        joinButton.interactable = true;

        // Á¢¼Ó »óÅÂ Ç¥½Ã
        startMenuNetworkStatusText.text = "¹æÀÌ Á¸ÀçÇÏÁö ¾Ê°Å³ª ÀÌ¹Ì ÀÎ¿øÀÌ °¡µæ Âù ¹æÀÔ´Ï´Ù";
    }

    public void MakeRoom()
    {
        // Áßº¹ ¹æ »ı¼º ¹æÁö
        makeRoomButton.interactable = false;

        // ÀÓ½Ã ½ºÆ®¸µ ·ë³×ÀÓÃ¼Ä¿¿¡ Æ¯¼ö¹®ÀÚ¸¦ Á¦¿ÜÇÑ ¹®ÀÚ¿­À» ³Ö´Â´Ù
        string roomNameChecker = Regex.Replace(roomInput.text, @"[^0-9a-zA-Z°¡-ÆR]{1,10}", "", RegexOptions.Singleline);

        // ¹æ ÀÌ¸§ÀÌ ºñ¾îÀÖ´Ù¸é
        if (roomInput.text.Equals(""))
        {
            makeRoomButton.interactable = true;
            // info¸¦ ÀÔ·ÂÇØÁÖ¼¼¿ä
            networkStatusText.text = "¹æ Á¦¸ñÀ» ÀÔ·ÂÇÏ¼¼¿ä";
        }
        // ·ëÀÎÇ².ÅØ½ºÆ®°¡ ·ë³×ÀÓÃ¼Ä¿¿Í ´Ù¸£´Ù¸é
        else if (roomInput.text.Equals(roomNameChecker) == false)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "¹æ Á¦¸ñ¿¡ Æ¯¼ö¹®ÀÚ°¡ Æ÷ÇÔµÇ¾î ÀÖ½À´Ï´Ù";
        }
        // ¹æ ÀÌ¸§ÀÌ 18ÀÚº¸´Ù Å©´Ù¸é
        else if (roomInput.text.Length > 35)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "¹æ Á¦¸ñÀÌ ³Ê¹« ±é´Ï´Ù\nÃÖ´ë 18ÀÚ±îÁö ¾µ ¼ö ÀÖ½À´Ï´Ù";
        }
        // ¹æ ÀÌ¸§ÀÌ ÇÑ ±ÛÀÚ¶ó¸é
        else if (roomInput.text.Length < 2)
        {
            makeRoomButton.interactable = true;
            roomInput.text.Remove(0, roomInput.text.Length);
            networkStatusText.text = "¹æ Á¦¸ñÀº ÃÖ¼Ò µÎ ±ÛÀÚ ÀÌ»óÀÌ¾î¾ß ÇÕ´Ï´Ù";
        }
        else
        {
            // Á¢¼Ó »óÅÂ Ç¥½Ã
            networkStatusText.text = "¹æÀ» »ı¼ºÇÏ´Â Áß...";
            //µå·Ó ´Ù¿î ¹ë·ù¿¡ 4(ÃÖ¼ÒÀÎ¿ø) ´õÇØÁÖ±â
            roomMaxPlayer = peapleDropdown.value + 4;
            // ¹æ ¸¸µé±â(¹æÀÌ¸§, ¹æ¿É¼Ç{ ¹æ ÃÖ´ëÀÎ¿ø = ¹ÙÀÌÆ® Çüº¯È¯ roomMaxPlayer})
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = byte.Parse(roomMaxPlayer.ToString()) });
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        networkStatusText.text = "°°Àº ÀÌ¸§ÀÇ ¹æÀÌ ÀÌ¹Ì Á¸ÀçÇÕ´Ï´Ù";

        makeRoomButton.interactable = true;
    }

    // ·ë¿¡ Âü°¡ ¿Ï·áµÈ °æ¿ì ÀÚµ¿ ½ÇÇà
    public override void OnJoinedRoom()
    {
        // ¹æ ¸Ş´º Ç¥½Ã
        room.SetActive(true);

        // ÀÌÀü ¸Ş´º(½ºÅ¸Æ® ¸Ş´º, ¹æ ¸¸µé±â ¸Ş´º, ÆË¾÷ÅØ½ºÆ®) ²ô±â
        startMenu.SetActive(false);
        makeRoomMenu.SetActive(false);
        roomPopUpText.text = "";


        // ¸¸¾à ¹æÀåÀÌ¶ó¸é
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }

        // °ÔÀÓ½ÃÀÛ ¹öÆ°, ¹æ ¸¸µé±â ¹öÆ° È°¼ºÈ­
        joinButton.interactable = true;
        makeRoomButton.interactable = true;

        // ½ºÅ¸Æ® ¸Ş´º ´İ±â
        startMenu.SetActive(false);

        // ·ë ÀÌ¸§ ¼³Á¤
        roomTitleText.text = "¹æ Á¦¸ñ : " + PhotonNetwork.CurrentRoom.Name;

        if(PhotonNetwork.IsMasterClient)
        {
            // °ÔÀÓ ½ÃÀÛ ¹öÆ° È°¼ºÈ­
            inGameStartButton.SetActive(true);


        }
        else
        {
            inGameStartButton.SetActive(false);
        }

        
    }

    public void LeaveRoom()
    {
        // ¹æ ³ª°¡±â
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
        //{
        //    roomPopUpText.text = "ÃÖ¼Ò ½ÃÀÛ ÀÎ¿øÀº 4¸íÀÔ´Ï´Ù";
        //}
        //else
        {
            // Áßº¹ Á¢¼Ó ½Ãµµ¸¦ ¸·±â À§ÇØ, °ÔÀÓ½ÃÀÛ ¹öÆ° Àá½Ã ºñÈ°¼ºÈ­
            gameStartButton.interactable = false;
            // ¸ğµç ·ë Âü°¡ÀÚµéÀÌ InGame ¾ÀÀ» ·ÎµåÇÏ°Ô ÇÔ
            PhotonNetwork.LoadLevel("InGame");
        }

        
    }

    // ÇÃ·¹ÀÌ¾î°¡ ¹æ¿¡ Âü¿©ÇÏ¸é ÀÚµ¿ È£Ãâ
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
        }
    }

    // ÇÃ·¹ÀÌ¾î°¡ ¹æ¿¡¼­ ³ª°¡¸é ÀÚµ¿ È£Ãâ
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("NicknameRPC", RpcTarget.All);
            // °ÔÀÓ ½ÃÀÛ ¹öÆ° È°¼ºÈ­
            inGameStartButton.SetActive(true);
        }

    }

    [PunRPC]
    void NicknameRPC()
    {
        // ·ë ÇöÀç ÇÃ·¹ÀÌ¾î/ ÃÖ´ë ÇÃ·¹ÀÌ¾î ¼³Á¤
        roomPlayerNumberText.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // ¹æ ´Ğ³×ÀÓµé Ã»¼ÒÇØÁÖ±â
        for (int i = 0; i < 12; i++)
        {
            nicknamePanel[i].SetActive(false);
            nicknameText[i].text = "";
        }

        // ´Ğ³×ÀÓ ½áÁÖ±â
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // ÇÃ·¹ÀÌ¾î°¡ ³­µ¥ ¹æÀåÀÌ¾ß
            if (PhotonNetwork.PlayerList[i].IsMasterClient && PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (³ª)" + "</color>" + "<color=#E5B400>" + " (¹æÀå)" + "</color>";
            }
            // ÇÃ·¹ÀÌ¾î°¡ ¹æÀåÀÌ¶ó¸é
            else if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#E5B400>" + " (¹æÀå)" +"</color>";
            }
            // ÇÃ·¹ÀÌ¾î°¡ ³ª¶ó¸é
            else if (PhotonNetwork.PlayerList[i].IsLocal)
            {
                nicknamePanel[i].SetActive(true);
                nicknameText[i].text = PhotonNetwork.PlayerList[i].NickName + "<color=#B5B5B5>" + " (³ª)" + "</color>";
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

    // ·Îºñ °ÔÀÓ ½ÃÀÛ ¹öÆ°À» ´­·¶À» ¶§
    public void StartMenuClick()
    {
        // ´Ğ³×ÀÓÀ» Á¤Çß´Ù¸é
        if (isNickname)
        {
            // È¯¿µ ¸Ş½ÃÁö
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "´Ô È¯¿µÇÕ´Ï´Ù";
            // ½ºÅ¸Æ® ¸Ş´º¸¦ ¶ç¿î´Ù
            startMenu.SetActive(true);
        }
        // ´Ğ³×ÀÓÀ» Á¤ÇÏÁö ¾Ê¾Ò´Ù¸é
        else
        {
            // ÀÌ¸§À» ¹¯´Â´Ù
            setNicknameMenuPopUpText.text = "´ç½ÅÀÇ ÀÌ¸§Àº ¹«¾ùÀÎ°¡¿ä?";
            // ¼Â ´Ğ³×ÀÓ ¸Ş´º¸¦ ¶ç¿î´Ù
            setNicknameMenu.SetActive(true);
        }
    }

    // ´Ğ³×ÀÓ È®ÀÎ ¹öÆ°À» ´­·¶À» ¶§
    public void SetNickname()
    {
        // ÀÓ½Ã ½ºÆ®¸µ ´Ğ³×ÀÓÃ¼Ä¿¿¡ Æ¯¼ö¹®ÀÚ¿Í ¼ıÀÚ¸¦ Á¦¿ÜÇÑ ¹®ÀÚ¿­À» ³Ö´Â´Ù
        string nicknameChecker = Regex.Replace(nicknameInput.text, @"[^a-zA-Z°¡-ÆR]", "", RegexOptions.Singleline);


        // ´Ğ³×ÀÓÀ» ¾È ½è´Ù¸é
        if (nicknameInput.text.Equals(""))
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "´Ğ³×ÀÓÀ» ÀÔ·ÂÇØÁÖ¼¼¿ä";
        }
        // ´Ğ³×ÀÓÀÎÇ².ÅØ½ºÆ®°¡ ´Ğ³×ÀÓÃ¼Ä¿¿Í ´Ù¸£´Ù¸é
        else if (nicknameInput.text.Equals(nicknameChecker) == false)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "´Ğ³×ÀÓ¿¡ ¼ıÀÚ ¶Ç´Â Æ¯¼ö¹®ÀÚ°¡ Æ÷ÇÔµÇ¾î ÀÖ½À´Ï´Ù";
        }
        // ´Ğ³×ÀÓÀÌ 35ÀÚº¸´Ù Å©´Ù¸é
        else if (nicknameInput.text.Length > 35)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "´Ğ³×ÀÓÀÌ ³Ê¹« ±é´Ï´Ù\nÃÖ´ë 35ÀÚ±îÁö ¾µ ¼ö ÀÖ½À´Ï´Ù";
        }
        // ´Ğ³×ÀÓÀÌ ÇÑ ±ÛÀÚ¶ó¸é
        else if (nicknameInput.text.Length < 2)
        {
            nicknameInput.text.Remove(0, nicknameInput.text.Length);
            setNicknameMenuPopUpText.text = "´Ğ³×ÀÓÀº ÃÖ¼Ò µÎ ±ÛÀÚ ÀÌ»óÀÌ¾î¾ß ÇÕ´Ï´Ù";
        }
        else
        {
            // ·ÎÄÃ ÇÃ·¹ÀÌ¾îÀÇ ÀÌ¸§À» ´Ğ³×ÀÓ ÀÎÇ² ÅØ½ºÆ®·Î ÇÑ´Ù
            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
            // isNicknameÀ» true·Î ÇÑ´Ù
            isNickname = true;
            // È¯¿µ ¸Ş½ÃÁö
            startMenuNetworkStatusText.text = PhotonNetwork.LocalPlayer.NickName + "´Ô È¯¿µÇÕ´Ï´Ù";
            // ¼Â ´Ğ³×ÀÓ ¸Ş´º¸¦ ²ö´Ù
            setNicknameMenu.SetActive(false);
            // ½ºÅ¸Æ® ¸Ş´º¸¦ ÄÒ´Ù
            startMenu.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
