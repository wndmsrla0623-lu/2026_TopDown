using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string titleSceneName = "TitleScene";
    public string gameSceneName = "GameScene";


    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pebbleText;

    private float survivalTime = 0f;
    private int pebbleCount = 0;

    void Update()
    {
        survivalTime = survivalTime + Time.deltaTime;

        int seconds = (int)survivalTime;

        timerText.text = "시간: " + seconds + "초";
    }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GameOver()
    {
        GameDataManager.Instance.SaveGameResult();
        GoTitle();
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
}
