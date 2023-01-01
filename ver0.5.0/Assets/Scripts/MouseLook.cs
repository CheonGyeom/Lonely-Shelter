using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviourPun
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVer = 9.0f;

    public float minimumVer = -45.0f;
    public float maximumVer = 45.0f;

    float _rotationX = 0;
    void Start()
    {
        Camera playerCamera = GetComponent<Camera>();
        if (!photonView.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
        }


        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }


    void Update()
    {
        // ����䰡 �ڽ��� ���� �ƴϰų� ���ӿ��� �޴��� Ȱ��ȭ�Ǿ��ų� ESC �޴��� Ȱ��ȭ�Ǿ��ٸ�
        if (!photonView.IsMine || UIManager.instance.gameoverUI.activeSelf || UIManager.instance.escMenu.activeSelf)
        {
            return;
        }



        MouseRotation();

    }

    private void MouseRotation()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityHor, 0f);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            _rotationX = Mathf.Clamp(_rotationX, minimumVer, maximumVer);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0f);
        }
    }

}
