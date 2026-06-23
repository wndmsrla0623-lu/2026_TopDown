using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string titleSceneName = "TitleScene";
    public string gameSceneName = "Stage_1";
    public GameObject gameOverPanel;


    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pebbleText;

    private float survivalTime = 0f;
    private int pebbleCount = 0;

    void Update()
    {
        // 1. 매 프레임마다 시간을 정상적으로 더해줍니다.
        survivalTime += Time.deltaTime;
        int seconds = (int)survivalTime;

        // 2. Start()에서 찾아온 timerText UI에 실시간으로 시간을 그려줍니다!
        if (timerText != null)
        {
            timerText.text = "시간: " + seconds + "초";
        }
    }

    private void Awake()
    {
        // 💡 씬이 바뀔 때 깨끗하게 초기화되도록 DontDestroyOnLoad를 완전히 제거한 깔끔한 싱글톤입니다.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        GameObject findText = GameObject.Find("TimeText");
        if (findText != null)
        {
            timerText = findText.GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public int GetSeconds()
    {
        return (int)survivalTime;
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        if (GameDataManager.Instance != null)
        {
            // 💡 세이브 데이터가 통째로 비어있다면 새로 만들어서 오류를 방지합니다.
            if (GameDataManager.Instance.currentSaveData == null)
            {
                GameDataManager.Instance.currentSaveData = new SaveData();
            }

            // 안전하게 얼음 해금을 true로 고정!
            GameDataManager.Instance.currentSaveData.isIceItemUnlocked = true;

            // 💾 하드디스크에 파일 쓰기 명령 실행
            GameDataManager.Instance.SaveGame((int)survivalTime, pebbleCount);
            Debug.Log("[체크] SaveGame 명령이 정상적으로 호출되었습니다.");
        }
        else
        {
            Debug.LogError("GameDataManager.Instance가 비어있어서 저장을 못 했습니다!");
        }
    }

    public void GoTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void AddPebble()
    {
        pebbleCount = pebbleCount + 1;
        pebbleText.text = "열매: " + pebbleCount + "개";
        Debug.Log("열매 획득! 현재 개수: " + pebbleCount);

        if (ItemSpawner.Instance != null)
        {
            ItemSpawner.Instance.SpawnDecision();
        }
        
    }

    public void OnPlayerDeath()
    {
        Debug.Log("플레이어 사망! 로그라이크 아이템 해금 조건을 충족했습니다.");

        if (GameDataManager.Instance != null && GameDataManager.Instance.currentSaveData != null)
        {
            // ⭐ [핵심 코드] 죽는 순간 세이브 데이터의 얼음 해금 여부를 true로 변경!
            GameDataManager.Instance.currentSaveData.isIceItemUnlocked = true;

            // 💾 변경된 데이터를 하드디스크(BestRecord.json)에 실제로 저장!
            GameDataManager.Instance.SaveGame((int)survivalTime, pebbleCount);
        }
    }

    // 메인 화면으로 돌아가는 버튼 연동 함수
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
