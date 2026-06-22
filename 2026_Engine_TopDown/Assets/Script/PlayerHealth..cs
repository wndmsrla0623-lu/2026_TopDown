using UnityEngine;
using TMPro; // 텍스트를 제어하기 위해 반드시 필요합니다!

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;       // 최대 체력
    public int currentHealth;         // 현재 체력

    [Header("UI 설정")]
    public TextMeshProUGUI hpText;    // 화면에 HP를 표시할 텍스트 컴포넌트

    void Start()
    {
        // 게임이 시작되면 현재 체력을 가득 채우고 UI를 업데이트합니다.
        currentHealth = maxHealth;
        UpdateHPUI();
    }

    // 외부(몬스터)에서 플레이어에게 데미지를 줄 때 호출할 함수
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // 체력이 깎였으므로 화면의 텍스트도 실시간으로 갱신합니다.
        UpdateHPUI();

        Debug.Log($"아야! 플레이어가 {damageAmount}의 데미지를 받았습니다. 남은 체력: {currentHealth}/{maxHealth}");

        // HP가 0이 되었을 때 처리하는 조건문 내부
        if (currentHealth <= 0)
        {
            Debug.Log("플레이어 사망!");

            // 💡여기에 GameManager를 찾아서 GameOver() 함수를 실행하라는 명령을 내립니다!
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver(); // GameManager에 있는 게임오버 로직 가동!
            }
            else
            {
                Debug.LogError("씬에 GameManager 오브젝트를 찾을 수 없습니다!");
            }

            // ⚠️ 중요: Destroy(gameObject); 코드가 카메라를 물고 있다면 
            // 카메라 에러가 나므로, 안전하게 플레이어의 이미지와 충돌체만 꺼줍니다.
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // 화면의 텍스트를 "HP: 100 / 100" 형태로 예쁘게 바꿔주는 함수
    void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("플레이어가 사망했습니다... 게임 오버!");

        // 사망 시 UI 텍스트를 "Game Over" 등으로 변경해 줄 수도 있습니다.
        if (hpText != null)
        {
            hpText.text = "HP: 0 / " + maxHealth + " (사망)";
        }

        gameObject.SetActive(false);
    }
}
