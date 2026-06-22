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

        if (currentHealth <= 0)
        {
            Die();
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
