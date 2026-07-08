using UnityEngine;
using System.Collections;

public class CustomerAnimator : MonoBehaviour
{
    public RectTransform customerRect;

    private Vector2 centerPos; // 내가 에디터에 배치해둔 원래 위치 (자동 저장)
    private Vector2 hiddenPos; // 원래 위치에서 밑으로 숨길 위치 (자동 계산)

    public float moveSpeed = 2f; 

    [Header("탄력 애니메이션 커브")]
    public AnimationCurve enterCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve exitCurve = AnimationCurve.Linear(0, 0, 1, 1);


    void Awake()
    {
        if (customerRect != null)
        {
            centerPos = customerRect.anchoredPosition;
            hiddenPos = centerPos + new Vector2(0, -1200); 
            customerRect.anchoredPosition = hiddenPos; // 시작할 때는 밑에 숨겨두기
        }
    }

    public IEnumerator EnterAnimation()
    {
        customerRect.anchoredPosition = hiddenPos;
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            float curveValue = enterCurve.Evaluate(time);
            customerRect.anchoredPosition = Vector2.LerpUnclamped(hiddenPos, centerPos, curveValue);
            yield return null;
        }
        customerRect.anchoredPosition = centerPos;
    }

    public IEnumerator ExitAnimation()
    {
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            float curveValue = exitCurve.Evaluate(time);
            customerRect.anchoredPosition = Vector2.LerpUnclamped(centerPos, hiddenPos, curveValue);
            yield return null;
        }
        customerRect.anchoredPosition = hiddenPos;
    }
}