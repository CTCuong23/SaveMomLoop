using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    private NavMeshObstacle obstacle;
    private BoxCollider boxCollider;

    [Header("Trạng thái cửa (Tích vào là mở)")]
    public bool isOpen = false;

    void Start()
    {
        // Tự động tìm component trên cùng 1 object
        obstacle = GetComponent<NavMeshObstacle>();
        boxCollider = GetComponent<BoxCollider>();

        // Kiểm tra xem có đủ linh kiện chưa
        if (obstacle == null || boxCollider == null)
        {
            Debug.LogError("Thiếu component rồi! Gắn NavMeshObstacle và BoxCollider vào cửa đi!");
            return;
        }

        ApplyDoorState(); // Cập nhật trạng thái lúc bắt đầu
    }

    void Update()
    {
        // Bấm phím SPACE (Dấu cách) để test Đóng/Mở
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleDoor();
        }
    }

    // Hàm đổi trạng thái (Public để sau này gọi từ script khác cũng được)
    public void ToggleDoor()
    {
        isOpen = !isOpen; // Đảo ngược: Đang tắt thành bật, đang bật thành tắt
        ApplyDoorState();
    }

    // Hàm áp dụng logic
    void ApplyDoorState()
    {
        if (isOpen)
        {
            // --- CỬA MỞ ---
            // TẮT vật cản AI đi -> Để Enemy đi qua (Quan trọng nhất)
            obstacle.enabled = false;

            // BẬT Trigger -> Để đi xuyên qua vật lý (nếu muốn)
            boxCollider.isTrigger = true;

            Debug.Log("Cửa Đã MỞ (NavMeshObstacle: OFF)");
        }
        else
        {
            // --- CỬA ĐÓNG ---
            // BẬT vật cản AI lên
            obstacle.enabled = true;
            obstacle.carving = true; // Đục lỗ NavMesh

            // TẮT Trigger -> Để vật lý cứng lại
            boxCollider.isTrigger = false;

            Debug.Log("Cửa Đã ĐÓNG (NavMeshObstacle: ON)");
        }
    }
}