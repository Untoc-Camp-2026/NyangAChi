using UnityEngine;
using UnityEngine.EventSystems; // 유니티의 클릭/드래그 시스템

public class DraggableIngredient : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Rigidbody2D rb;
    private Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 마우스를 클릭했을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        // [수정] Input.mousePosition 대신 이벤트가 보내준 eventData.position을 사용합니다.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - mousePos;
    }

    // 마우스로 드래그하는 중일 때 (매 프레임 실행)
    public void OnDrag(PointerEventData eventData)
    {
        // [수정] 여기도 eventData.position으로 변경하여 신형 인풋 시스템 에러를 완벽히 차단합니다.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 newPosition = mousePos + offset;
        newPosition.z = 0;

        transform.position = newPosition;
    }

    // 마우스를 놓았을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
    }
}