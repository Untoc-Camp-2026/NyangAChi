using UnityEngine;

public class SettingController : MonoBehaviour
{
    [Header("설정창 UI 팝업 패널")]
    public GameObject settingPannel;

    void Start()  // 게임 켜질 때 설정창 꺼져있도록 (안전장치)
    {
        if (settingPannel != null)
        {
            settingPannel.SetActive(false);
        }
    }

    public void OpenSetting()
    {
        if (settingPannel != null)
        {
            settingPannel.SetActive(true);
        }
    }

    public void CloseSetting()
    {
        if (settingPannel != null)
        {
            settingPannel.SetActive(false);
        }
    }
}
