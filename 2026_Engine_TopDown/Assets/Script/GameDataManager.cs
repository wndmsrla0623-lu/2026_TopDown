using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public GameSettingData gameSettingData;

    public SaveData saveData;

    public int isTutorialFinished;


    private string savePath;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/saveData.json";

            LoadJsonData();
            LoadPlayerPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetPlayerHP()
    {
        int baseHP = gameSettingData.startHp;
        int bonusHp = gameSettingData.hpBonusPerDeath;

        return baseHP + bonusHp * saveData.deathCount;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = gameSettingData.atkBonusPerDeath;
        return baseAttack + bonusAttack * saveData.deathCount;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    public void SaveGameResult()
    {
        saveData.deathCount++;

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
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("TUTORIAL");
        LoadPlayerPrefs();

        Debug.Log("PlayerPrefs 삭제 완료");
    }

}



