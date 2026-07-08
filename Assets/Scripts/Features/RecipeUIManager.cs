using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// ==========================================
// [여기가 2단계 코드입니다]
// 레시피 데이터 구조를 여기에 정의해 둡니다.
// ==========================================
[System.Serializable]
public class RecipeIngredientData
{
    public string ingredientName; // 재료 이름 (예: "고등어")
    public Sprite ingredientIcon; // 화면에 띄울 재료 이미지
}

[System.Serializable]
public class RecipeData
{
    public string resultDishName; // 요리 이름 (예: "고등어주먹밥")
    public Sprite resultIcon;     // 요리 완성 이미지
    public List<RecipeIngredientData> requiredIngredients; // 필요한 재료들
}


// ==========================================
// [여기가 3단계 코드입니다]
// ==========================================
public class RecipeUIManager : MonoBehaviour
{
    [Header("조립할 UI 부품(프리팹)들")]
    public GameObject ingredientSlotPrefab;
    public GameObject plusTextPrefab;
    public GameObject equalsTextPrefab;
    public GameObject resultSlotPrefab;

    [Header("현재 패널에 소환된 슬롯들을 관리할 리스트")]
    public List<GameObject> activeIngredientSlots = new List<GameObject>();

    /// <summary>
    /// 손님이 주문한 레시피 데이터를 받아서 UI를 쫙 그려주는 함수
    /// </summary>
    public void DisplayRecipe(RecipeData recipe)
    {
        // 1. 기존에 그려져 있던 레시피 UI 찌꺼기들을 전부 삭제하여 초기화
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        activeIngredientSlots.Clear();

        // 2. 레시피의 재료 개수만큼 반복하며 UI 생성
        for (int i = 0; i < recipe.requiredIngredients.Count; i++)
        {
            GameObject slotObj = Instantiate(ingredientSlotPrefab, transform);
            slotObj.GetComponent<Image>().sprite = recipe.requiredIngredients[i].ingredientIcon;
            slotObj.name = recipe.requiredIngredients[i].ingredientName;
            activeIngredientSlots.Add(slotObj);

            // 3. 마지막 재료가 아니라면 사이에 '+' 기호 생성
            if (i < recipe.requiredIngredients.Count - 1)
            {
                Instantiate(plusTextPrefab, transform);
            }
        }

        // 4. 재료가 다 나열되었으면 '=' 기호 생성
        Instantiate(equalsTextPrefab, transform);

        // 5. 결과물 슬롯 생성 및 이미지 적용
        GameObject resultObj = Instantiate(resultSlotPrefab, transform);
        resultObj.GetComponent<Image>().sprite = recipe.resultIcon;
    }

    public void ActivateCheck(string ingredientName)
    {
        foreach (GameObject slotObj in activeIngredientSlots)
        {
            // 이름이 일치하고, 아직 체크(Dim)가 안 켜진 슬롯을 찾음
            Transform dim = slotObj.transform.Find("Dim");
            Transform check = slotObj.transform.Find("Check");

            if (slotObj.name == ingredientName && dim != null && !dim.gameObject.activeSelf)
            {
                dim.gameObject.SetActive(true);
                check.gameObject.SetActive(true);
                break; // 하나 켰으면 종료 (동일한 재료가 2개 들어갈 수도 있으므로)
            }
        }
    }
}
