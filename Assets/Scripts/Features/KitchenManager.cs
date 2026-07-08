using System.Collections;
using UnityEngine;
using TMPro; // 텍스트 매시 프로(TextMeshPro) UI를 제어하기 위해 필수입니다.

public class OrderManager : MonoBehaviour
{
    [Header("화면 UI 패널")]
    public GameObject counterPanel;       // 카운터(주문) 화면 패널
    public GameObject kitchenPanel;       // 주방(조리) 화면 패널

    [Header("손님 대화 UI")]
    public TextMeshProUGUI orderText;     // 손님의 대사가 표시될 텍스트 컴포넌트
    
    [Header("글자 출력 속도")]
    public float typingSpeed = 0.05f; // 글자 간의 간격 (초 단위)

    // 타자기 효과를 실행할 코루틴 함수
    IEnumerator TypeText(string textToType)
    {
        // 1. 우선 텍스트창에 전체 대사를 다 넣습니다.
        orderText.text = textToType;
        
        // 2. 눈에 보이는 글자 수를 0개로 만듭니다. (글자가 순간적으로 다 숨겨짐)
        orderText.maxVisibleCharacters = 0;

        // 3. 텍스트가 유니티 내부에서 완전히 정렬될 때까지 딱 1프레임 기다려줍니다.
        yield return null;

        // 4. 전체 글자 수(인식된 글자)를 가져옵니다.
        int totalVisibleCharacters = orderText.textInfo.characterCount;

        // 5. 반복문을 돌며 글자를 한 글자씩 보이게 만듭니다.
        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            orderText.maxVisibleCharacters = i; // 현재 순서만큼 글자를 노출
            
            // typingSpeed(0.05초)만큼 잠깐 쉬었다가 다음 루프(글자)로 넘어갑니다.
            yield return new WaitForSeconds(typingSpeed); 
        }
    }
    
    // 손님들의 주문 대사 목록 (배열)
    private string[] orderDialogues = new string[]
    {
        "페퍼로니 피자 하나 주세요.",
        "치즈 피자 한 판 부탁해요. 소스랑 치즈만 올려서요.",
        "치즈랑 페퍼로니 둘 다 듬뿍 들어간 피자 주세요!"
    };

    // 현재 손님이 주문한 내용을 저장하는 변수 (나중에 채점할 때 사용)
    public static string currentOrder = "";

    void Start()
    {
        // 게임 시작 시 초기 화면 설정
        counterPanel.SetActive(true);  // 카운터 화면 켜기
        kitchenPanel.SetActive(false); // 주방 화면 끄기

        // 첫 번째 손님의 주문을 생성합니다.
        GenerateNewOrder();
    }

    /// <summary>
    /// 무작위 주문을 생성하여 대화창에 띄우는 함수
    /// </summary>
    public void GenerateNewOrder()
    {
        int randomIndex = Random.Range(0, orderDialogues.Length);
        currentOrder = orderDialogues[randomIndex];

        // [중요] 코루틴 함수는 그냥 실행하면 안 되고, 반드시 StartCoroutine()으로 감싸서 실행해야 합니다!
        StartCoroutine(TypeText(currentOrder));
    }

    /// <summary>
    /// '피자 만들기' 버튼을 클릭했을 때 호출될 함수
    /// </summary>
    public void OnClickStartCooking()
    {
        // 화면을 전환합니다.
        counterPanel.SetActive(false); // 카운터 화면 끄기
        kitchenPanel.SetActive(true);  // 주방 화면 켜기
    }
}