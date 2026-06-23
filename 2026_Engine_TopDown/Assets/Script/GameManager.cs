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

            GameDataManager.Instance.SaveGame((int)survivalTime, pebbleCount);
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
        ItemSpawner.Instance.SpawnOnePebble();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScene");
    }
}
