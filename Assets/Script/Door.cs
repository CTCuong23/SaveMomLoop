using UnityEngine;
using System.Collections; // Nhớ thêm dòng này để dùng IEnumerator
using UnityEngine.AI;

// KẾ THỪA TỪ INTERACTABLE (Thay vì MonoBehaviour)
public class Door : Interactable
{
    private NavMeshObstacle obstacle;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private bool isOpen = false;

    protected override void Start()
    {
        base.Start(); // <--- Dòng này giúp fix lỗi không đổi màu
        obstacle = GetComponent<NavMeshObstacle>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>(); // <--- Lấy component Rigidbody
        ApplyDoorState();
    }

    // GHI ĐÈ HÀM CỦA CHA
    public override void OnInteract()
    {
        if (isBroken) return; // Nếu cửa đã bị phá thì không cho đóng mở nữa
        // Logic riêng của cái cửa: Đảo ngược trạng thái
        isOpen = !isOpen;
        ApplyDoorState();
    }

    void ApplyDoorState()
    {
        // (Giữ nguyên logic đóng mở hôm qua)
        if (isOpen)
        {
            obstacle.enabled = false;
            boxCollider.isTrigger = true;
            Debug.Log("Cửa MỞ");
        }
        else
        {
            obstacle.enabled = true;
            obstacle.carving = true;
            boxCollider.isTrigger = false;
            Debug.Log("Cửa ĐÓNG");
        }
    }

    public override void Break()
    {
        if (isBroken) return;

        // 1. Gọi cha để đổi màu xám (Fix lỗi cũ)
        base.Break();
        Debug.Log("Cửa đã bị phá hủy!");

        // 1. Tắt NavMeshObstacle (Để Enemy không bị chặn)
        if (obstacle) obstacle.enabled = false;

        // 2. Xử lý va chạm:
        // Ta KHÔNG tắt BoxCollider nữa, vì nếu tắt thì cửa sẽ lọt thỏm xuống lòng đất!
        // Thay vào đó, ta biến nó thành vật thể vật lý thường (không phải Trigger)
        if (boxCollider)
        {
            boxCollider.isTrigger = false;
            boxCollider.enabled = true;
        }

        // 3. KÍCH HOẠT VẬT LÝ & TẠO LỰC ĐẨY
        if (rb != null)
        {
            rb.isKinematic = false; // Bắt đầu chịu tác động vật lý
            rb.useGravity = true;   // Bật trọng lực

            // --- QUAN TRỌNG NHẤT: TẠO LỰC ĐỂ LÀM MẤT THĂNG BẰNG ---

            // Cách 1: Thêm lực xoắn (Torque) để bắt nó quay trục X (Ngã sấp hoặc ngửa)
            // Số 10f là lực xoắn, bạn có thể tăng lên 50f nếu cửa quá nặng
            rb.AddTorque(transform.right * 10f, ForceMode.Impulse);

            // Cách 2: Đạp thêm một cái vào bụng cửa cho nó bay ra xa một chút
            // (Kết hợp cả lực đẩy tới và đẩy lên trời một chút cho nó nảy lên)
            Vector3 pushDirection = transform.forward + Vector3.up;
            rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }

        // 3. QUAN TRỌNG: Hẹn giờ để tắt va chạm sau khi ngã xong
        StartCoroutine(DisableCollisionAfterDelay(0.5f));

        // Hàm hẹn giờ: Chờ cửa nằm im rồi biến nó thành "hồn ma"
        IEnumerator DisableCollisionAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay); // Chờ 2 giây

            // Tắt va chạm hoàn toàn -> Enemy đi xuyên qua luôn
            if (boxCollider) boxCollider.enabled = false;

            // Tắt tính toán vật lý cho đỡ nặng máy
            if (rb) rb.isKinematic = true;

            Debug.Log("Cửa đã nằm im, Enemy có thể đi qua!");
        }
    }
}