using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // <-- THÊM DÒNG NÀY để dùng chức năng Reset màn chơi

public class EnemyAI : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform target; // Kéo Mẹ vào đây

    [Header("Cài đặt Sát Nhân")]
    public float killRange = 1.0f; // Khoảng cách để ra tay (1 mét)

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            // Tự tìm object tên "Mom" nếu lỡ quên kéo vào
            GameObject momObj = GameObject.Find("Mom");
            if (momObj != null) target = momObj.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1. Đuổi theo Mẹ
        agent.SetDestination(target.position);

        // 2. Kiểm tra khoảng cách: Nếu đã đến đủ gần
        float distanceToMom = Vector3.Distance(transform.position, target.position);

        if (distanceToMom <= killRange)
        {
            AttackMom();
        }
    }

    void AttackMom()
    {
        // Tạm thời chỉ in Log và Reset game
        Debug.Log("SÁT NHÂN ĐÃ BẮT ĐƯỢC MẸ! ---> BAD ENDING");

        // Gọi hàm Reset vòng lặp
        ResetLoop();
    }

    void ResetLoop()
    {
        // Lấy tên màn chơi hiện tại và load lại chính nó
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}