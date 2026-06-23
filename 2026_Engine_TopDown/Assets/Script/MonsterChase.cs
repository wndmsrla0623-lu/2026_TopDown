using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public float speed = 1f;           // 슬라임 이동 속도
    private Transform playerTransform; // 플레이어 위치

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;            // 물리를 제어할 리지드바디 변수 추가

    [Header("방향별 스프라이트 배열")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("애니메이션 속도 설정")]
    public float frameTime = 0.15f;

    private int currentFrame = 0;
    private float animationTimer = 0f;
    private Sprite[] currentAnimationArray;

    [Header("몬스터 공격 및 넉백 설정")]
    public int attackDamage = 10;
    public float knockbackForce = 3f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // 리지드바디 컴포넌트 가져오기

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        currentAnimationArray = spriteDown;
    }

    void FixedUpdate() // 물리를 사용하는 이동은 FixedUpdate에서 처리하는 것이 안전합니다.
    {
        if (playerTransform == null || rb == null) return;

        // 1. 플레이어 방향 벡터 계산 후 리지드바디의 속도(Velocity)로 이동 처리!
        // 이 방식으로 이동해야 슬라임끼리 부딪혔을 때 물리 엔진이 서로를 밀어낼 수 있습니다.
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // 2. 이동 방향 분석하여 애니메이션 선택
        SetAnimationDirection(direction);
    }

    void Update()
    {
        // 3. 선택된 방향의 스프라이트를 시간에 맞춰 재생
        PlayAnimation();
    }

    void SetAnimationDirection(Vector2 dir)
    {
        Sprite[] previousArray = currentAnimationArray;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0f) currentAnimationArray = spriteRight;
            else currentAnimationArray = spriteLeft;
        }
        else
        {
            if (dir.y > 0f) currentAnimationArray = spriteUp;
            else currentAnimationArray = spriteDown;
        }

        if (previousArray != currentAnimationArray)
        {
            currentFrame = 0;
            animationTimer = 0f;
        }
    }

    void PlayAnimation()
    {
        if (currentAnimationArray == null || currentAnimationArray.Length == 0) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameTime)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % currentAnimationArray.Length;
        }

        spriteRenderer.sprite = currentAnimationArray[currentFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);

                // 플레이어와 부딪혔을 때 넉백 처리
                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                transform.position += (Vector3)knockbackDirection * knockbackForce;
            }
        }
    }

    // MonsterChase.cs 내부에 추가할 멈춤 기능 예시
    public System.Collections.IEnumerator FreezeMonsterRoutine()
    {
        // ⭕ 이 스크립트(MonsterChase)의 업데이트 추적 기능을 고장 내서 멈추게 만듭니다!
        this.enabled = false;

        // 만약 리지드바디로 움직인다면 물리 속도도 강제로 0으로 잡아줍니다.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(3f); // 3초 정지

        // ⭕ 3초 뒤에 다시 스크립트를 켜서 추적을 시작하게 만듭니다.
        this.enabled = true;
    }
}
