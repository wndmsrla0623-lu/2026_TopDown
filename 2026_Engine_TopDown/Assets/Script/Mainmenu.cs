using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("최고 기록 UI 팝업창 연결")]
    public GameObject recordPanel;         // 최고기록 Panel 팝업창
    public TextMeshProUGUI bestTimeText;   // 최고 시간 표시용 TextMeshPro 텍스트
    public TextMeshProUGUI bestPebbleText; // 최고 열매 표시용 TextMeshPro 텍스트

    // 시작 버튼을 누르면 호출될 함수
    public void StartGame()
    {
        // "Stage_1" 자리에 전환하고 싶은 실제 인게임 씬 이름을 적어줍니다.
        SceneManager.LoadScene("Stage_1");
    }

    // 최고 기록 보기 버튼을 누르면 호출될 함수
    public void OpenRecordPanel()
    {
        if (recordPanel != null)
        {
            recordPanel.SetActive(true);
        }

        if (GameDataManager.Instance != null)
        {
            var data = GameDataManager.Instance.currentSaveData;

            if (bestTimeText != null)
                bestTimeText.text = "최고 생존 시간 : " + data.bestTime + "초";

            if (bestPebbleText != null)
                bestPebbleText.text = "최고 획득 열매 : " + data.bestPebbles + "개";
        }
    }

    // ❌ 팝업창 닫기 버튼을 누르면 호출될 함수도 하나 만들어 둡니다!
    public void CloseRecordPanel()
    {
        if (recordPanel != null)
        {
            recordPanel.SetActive(false);
        }
    }
}
