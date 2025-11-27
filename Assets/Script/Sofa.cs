using UnityEngine;
using UnityEngine.AI;

public class Sofa : Interactable // Kế thừa luôn!
{
    private NavMeshObstacle obstacle;
    private bool isBlocked = false; // Trạng thái: Đã chặn cửa chưa?

    [Header("Vị trí")]
    [SerializeField] Vector3 blockPosition;   // Vị trí chặn cửa
    [SerializeField] Vector3 originalPosition; // Vị trí ban đầu

    protected override void Start()
    {
        base.Start(); // <-- THÊM DÒNG NÀY ĐỂ LẤY RENDERER TỪ CHA

        obstacle = GetComponent<NavMeshObstacle>();
        originalPosition = transform.position; // Lưu vị trí gốc

        // Mẹo: Tự tính vị trí chặn cửa (Dịch sang trái/phải 1 đoạn)
        // Hoặc bạn tự điền tay trong Inspector
        if (blockPosition == Vector3.zero)
            blockPosition = transform.position + new Vector3(2, 0, 0);
    }

    public override void OnInteract()
    {
        isBlocked = !isBlocked; // Đảo trạng thái

        if (isBlocked)
        {
            // Đẩy ra chặn cửa
            transform.position = blockPosition;
            obstacle.enabled = true; // Bật chặn AI
            Debug.Log("Đã Đẩy Sofa!");
        }
        else
        {
            // Kéo về chỗ cũ
            transform.position = originalPosition;
            obstacle.enabled = false; // Tắt chặn (hoặc giữ nguyên nếu muốn nó luôn là vật cản)
            Debug.Log("Đã Kéo Sofa Về!");
        }
    }

    public override void Break()
    {
        // Gọi hàm cha để đổi màu
        base.Break();

        Debug.Log("Sofa bị chém nát!");

        // Tắt chặn đường để Enemy đi xuyên qua xác cái ghế
        if (obstacle != null) obstacle.enabled = false;

        // Tắt Collider nếu có
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}