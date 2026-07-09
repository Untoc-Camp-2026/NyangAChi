using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [Header("설정창 UI 패널")]
    public GameObject settingPanel;
    public GameObject warningModalPanel; // 저장 안 하고 닫을 때 띄울 경고 패널
    public GameObject saveNoticePanel;    // 저장 성공 시 띄울 안내 패널

    [Header("BGM 스프라이트 (0:음소거, 1:재생)")]
    public Sprite[] bgmSprites;

    [Header("SFX 스프라이트 (0:음소거, 1:소, 2:중, 3:대)")]
    public Sprite[] sfxSprites;

    [Header("배경음(BGM) 컴포넌트")]
    public Slider bgmSlider;
    public Image bgmIconImage;
    private float lastBgmValue = 0.5f;

    [Header("효과음(SFX) 컴포넌트")]
    public Slider sfxSlider;
    public Image sfxIconImage;
    private float lastSfxValue = 0.5f;

    [Header("오디오 소스")]
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    private float currentSavedBGM;
    private float currentSavedSFX;
    private bool isInitializing = false;

    void Start()
    {
        // 첫 게임 시작 시 모든 패널 확실히 비활성화
        if (settingPanel != null) settingPanel.SetActive(false);
        if (warningModalPanel != null) warningModalPanel.SetActive(false);
        if (saveNoticePanel != null) saveNoticePanel.SetActive(false);

        isInitializing = true;

        currentSavedBGM = PlayerPrefs.GetFloat("BGM_Volume", 0.5f);
        currentSavedSFX = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.value = currentSavedBGM;
            bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
            UpdateBgmIcon(currentSavedBGM);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.value = currentSavedSFX;
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
            UpdateSfxIcon(currentSavedSFX);
        }

        ApplyAudioVolume(currentSavedBGM, currentSavedSFX);

        isInitializing = false;
    }

    public void OnBgmSliderChanged(float value)
    {
        if (isInitializing) return;
        UpdateBgmIcon(value);
        if (bgmAudioSource != null) bgmAudioSource.volume = value;
    }

    public void OnSfxSliderChanged(float value)
    {
        if (isInitializing) return;
        UpdateSfxIcon(value);
        if (sfxAudioSource != null) sfxAudioSource.volume = value;
    }

    public void OnClickBgmMuteToggle()
    {
        if (bgmSlider == null) return;

        if (bgmSlider.value > 0f)
        {
            lastBgmValue = bgmSlider.value;
            bgmSlider.value = 0f;
        }
        else
        {
            bgmSlider.value = (lastBgmValue > 0f) ? lastBgmValue : 0.5f;
        }
        UpdateBgmIcon(bgmSlider.value);
    }

    public void OnClickSfxMuteToggle()
    {
        if (sfxSlider == null) return;

        if (sfxSlider.value > 0f)
        {
            lastSfxValue = sfxSlider.value;
            sfxSlider.value = 0f;
        }
        else
        {
            sfxSlider.value = (lastSfxValue > 0f) ? lastSfxValue : 0.5f;
        }
        UpdateSfxIcon(sfxSlider.value);
    }

    public void OnClickSaveButton()
    {
        if (bgmSlider == null || sfxSlider == null) return;

        currentSavedBGM = bgmSlider.value;
        currentSavedSFX = sfxSlider.value;

        PlayerPrefs.SetFloat("BGM_Volume", currentSavedBGM);
        PlayerPrefs.SetFloat("SFX_Volume", currentSavedSFX);
        PlayerPrefs.Save();

        if (saveNoticePanel != null)
        {
            saveNoticePanel.SetActive(true);
        }
    }

    public void OnClickSaveNoticeConfirm()
    {
        if (saveNoticePanel != null)
        {
            saveNoticePanel.SetActive(false);
        }
    }

    public void OnClickCloseButton()
    {
        if (bgmSlider == null || sfxSlider == null) return;

        if (Mathf.Abs(bgmSlider.value - currentSavedBGM) > 0.01f || Mathf.Abs(sfxSlider.value - currentSavedSFX) > 0.01f)
        {
            if (warningModalPanel != null) warningModalPanel.SetActive(true);
        }
        else
        {
            CloseSetting();
        }
    }

    public void OnClickWarningConfirm()
    {
        isInitializing = true;
        bgmSlider.value = currentSavedBGM;
        sfxSlider.value = currentSavedSFX;
        UpdateBgmIcon(currentSavedBGM);
        UpdateSfxIcon(currentSavedSFX);
        ApplyAudioVolume(currentSavedBGM, currentSavedSFX);
        isInitializing = false;

        if (warningModalPanel != null) warningModalPanel.SetActive(false);
        CloseSetting();
    }

    public void OnClickWarningCancel()
    {
        if (warningModalPanel != null) warningModalPanel.SetActive(false);
    }

    private void UpdateBgmIcon(float value)
    {
        if (bgmIconImage == null || bgmSprites.Length < 2) return;
        bgmIconImage.sprite = (value <= 0f) ? bgmSprites[0] : bgmSprites[1];
    }

    private void UpdateSfxIcon(float value)
    {
        if (sfxIconImage == null || sfxSprites.Length < 4) return;

        if (value <= 0f) sfxIconImage.sprite = sfxSprites[0];
        else if (value <= 0.33f) sfxIconImage.sprite = sfxSprites[1];
        else if (value <= 0.66f) sfxIconImage.sprite = sfxSprites[2];
        else sfxIconImage.sprite = sfxSprites[3];
    }

    private void ApplyAudioVolume(float bgm, float sfx)
    {
        if (bgmAudioSource != null) bgmAudioSource.volume = bgm;
        if (sfxAudioSource != null) sfxAudioSource.volume = sfx;
    }

    // 설정창을 켤 때 내부 하위 모달들은 확실하게 꺼지도록 수정
    public void OpenSetting()
    {
        if (warningModalPanel != null) warningModalPanel.SetActive(false);
        if (saveNoticePanel != null) saveNoticePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    // 설정창을 닫을 때 내부 하위 모달들도 깨끗하게 정리
    public void CloseSetting()
    {
        if (warningModalPanel != null) warningModalPanel.SetActive(false);
        if (saveNoticePanel != null) saveNoticePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);
    }
}