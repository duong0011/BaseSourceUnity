using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CustomButton : Button
{
    [Header("Click Animation")]
    [SerializeField] private float clickScale = 0.9f;   // Scale nhỏ lại khi click
    [SerializeField] private float tweenDuration = 0.1f;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // 🔊 Phát âm thanh click
        AudioManager.Instance?.PlaySFXButtonClick();

        // ✨ Hiệu ứng scale bằng DOTween
        if (transform != null)
        {
            transform.DOScale(clickScale, tweenDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(1f, tweenDuration).SetEase(Ease.OutBack);
                });
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        // Có thể thêm hiệu ứng nhấn xuống nếu muốn
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        // Có thể thêm hiệu ứng nhả ra
    }
}
