using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]private float secondPerRealTimrSecond;

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc; // 증감량 비율

    [SerializeField] private float nightFogDensity; // 밤 상태의 Fog 밀도
    private float dayFogDensity; // 낮 상태의 Fog 밀도
    private float currentFogDensity; //계산

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }


    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimrSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else if (transform.eulerAngles.x >= 0)
        {
            isNight = false;
        }

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }


        }
    }
}
