using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

public class IntroStoryController : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject titleGroup;    // 타이틀 화면 전체 묶음
    public GameObject storyPanel;    // 스토리 화면 전체 묶음
    public TMP_Text storyText;           // 대사 글자 텍스트

    [Header("스토리 연출용 이미지 컴포넌트")]
    public Image bgImageComponent;   // 뒷배경 이미지 컴포넌트
    public Image catImageComponent;  // 고양이 캐릭터 이미지 컴포넌트

    [Header("리소스 등록용 배열")]
    // 유니티 에디터에서 연수님이 그려준 사진들을 순서대로 넣을 칸
    public Sprite[] backgroundSprites;
    public Sprite[] catSprites;

    [Header("스토리 데이터")]
    private string[] stories = new string[]
    {
        "내 이름은 나비. 얼마 전까지만 해도 차가운 아스팔트 위를 떠돌던 길고양이었다냥.", // 0번대사
        "운 좋게 상냥한 집사를 만나 따뜻한 집고양이가 되었고, \n매일 포근한 침대 속에서 행복한 나날을 보내고 있지.", // 1번
        "하지만... 푹신한 이불에 누울 때마다 자꾸만 눈에 밟히는 얼굴들이 있다냥.", // 2번
        "지금 이 순간에도 매서운 밤바람을 맞으며 쓰레기통을 뒤지고 있을, 내 옛 길거리 친구들...", // 3번
        "녀석들에게도 배부르고 따뜻한 밤을 선물해 주고 싶다냥! 그래서 결심했다냥.", // 4번
        "인간 집사가 모두 잠든 깊은 밤, 주방 문을 몰래 열고 우리들만의 비밀 음식점 '묘수'를 시작하기로!", // 5번
        "친구들에게 최고의 요리를 대접하려면 낮 동안 부지런히 신선한 재료를 모아야 한다냥.", // 6번
        "동네 쓰레기통도 은밀히 수색하고, 가끔은 애교를 부려 인간들의 간식도 받아내야지!", // 7번
        "오늘 밤도 배고픈 친구들이 하나둘씩 찾아올 거다냥. \n자, 서둘러 주방으로 가서 밤손님들을 맞이할 준비를 해보자구!" // 8번
    };

    private int currentIndex = 0;
    public GameObject settingPanel;

    void Start()
    {
        int firstCheck = PlayerPrefs.GetInt("IsFirstTime", 1);

      /*  if (firstCheck == 1)
        {*/
            if (titleGroup != null) titleGroup.SetActive(false);
            if (storyPanel != null) storyPanel.SetActive(true);

            currentIndex = 0;
            ApplySceneDirection(); // 첫 연출 적용
       /* }
        else
        {
            if (titleGroup != null) titleGroup.SetActive(true);
            if (storyPanel != null) storyPanel.SetActive(false);
        }*/
    }

    public void OnClickNext()
    {
        currentIndex++;

        if (currentIndex < stories.Length)
        {
            ApplySceneDirection(); // 대사가 넘어가기 전 연출부터 바꾸기
        }
        else
        {
            Debug.Log("[Story] 최초 스토리 종료 -> 타이틀 화면 활성화");
            PlayerPrefs.SetInt("IsFirstTime", 0);
            PlayerPrefs.Save();

            storyPanel.SetActive(false);
            titleGroup.SetActive(true);
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("KitchenScene");
    }

    public void OpenSetting()
    {
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        if (settingPanel != null) settingPanel.SetActive(false);  
    }

    // 대사 번호(currentIndex)에 따라 배경과 고양이 사진을 바꿔주는 함수
    private void ApplySceneDirection()
    {
        storyText.text = stories[currentIndex];

        // [연출 기획 제안]
        // 0번 대사: 슬픈 분위기 (어두운 골목 배경, 우는 고양이)
        // 1~2번 대사: 행복한 분위기 (따뜻한 거실 배경, 웃는 고양이)
        // 3~4번 대사: 다시 슬픈 회상 (어두운 골목 배경, 걱정하는 고양이)
        // 5~8번 대사: 당찬 다짐 (식당 내부 배경, 요리사 모자 쓴 고양이)

        if (currentIndex == 0)
        {
            ChangeVisual(0, 0); // 배경 0번, 고양이 0번 사진으로 변경
        }
        else if (currentIndex == 1 || currentIndex == 2)
        {
            ChangeVisual(1, 1); // 배경 1번, 고양이 1번 사진으로 변경
        }
        else if (currentIndex == 3 || currentIndex == 4)
        {
            ChangeVisual(0, 2); // 배경 0번, 고양이 2번 사진으로 변경
        }
        else if (currentIndex >= 5)
        {
            ChangeVisual(2, 3); // 배경 2번, 고양이 3번 사진으로 변경
        }
    }

    // 실제 컴포넌트의 이미지를 챡 갈아끼워주는 안전장치 기능
    private void ChangeVisual(int bgIndex, int catIndex)
    {
        if (backgroundSprites != null && bgIndex < backgroundSprites.Length)
            bgImageComponent.sprite = backgroundSprites[bgIndex];

        if (catSprites != null && catIndex < catSprites.Length)
            catImageComponent.sprite = catSprites[catIndex];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("<color=yellow>[System] 데이터 초기화 완료!</color>");
        }
    }
}