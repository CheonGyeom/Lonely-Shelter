using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �ڵ�
using UnityEngine.UI; // UI ���� �ڵ�
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    // �̱��� ���ٿ� ������Ƽ
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

    private static UIManager m_instance; // �̱����� �Ҵ�� ����


    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 

    public GameObject escMenu; // ESC �޴�

    


    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ESC �޴� Ȱ��ȭ
    public void SetActiveEscMenu(bool active)
    {
        escMenu.SetActive(active);
    }

    public void DisableEscMenu()
    {
        // ESC �޴��� �ݴ´�
        UIManager.instance.SetActiveEscMenu(false);
        // ���콺 Ŀ�� ���, Ŀ�� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ���� ȭ������ ������
    public void GoMain()
    {
        // �� ������
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
