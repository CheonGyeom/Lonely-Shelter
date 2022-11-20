using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string vAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string hAxisName = "Horizontal"; // �¿� �������� ���� �Է��� �̸�
    public string jumpButtonName = "Jump"; // ������ ���� �Է� ��ư �̸�
    public string fireButtonName = "Fire1"; // �߻縦 ���� �Է� ��ư �̸�

    public float Zmove { get; private set; } // ������ ������ �Է°�
    public float Xmove { get; private set; } // ������ ������ �Է°�
    public bool jump { get; private set; } // ������ ���� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�


    void Update()
    {
        Zmove = Input.GetAxis(vAxisName);
        Xmove = Input.GetAxis(hAxisName);
        jump = Input.GetButton(jumpButtonName);
        //fire�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName);
    }
}
