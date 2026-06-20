using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // 다른 곳에서 ItemSpawner.Instance 로 부를 수 있게 해주는 전역 창구
    public static ItemSpawner Instance;

    public GameObject pebblePrefab;  // 심을 조약돌 프리팹 (원본)
    public int spawnCount = 20;      // 처음에 스폰할 조약돌 총 개수

    [Header("스폰할 사각형 영역 설정")]
    public float minX = -2.5f;
    public float maxX = 14f;
    public float minY = -6f;
    public float maxY = 0f;

    // 유니티에서 게임이 시작될 때 가장 먼저 실행되는 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 창구에 나 자신을 등록
        }
    }

    void Start()
    {
        // 게임이 시작되면 지정된 개수만큼 조약돌을 생성합니다.
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnPebbleRandomly();
        }
    }

    void SpawnPebbleRandomly()
    {
        Vector3 randomPosition = Vector3.zero;
        bool isOverlap = true;
        int attempts = 0; // 과도한 루프 방지용 안전장치

        // 겹치지 않는 빈자리를 찾을 때까지 최대 10번 무작위 좌표를 다시 뽑습니다.
        while (isOverlap && attempts < 10)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            randomPosition = new Vector3(randomX, randomY, 0f);

            // 해당 위치 주변 0.8칸 안에 이미 다른 조약돌(Pebble)이 있는지 체크합니다.
            Collider2D hit = Physics2D.OverlapCircle(randomPosition, 0.8f);

            if (hit == null)
            {
                isOverlap = false; // 주변이 비어있다면 탈출!
            }
            attempts++;
        }

        if (pebblePrefab != null)
        {
            Instantiate(pebblePrefab, randomPosition, Quaternion.identity);
        }
    }

    // 외부(GameManager)에서 조약돌을 먹었을 때 호출할 함수
    public void SpawnOnePebble()
    {
        SpawnPebbleRandomly();
    }
}