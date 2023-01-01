using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수


    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 

    public GameObject escMenu; // ESC 메뉴

    


    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ESC 메뉴 활성화
    public void SetActiveEscMenu(bool active)
    {
        escMenu.SetActive(active);
    }

    public void DisableEscMenu()
    {
        // ESC 메뉴를 닫는다
        UIManager.instance.SetActiveEscMenu(false);
        // 마우스 커서 잠금, 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 메인 화면으로 나가기
    public void GoMain()
    {
        // 방 나가기
        PhotonNetwork.LeaveRoom();

    }

    public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            SceneManager.LoadScene("Main");
            return;
        }
    }

}
