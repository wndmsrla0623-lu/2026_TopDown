using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set;}

    public float bestSurvivalTime;

    public GameSettingData gameSettingData;

    public SaveData saveData;

    public int isTutorialFinished;

    public SaveData currentSaveData = new SaveData();

    private string savePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // 💡 씬이 바뀔 때마다 자동으로 실행될 함수를 유니티 시스템에 등록합니다!
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
                return; // ⚠️ 중복 매니저 파괴 시 아래 로직 실행 방지
            }

           savePath = Path.Combine(Application.persistentDataPath, "BestRecord.json");

           LoadGame();
        }

    // 💡 싱글톤 오브젝트가 파괴될 때는 메모리 누수 방지를 위해 이벤트를 해제해 줍니다.
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 💡 핵심: 다시 시작해서 새로운 씬이 켜질 때마다 매번 실행되는 함수입니다!
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       Debug.Log("새로운 씬 로드됨: " + scene.name);
    }

    public int GetPlayerHP()
    {
        int baseHP = gameSettingData.startHp;
        int bonusHp = gameSettingData.hpBonusPerDeath;

        return baseHP + bonusHp * saveData.bestTime;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = gameSettingData.atkBonusPerDeath;
        return baseAttack + bonusAttack * saveData.bestTime;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    public void SaveGameResult(float finalTime, int pebblesObtained)
    {
        saveData.bestTime++;
        saveData.bestPebbles += pebblesObtained;

        if (finalTime > bestSurvivalTime)
        {
            bestSurvivalTime = finalTime;
            SavePlayerPrefs();
            Debug.Log(" 최고 기록 경신! " + bestSurvivalTime + "초");
        }

        SaveJsonData();
    }


    public void SaveJsonData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("JSON 저장 완료: " + savePath);
    }

    public void LoadJsonData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            SaveJsonData();
        }
    }

    public void SaveGame(int scoreTime, int scorePebbles)
    {
        if (scoreTime > currentSaveData.bestTime) currentSaveData.bestTime = scoreTime;
        if (scorePebbles > currentSaveData.bestPebbles) currentSaveData.bestPebbles = scorePebbles;

        string json = JsonUtility.ToJson(currentSaveData, true);

        File.WriteAllText(savePath, json);
        Debug.Log("JSON 최고 기록 저장 완료!:" + json);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            currentSaveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("JSON 데이터 로드 성공!");
        }
    }

    public void DeleteJsonData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        saveData = new SaveData();
        SaveJsonData();

        Debug.Log("Json 데이터 삭제 완료");
    }

    public void LoadPlayerPrefs()
    {
        isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0);

        // [추가] 저장되어 있던 최고 생존 시간을 로드 (없으면 0초)
        bestSurvivalTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0f);
        Debug.Log("PlayerPrefs 로드 완료!");
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);

        // [추가] 최고 생존 시간 저장
        PlayerPrefs.SetFloat("BestSurvivalTime", bestSurvivalTime);
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs 저장 완료!");
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("TUTORIAL");
        PlayerPrefs.DeleteKey("BestSurvivalTime"); // [추가] 최고기록 삭제
        bestSurvivalTime = 0f;
        LoadPlayerPrefs();
    }

}



