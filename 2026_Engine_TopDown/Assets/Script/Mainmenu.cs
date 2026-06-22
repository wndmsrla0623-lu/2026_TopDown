using UnityEngine;
using UnityEngine.SceneManagement; // 💡 씬 관리를 위해 반드시 추가해야 합니다!

public class MainMenu : MonoBehaviour
{
    // 시작 버튼을 누르면 호출될 함수
    public void StartGame()
    {
        // "Stage_1" 자리에 전환하고 싶은 실제 인게임 씬 이름을 적어줍니다.
        SceneManager.LoadScene("Stage_1");
    }
}
