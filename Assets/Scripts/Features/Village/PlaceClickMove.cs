using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceClickMove : MonoBehaviour
{
    public string sceneName;

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name + " 클릭됨");

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("이동할 씬 이름이 비어 있습니다.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}