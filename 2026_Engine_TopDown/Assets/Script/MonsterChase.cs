using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public float speed = 1f;           // 슬라임 이동 속도
    private Transform playerTransform; // 플레이어 위치

    private SpriteRenderer spriteRenderer;

    [Header("방향별 스프라이트 배열 (플레이어와 동일 구조)")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("애니메이션 속도 설정")]
    public float frameTime = 0.15f;    // 프레임 전환 시간 (플레이어와 동일)

    private int currentFrame = 0;
    private float animationTimer = 0f;
    private Sprite[] currentAnimationArray; // 현재 재생해야 할 방향의 배열

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // 기본 상태는 아래를 바라보는 애니메이션으로 시작
        currentAnimationArray = spriteDown;
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 1. 플레이어 방향 벡터 계산 및 이동
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 2. 이동 방향을 분석하여 적절한 애니메이션 배열 선택
        SetAnimationDirection(direction);

        // 3. 선택된 방향의 스프라이트를 시간에 맞춰 번갈아 재생
        PlayAnimation();
    }

    // 몬스터가 움직이는 방향 벡터(x, y)를 분석해 상하좌우를 판단하는 함수
    void SetAnimationDirection(Vector3 dir)
    {
        Sprite[] previousArray = currentAnimationArray;

        // X축 이동량이 Y축 이동량보다 클 때 (좌우 이동 우세)
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0f)
                currentAnimationArray = spriteRight;
            else
                currentAnimationArray = spriteLeft;
        }
        // Y축 이동량이 X축 이동량보다 클 때 (상하 이동 우세)
        else
        {
            if (dir.y > 0f)
                currentAnimationArray = spriteUp;
            else
                currentAnimationArray = spriteDown;
        }

        // 만약 몬스터의 방향이 바뀌었다면 애니메이션 프레임을 리셋하여 어색함을 방지
        if (previousArray != currentAnimationArray)
        {
            currentFrame = 0;
            animationTimer = 0f;
        }
    }

    // 배열에 등록된 여러 장의 이미지를 순서대로 바꾸어주는 애니메이션 재생 함수
    void PlayAnimation()
    {
        if (currentAnimationArray == null || currentAnimationArray.Length == 0) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameTime)
        {
            animationTimer = 0f;
            // 다음 프레임으로 넘어가되, 배열의 끝에 도달하면 다시 0번으로 순환
            currentFrame = (currentFrame + 1) % currentAnimationArray.Length;
        }

        // 최종적으로 선택된 프레임의 이미지를 스프라이트 렌더러에 뿌려줌
        spriteRenderer.sprite = currentAnimationArray[currentFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("으악! 슬라임한테 부딪혔다!");
        }
    }
}
