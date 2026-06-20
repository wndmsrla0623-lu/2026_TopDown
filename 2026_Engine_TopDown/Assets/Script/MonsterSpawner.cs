using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject slimePrefab;     // 소환할 슬라임 프리팹
    public float spawnInterval = 3f;   // 몬스터가 소환되는 시간 간격 (3초에 한 마리씩)

    [Header("스폰할 사각형 영역 설정")]
    public float minX = -7.5f; // 우리가 맞춘 완벽한 잔디밭 사각형 좌표!
    public float maxX = 7.5f;
    public float minY = -5.2f;
    public float maxY = 3.2f;

    private float timer = 0f;

    void Update()
    {
        // 시간이 흐르다가 설정한 간격(3초)이 지나면 슬라임을 소환합니다.
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMonster();
            timer = 0f; // 타이머 리셋
        }
    }

    void SpawnMonster()
    {
        // 잔디밭 영역 안에서 무작위 좌표 생성
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        // 해당 위치에 슬라임 생성!
        if (slimePrefab != null)
        {
            Instantiate(slimePrefab, randomPosition, Quaternion.identity);
        }
    }
}