using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PigeonBeatdown : MonoBehaviour
{
    [Header("UI 연결")]
    public Slider hpSlider;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pigeonNameText;
    public TextMeshProUGUI criticalText;

    public Image currentPigeon;
    public Sprite normalPigeon;
    public Sprite hitPigeon;
    public Sprite deadPigeon;
    public Sprite rarePigeon;
    public Sprite rareHitPigeon;
    public Sprite rareDeadPigeon;

    public Image currentCat;
    public Sprite NormalCat;
    public Sprite HitCat;

    [Header("게임 설정")]
    public float timeLimit = 10f; // 제한 시간
    public int critChance = 20;   // 크리티컬 확률 (20%)
    public int rareChance = 10;   // 레어 비둘기 확률 (10%)
    public float critDisplayTime = 0.2f; // 크리티컬 보여주는 시간

    private int currentHp;
    private int maxHp;
    private float currentTime;
    private int killCount = 0;
    private bool isPlaying = false;

    void Start()
    {
        currentTime = timeLimit;
        killCount = 0;
        isPlaying = true;
        if (criticalText != null)
            criticalText.gameObject.SetActive(false); // 시작할 때는 꺼두기
        SpawnPigeon(); // 첫 비둘기 소환
    }

    void Update()
    {
        if (!isPlaying) return;

        // 타이머 처리
        currentTime -= Time.deltaTime;
        timerText.text = $"남은 시간 : {Mathf.Ceil(currentTime)}초";

        if (currentTime <= 0)
        {
            EndGame();
            return;
        }

        // 스페이스바 타격 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 1. 맞는 이미지로 변경
            if (currentPigeon.sprite == normalPigeon)
                currentPigeon.sprite = hitPigeon;
            else if (currentPigeon.sprite == rarePigeon)
                currentPigeon.sprite = rareHitPigeon;
            currentCat.sprite = HitCat;

            HitPigeon();

            // 2. 이전에 예약된 원상복구 타이머가 있다면 취소하고 다시 1초 뒤로 갱신
            CancelInvoke("ResetPigeonSprite");
            Invoke("ResetPigeonSprite", 0.2f);
        }
    }
    void ResetPigeonSprite()
    {
        // 체력 최대치를 기준으로 현재 비둘기가 레어인지 일반인지 판단하여 복구
        if (maxHp == 20)
            currentPigeon.sprite = rarePigeon;
        else
            currentPigeon.sprite = normalPigeon;
        currentCat.sprite = NormalCat;
    }

    void HitPigeon()
    {
        int damage = 1;


        // 크리티컬 판정
        if (Random.Range(1, 101) <= critChance)
        {
            damage = 2;
            ShowCriticalEffect(); // 👈 크리티컬 연출 함수 실행
            // TODO: 크리티컬 이펙트나 텍스트 띄우기 연출 추가 가능
        }

        currentHp -= damage;
        hpSlider.value = currentHp;

        // 처치 완료 시
        if (currentHp <= 0)
        {
            killCount++;
            scoreText.text = $"처치한 비둘기 수: {killCount}";
            SpawnPigeon(); // 바로 다음 비둘기 소환
        }
    }

    void ShowCriticalEffect()
    {
        if (criticalText == null) return;

        criticalText.gameObject.SetActive(true); // 텍스트 켜기

        // 연타 시 타이머가 꼬이지 않도록 기존 예약 취소 후 재등록
        CancelInvoke("HideCriticalText");
        Invoke("HideCriticalText", critDisplayTime);
    }

    // 👈 지정된 시간 뒤에 호출되어 텍스트를 끄는 함수
    void HideCriticalText()
    {
        if (criticalText != null)
            criticalText.gameObject.SetActive(false);
    }

    void SpawnPigeon()
    {
        // 레어 비둘기 판정
        if (Random.Range(1, 101) <= rareChance)
        {
            maxHp = 20;
            currentPigeon.sprite = rarePigeon;
            pigeonNameText.text = "모자 비둘기";
            pigeonNameText.color = Color.yellow;
        }
        else
        {
            maxHp = 10;
            currentPigeon.sprite = normalPigeon;
            pigeonNameText.text = "아가 비둘기";
            pigeonNameText.color = Color.white;
        }

        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    void EndGame()
    {
        isPlaying = false;
        timerText.text = "종료!";

        if (criticalText != null)
            criticalText.gameObject.SetActive(false); // 종료 시 끄기

        // 처치 수에 비례한 보상 계산 (예: 1마리당 10골드)
        int rewardGold = killCount * 10;
        scoreText.text = $"최종 처치: {killCount}마리\n보상: {rewardGold} 골드 획득!";
    }
}