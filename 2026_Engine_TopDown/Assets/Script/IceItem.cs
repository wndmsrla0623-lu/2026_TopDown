using UnityEngine;

public class IceItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("❄️ 얼음 아이템 획득! 몬스터 정지 기능을 시작합니다.");

            // 1. 점수 올리기
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPebble();
            }

            // 2. 🎮 [핵심] 맵에 존재하는 모든 몬스터들을 찾아서 멈추기
            // 현재 프로젝트에 있는 몬스터 추적 스크립트 이름(MonsterChase)을 가진 오브젝트들을 다 끌고 옵니다.
            MonsterChase[] monsters = FindObjectsByType<MonsterChase>(FindObjectsSortMode.None);

            foreach (MonsterChase monster in monsters)
            {
                if (monster != null)
                {
                    // 몬스터 스크립트 내부에 'Freeze()' 같은 멈춤 함수를 호출하거나, 
                    // 속도를 임시로 0으로 만드는 처리를 해줍니다.
                    monster.StartCoroutine(monster.FreezeMonsterRoutine());
                }
            }

            // 3. 아이템 삭제
            Destroy(gameObject);
        }
    }
}