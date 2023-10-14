using System;
using UnityEngine;
using YG;

public class UserDataReader : MonoBehaviour
{
    public static Action<bool> MobileDeviceDefineEvent;

    private void OnEnable() => YandexGame.GetDataEvent += GetData;

    private void OnDisable() => YandexGame.GetDataEvent -= GetData;

    private void Awake()
    {
        if (YandexGame.SDKEnabled == true)
        {
            GetData();
        }
    }

    private void GetData()
    {
        MobileDeviceDefineEvent?.Invoke(YandexGame.EnvironmentData.isMobile);
    }
}