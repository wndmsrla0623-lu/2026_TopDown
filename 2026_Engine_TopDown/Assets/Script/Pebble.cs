using UnityEngine;

public class Pebble : MonoBehaviour
{
    // 조약돌에 플레이어가 부딪혔을 때 발동
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 대상의 태그가 "Player" 라면?
        if (other.CompareTag("Player"))
        {
            // GameManager 대장에게 점수를 1 올리라고 명령함!
            GameManager.Instance.AddPebble();

            // 점수를 올렸으니 바닥에 있는 조약돌 오브젝트는 삭제함!
            Destroy(gameObject);
        }
    }
}