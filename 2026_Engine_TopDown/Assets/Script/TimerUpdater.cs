using UnityEngine;
using TMPro; // TextMeshPro를 쓰기 위해 필수!

public class TimerUpdater : MonoBehaviour
{
    private TextMeshProUGUI myText;

    void Start()
    {
        // 이 스크립트가 붙은 오브젝트의 TextMeshPro 컴포넌트를 스스로 가져옵니다.
        myText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // 💡 GameManager가 씬에 살아있다면, 실시간으로 생존 시간을 받아와 내 글자를 바꿉니다!
        if (GameManager.Instance != null && myText != null)
        {
            myText.text = "시간: " + GameManager.Instance.GetSeconds() + "초";
        }
    }
}