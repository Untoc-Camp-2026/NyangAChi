using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("UI 연결")]
    public RectTransform cursor;       // 왔다갔다 할 커서
    public RectTransform successZone;  // 성공 구간 (초록색 바)

    public Image currentNyang;
    public Sprite normalNyang;
    public Sprite successNyang;
    public Sprite failNyang;

    [Header("설정")]
    public float speed = 1.5f;         // 커서 이동 속도
    public float gaugeWidth = 400f;    // 배경 게이지 전체 너비
    private float restartCooldown = 0f;

    private bool isStopped = false;
    private bool canInput = true;

    void Update()
    {
        if (restartCooldown > 0)
        {
            restartCooldown -= Time.deltaTime;
            return;
        }

        if (isStopped) return;

        // 1. 커서 좌우 왕복 이동 (Mathf.PingPong)
        // 0~1 사이를 왕복하는 값을 -0.5 ~ +0.5로 변환하여 가운데 정렬
        float pingpong = Mathf.PingPong(Time.time * speed, 1f) - 0.5f;
        // Y값은 현재 커서가 가지고 있는 원래 Y위치(cursor.anchoredPosition.y)를 그대로 유지하도록 변경!
        cursor.anchoredPosition = new Vector2(pingpong * gaugeWidth, cursor.anchoredPosition.y);

        // 2. 스페이스바 타격 시 판정
        if (canInput && Input.GetKeyDown(KeyCode.Space))
        {
            isStopped = true;
            CheckFishingResult();
        }
    }

    void CheckFishingResult()
    {
        // 커서가 정중앙(0)에서 얼마나 떨어져 있는지 절대값으로 계산
        float cursorDistanceFromCenter = Mathf.Abs(cursor.anchoredPosition.x);

        // 성공 구간의 절반 길이 (예: 너비가 100이면 중심에서 좌우로 50)
        float hitZoneHalf = successZone.rect.width / 4f;

        // 커서가 성공 구간 안에 들어왔는지 판정
        if (cursorDistanceFromCenter <= hitZoneHalf)
        {
            Debug.Log("낚시 성공! 고양이 기뻐하는 모습 출력!");
            currentNyang.sprite = successNyang;
            // TODO: 성공 스프라이트로 변경 및 보상 지급
        }
        else
        {
            Debug.Log("낚시 실패... 고양이 시무룩한 모습 출력");
            currentNyang.sprite = failNyang;
            // TODO: 실패 스프라이트로 변경
        }
    }
    // 👈 스크립트 맨 아래에 이 함수를 추가하세요
    public void RestartGame()
    {
        restartCooldown = 0.1f;

        isStopped = false;
        currentNyang.sprite = normalNyang;
        

        Debug.Log("낚시 다시 시작!");
        Invoke(nameof(EnableInput), 0.2f);
    }

    void EnableInput()
    {
        canInput = true;
    }
}

