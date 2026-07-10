using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 제어하려면 이 줄이 반드시 필요합니다!
using TMPro; // TextMeshPro를 제어하기 위해 반드시 추가해야 함

public class TrashCanMinigame : MonoBehaviour
{
    // 유니티 화면(인스펙터)에서 끌어다 놓을 결과창 텍스트
    public TextMeshProUGUI resultText;

    // 새로 추가하는 변수 2개
    public Image backgroundImage; // 색이나 이미지가 바뀔 타겟 배경 UI
    public Sprite normalBackgroundSprite; // 기존 이미지 파일
    public Sprite specialBackgroundSprite; // 10% 당첨 시 띄워줄 이미지 파일

    // 쓰레기통 버튼을 클릭했을 때 실행될 함수 
    // (유니티 버튼과 연결하려면 반드시 public이어야 함)
    public void RollGacha()
    {
        // 1부터 100까지의 랜덤 숫자 뽑기
        int chance = Random.Range(1, 101);

        if (chance <= 60) // 60% 확률
        {
            resultText.text = "기본 재료(생선) 획득!";
            resultText.color = new Color32(128, 115, 142, 255);
            if (backgroundImage != null && normalBackgroundSprite != null)
            {
                backgroundImage.sprite = normalBackgroundSprite;
            }
        }
        else if (chance <= 90) // 30% 확률 (61~90)
        {
            resultText.text = "고급 재료(연어) 획득!";
            resultText.color = new Color32(195, 96, 207, 255);
            if (backgroundImage != null && normalBackgroundSprite != null)
            {
                backgroundImage.sprite = normalBackgroundSprite;
            }
        }
        else // 10% 확률 (91~100)
        {
            resultText.text = "최고급 재료(황금 비둘기) 획득!!";
            resultText.color = new Color32(255, 197, 34, 255);
            if (backgroundImage != null && specialBackgroundSprite != null)
            {
                backgroundImage.sprite = specialBackgroundSprite;
            }
        }
    }
}