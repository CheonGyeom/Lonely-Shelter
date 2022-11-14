using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]private float secondPerRealTimrSecond; // ���ӽð��� 100�� = ���� �ð��� 1��

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc; // ������ ����

    [SerializeField] private float nightFogDensity; // �� ������ Fog �е�
    private float dayFogDensity; // �� ������ Fog �е�
    private float currentFogDensity; //���

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimrSecond * Time.deltaTime);

        if(transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else if(transform.eulerAngles.x >= 0)
        {
            isNight = false;
        }

        if (isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if(currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
