using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public string vAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string hAxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string jumpButtonName = "Jump"; // 점프를 위한 입력 버튼 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름

    public float Zmove { get; private set; } // 감지된 움직임 입력값
    public float Xmove { get; private set; } // 감지된 움직임 입력값
    public bool jump { get; private set; } // 감지된 점프 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값


    private void Update()
    {
        // 로컬 플레이어가 아닌 경우 입력을 받지 않음
        if (!photonView.IsMine)
        {
            return;
        }

        Zmove = Input.GetAxis(vAxisName);
        Xmove = Input.GetAxis(hAxisName);
        jump = Input.GetButton(jumpButtonName);
        //fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
    }
}
