using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class IngredientMenuData
{
    public string name;
    public Sprite sprite;
    public int count = 999;
    public TextMeshProUGUI countText;
}

public class KitchenManager : MonoBehaviour
{
    [Header("프리팹 및 연결")]
    public GameObject ingredientPrefab;
    public OrderManager orderManager; // 카운터로 돌아가기 위해 연결

    [Header("재료 데이터 목록")]
    public List<IngredientMenuData> ingredientMenu = new List<IngredientMenuData>();

    // 화면에 소환된 실제 재료 오브젝트(나중에 삭제하기 위함)
    private List<GameObject> spawnedObjects = new List<GameObject>();
    // 소환된 재료들의 이름 (채점용)
    private List<string> spawnedIngredientNames = new List<string>();

    void Start()
    {
        UpdateMenuUI();
    }

    /// <summary>
    /// 우측 버튼을 눌러 재료를 스폰하는 함수
    /// </summary>
    public void OnClickSpawnIngredient(string ingredientName)
    {
        IngredientMenuData data = ingredientMenu.Find(x => x.name == ingredientName);
        if (data != null && data.count > 0)
        {
            data.count--;
            UpdateMenuUI();

            Vector2 randomOffset = Random.insideUnitCircle * 0.2f;
            Vector3 spawnPosition = new Vector3(randomOffset.x, randomOffset.y, 0);

            GameObject newObj = Instantiate(ingredientPrefab, spawnPosition, Quaternion.identity);
            
            SpriteRenderer sr = newObj.GetComponent<SpriteRenderer>();
            if (sr != null && data.sprite != null) sr.sprite = data.sprite;

            // 관리 리스트에 추가
            spawnedObjects.Add(newObj);
            spawnedIngredientNames.Add(ingredientName);

            // 상단 UI 체크표시 켜기 (RecipeUIManager의 함수 호출)
            orderManager.recipeUIManager.ActivateCheck(ingredientName);
        }
    }

    /// <summary>
    /// '요리 완성' 버튼을 눌렀을 때 호출되는 함수
    /// </summary>
    public void OnClickCookComplete()
    {
        bool isSuccess = CheckRecipeSuccess();


        // 도마 위 청소하기
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
        spawnedIngredientNames.Clear();

        // [변경] 바로 다음 손님을 받지 않고, 카운터 결과 화면 연출을 거치도록 합니다!
        orderManager.EndOrder(isSuccess);
    }

    // 채점 로직 (들어간 재료 리스트와 요구 재료 리스트 비교)
    private bool CheckRecipeSuccess()
    {
        RecipeData target = OrderManager.currentRecipe;

        // 개수가 다르면 무조건 실패
        if (spawnedIngredientNames.Count != target.requiredIngredients.Count) return false;

        // 임시 리스트를 만들어 하나씩 매칭하며 지워나감
        List<string> targetNames = new List<string>();
        foreach (var req in target.requiredIngredients) targetNames.Add(req.ingredientName);

        foreach (string spawnedName in spawnedIngredientNames)
        {
            if (targetNames.Contains(spawnedName)) targetNames.Remove(spawnedName);
            else return false; // 필요 없는 재료가 들어감
        }

        return targetNames.Count == 0; // 남은 목표 재료가 없으면 완벽 성공
    }

    private void UpdateMenuUI()
    {
        foreach (var data in ingredientMenu)
        {
            if (data.countText != null) data.countText.text = data.count.ToString();
        }
    }
}