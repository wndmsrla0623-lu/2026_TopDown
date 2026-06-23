using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    public GameObject pebblePrefab;     // 일반 열매 프리팹
    public GameObject iceItemPrefab;    // 얼음 아이템 프리팹
    public int spawnCount = 20;         // 아이템 스폰 개수

    [Header("스폰할 사각형 영역 설정")]
    public float minX = -2.5f;
    public float maxX = 14f;
    public float minY = -6f;
    public float maxY = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // 💡 게임 시작 시 안전하게 데이터를 로드합니다.
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.LoadGame();
        }

        // 💡 무조건 원래 설정한 개수만큼 루프를 돌며 아이템을 생성합니다.
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnDecision();
        }
    }

    // ⭐ 열매를 만들지, 얼음을 만들지 결정하는 핵심 함수

    public void SpawnDecision()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.currentSaveData == null)
        {
            SpawnTargetPrefab(pebblePrefab);
            return;
        }

        // 💾 세이브 데이터에서 해금 여부를 제대로 가져옵니다.
        bool canSpawnIce = GameDataManager.Instance.currentSaveData.isIceItemUnlocked;

        // 🎰 치트키(true)를 풀고, '해금 완료' 상태에서 '30% 확률'일 때만 얼음이 섞여 나오도록 복구합니다!
        if (canSpawnIce && Random.Range(0f, 100f) <= 30f)
        {
            SpawnTargetPrefab(iceItemPrefab);
        }
        else
        {
            // 잠겨있거나 70% 확률에 걸리면 원래대로 기본 열매 스폰!
            SpawnTargetPrefab(pebblePrefab);
        }
    }

    // 💡 기존에 질문자님이 쓰시던 무작위 좌표 생성 및 오브젝트 생성 로직을 완벽하게 재현한 함수입니다.
    void SpawnTargetPrefab(GameObject targetPrefab)
    {
        if (targetPrefab == null) return; // 프리팹이 안 꽂혀있으면 에러 방지를 위해 탈출

        Vector3 randomPosition = Vector3.zero;
        bool isOverlap = true;
        int attempts = 0;

        // 겹치지 않는 빈자리를 찾을 때까지 위치를 무작위로 뽑습니다.
        while (isOverlap && attempts < 10)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            randomPosition = new Vector3(randomX, randomY, 0f);

            // 겹침 검사 (원래 쓰시던 규칙 그대로 유지)
            Collider2D hit = Physics2D.OverlapCircle(randomPosition, 0.5f);
            if (hit == null)
            {
                isOverlap = false;
            }
            attempts++;
        }

        // 최종 결정된 안전한 위치에 타겟 프리팹(열매 혹은 얼음)을 생성합니다!
        Instantiate(targetPrefab, randomPosition, Quaternion.identity);
    }
}