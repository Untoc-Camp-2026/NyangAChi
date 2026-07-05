using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ★ 중요: 씬을 바꿀 때 반드시 필요한 네임스페이스입니다!

public class KitchenManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI resultText;

    private int customerCount = 0; // 현재까지 응대한 손님 수
    private int maxCustomers = 2;  // 오늘 방문할 총 손님 수 (MVP니까 일단 2명으로 설정)

    void Start()
    {
        // 게임이 시작되면 첫 번째 손님을 받습니다.
        NextCustomer();
    }

    // 다음 손님을 불러오는 함수
    void NextCustomer()
    {
        customerCount++;
        resultText.text = "요리 중...";

        if (customerCount == 1)
        {
            orderText.text = "페퍼로니 피자 한 판 주세요!";
        }
        else if (customerCount == 2)
        {
            orderText.text = "치즈 피자 주세요!";
        }
    }

    // 플레이어가 [제출] 버튼을 누르면 실행되는 함수
    public void OnClickServeButton()
    {
        // 만약 아직 오늘 목표한 손님 수를 채우지 못했다면
        if (customerCount < maxCustomers)
        {
            resultText.text = "성공! 다음 손님이 들어옵니다.";
            // 1.5초 뒤에 다음 손님이 오도록 대기 시간을 줍니다 (Invoke 함수 사용)
            Invoke("NextCustomer", 1.5f);
        }
        else
        {
            // 오늘 올 손님이 더 없다면 영업 종료!
            orderText.text = "오늘 손님은 여기까지입니다!";
            resultText.text = "정산 중... 잠시 후 상점으로 이동합니다.";
            
            // 2초 뒤에 상점 씬으로 넘어가는 함수를 실행합니다.
            Invoke("GoToStoreScene", 2.0f);
        }
    }

    // 진짜로 상점 씬으로 화면을 전환하는 함수
    void GoToStoreScene()
    {
        // 빌드 세팅에 등록한 상점 씬의 정확한 파일 이름을 적어줍니다.
        SceneManager.LoadScene("StoreScene");
    }
}