using UnityEngine;

public class Phone : Interactable
{
    [Header("Phone Settings")]
    [SerializeField] float timeReduction = 15f; // Gọi xong bố về sớm 15 giây
    [SerializeField] bool hasCalled = false;    // Chỉ được gọi 1 lần mỗi màn

    public override void OnInteract()
    {
        if (hasCalled)
        {
            Debug.Log("Điện thoại đã gọi rồi!");
            return;
        }

        // Logic gọi điện
        hasCalled = true;
        Debug.Log("Alo Bố à! Về cứu Mẹ đi!!");

        // 1. Giảm thời gian chờ trong GameManager
        GameManager.Instance.ReduceTime(timeReduction);

        // 2. Hiệu ứng hình ảnh (VD: Đổi màu điện thoại để biết đã dùng)
        if (objRenderer != null)
        {
            objRenderer.material.color = Color.red; 
        }
    }
}