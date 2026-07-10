using UnityEngine;
using TMPro;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    [Header("화면 UI 패널")]
    public GameObject counterPanel;
    public GameObject kitchenPanel;

    [Header("손님 대화 UI")]
    public TextMeshProUGUI orderText;
    public float typingSpeed = 0.05f; 

    [Header("손님 말투 다양화 설정 (입장)")]
    public List<string> orderSuffixes = new List<string>()
    {
        " 주세요!"
    };
    
    [Header("요리 성공 퇴장 대사 목록")]
    public List<string> successLines = new List<string>()
    {
        "최고의 요리네요! 감사합니다."
    };

    [Header("요리 실패 퇴장 대사 목록")]
    public List<string> failureLines = new List<string>()
    {
        "음... 제가 원한 건 이게 아닌데요."
    };

    [Header("전체 레시피 데이터베이스")]
    public List<RecipeData> allRecipes = new List<RecipeData>();
    public static RecipeData currentRecipe;

    [Header("주방 UI 매니저 연결")]
    public RecipeUIManager recipeUIManager;

    [Header("손님 애니메이션 연결")]
    public CustomerAnimator customerAnimator; 

    void Start()
    {
        NextOrder(); 
    }

    public void NextOrder()
    {
        kitchenPanel.SetActive(false);
        counterPanel.SetActive(true);

        if (customerAnimator != null)
        {
            StartCoroutine(customerAnimator.EnterAnimation());
        }

        if (allRecipes.Count == 0) return;

        int randomIndex = Random.Range(0, allRecipes.Count);
        currentRecipe = allRecipes[randomIndex];

        string randomSuffix = " 주세요!"; 
        if (orderSuffixes != null && orderSuffixes.Count > 0)
        {
            int suffixIndex = Random.Range(0, orderSuffixes.Count);
            randomSuffix = orderSuffixes[suffixIndex];
        }

        string dialogueText = currentRecipe.resultDishName + randomSuffix;
        StartCoroutine(TypeText(dialogueText));
    }

    public void OnClickGoToStore()
    {
        SceneManager.LoadScene("StoreScene");
    }

    IEnumerator TypeText(string textToType)
    {
        orderText.text = textToType;
        orderText.maxVisibleCharacters = 0;
        yield return null;

        int totalVisibleCharacters = orderText.textInfo.characterCount;
        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            orderText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void OnClickGoToKitchen()
    {
        counterPanel.SetActive(false);
        kitchenPanel.SetActive(true);
        recipeUIManager.DisplayRecipe(currentRecipe);
    }

    public void EndOrder(bool isSuccess)
    {
        StartCoroutine(EndOrderRoutine(isSuccess));
    }

    private IEnumerator EndOrderRoutine(bool isSuccess)
    {
        counterPanel.SetActive(true);
        kitchenPanel.SetActive(false);

        string resultText = "";

        if (isSuccess)
        {
            int index = Random.Range(0, successLines.Count);
            resultText = successLines[index];
        }
        else
        {
            int index = Random.Range(0, failureLines.Count);
            resultText = failureLines[index];
        }

        // 뽑아온 랜덤 대사를 타자기 효과로 재생!
        yield return StartCoroutine(TypeText(resultText));

        yield return new WaitForSeconds(1.0f);

        if (customerAnimator != null)
        {
            yield return StartCoroutine(customerAnimator.ExitAnimation());
        }

        yield return new WaitForSeconds(0.5f);
        NextOrder(); 
    }
}