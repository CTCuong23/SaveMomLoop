using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float killRange = 1.0f;
    [SerializeField] float checkRadius = 0.8f;

    [Header("Cài đặt Phá Hoại")]
    [SerializeField] float breakingTime = 3.0f; // Mất 3 giây để phá xong cái cửa
    [SerializeField] float attackRate = 1.0f;   // Phá xong nghỉ 1 giây mới đi tiếp
    private float nextAttackTime = 0f;
    private bool isBusyBreaking = false; // Biến kiểm tra xem có đang bận đục cửa không

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            GameObject mom = GameObject.Find("Mom");
            if (mom) target = mom.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1. Nếu đang bận phá cửa thì ĐỪNG làm gì cả (Đứng im)
        if (isBusyBreaking) return;

        // 2. Đi tìm Mẹ
        agent.SetDestination(target.position);

        // 3. Kiểm tra khoảng cách giết người
        if (Vector3.Distance(transform.position, target.position) <= killRange)
        {
            AttackMom();
        }

        // 4. Quét vật cản
        CheckObstacle();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + transform.forward * 1.0f + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(attackCenter, checkRadius);
    }

    void CheckObstacle()
    {
        // Nếu chưa hồi chiêu xong thì không kiểm tra
        if (Time.time < nextAttackTime) return;

        Vector3 attackCenter = transform.position + transform.forward * 1.0f + Vector3.up * 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, checkRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

            Interactable obstacle = hit.GetComponent<Interactable>();

            // Tìm thấy vật cản và nó đang hoạt động
            if (obstacle != null && obstacle.gameObject.activeSelf)
            {
                // BẮT ĐẦU QUY TRÌNH PHÁ CỬA (Coroutine)
                StartCoroutine(BreakObstacleRoutine(obstacle));

                // Thoát vòng lặp ngay để chỉ phá 1 cái một lúc
                return;
            }
        }
    }

    // --- COROUTINE: QUY TRÌNH PHÁ CỬA ---
    IEnumerator BreakObstacleRoutine(Interactable obstacle)
    {
        // 1. Đánh dấu là đang bận
        isBusyBreaking = true;

        // 2. Dừng di chuyển ngay lập tức
        agent.isStopped = true;
        agent.velocity = Vector3.zero; // Phanh gấp

        Debug.Log("Phát hiện " + obstacle.name + ". Đang phá cửa... (Chờ " + breakingTime + "s)");

        // 3. Chờ đúng thời gian breakingTime (Ví dụ 3 giây)
        // (Sau này bạn chèn Animation vung rìu và âm thanh 'Cộc cộc' ở đây)
        yield return new WaitForSeconds(breakingTime);

        // 4. Kiểm tra lại xem vật đó còn đó không (lỡ ai phá trước rồi)
        if (obstacle != null)
        {
            obstacle.Break(); // BÙM! Cửa vỡ
        }

        // 5. Nghỉ mệt một chút (Cooldown)
        nextAttackTime = Time.time + attackRate;

        // 6. Cho phép đi tiếp
        agent.isStopped = false;
        isBusyBreaking = false;
    }

    void AttackMom()
    {
        Debug.Log("BAD ENDING!");
        ResetLoop();
    }

    void ResetLoop()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}