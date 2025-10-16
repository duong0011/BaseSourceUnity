using UnityEngine;
using DG.Tweening;

public class JustRotate : MonoBehaviour
{
    [Header("Rotate Settings")]
    public float speed = 100f;     // tốc độ quay (độ / giây)
    public bool clockwise = true;  // hướng quay (true = thuận, false = ngược)

    private Tween rotateTween;

    private void OnEnable()
    {
        StartRotate();
    }

    private void OnDisable()
    {
        // Dừng tween khi object bị disable (tránh memory leak)
        rotateTween?.Kill();
    }

    private void StartRotate()
    {
        rotateTween?.Kill(); // Xóa tween cũ nếu có

        // Quay vô hạn
        float direction = clockwise ? -1f : 1f;
        float duration = 360f / Mathf.Max(speed, 0.1f); // thời gian để quay hết 1 vòng

        rotateTween = transform
            .DORotate(new Vector3(0, 0, 360f * direction), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1); // 🔁 lặp vô hạn
    }
}
