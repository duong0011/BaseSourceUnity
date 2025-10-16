using System;
using UnityEngine;
using DG.Tweening;

public static class UIPopupHelper
{
    /// <summary>
    /// Hiển thị object với hiệu ứng popup, bỏ qua nếu đã đang bật.
    /// </summary>
    public static void Popup(GameObject target, float duration = 0.3f, float startScale = 0.2f, Ease ease = Ease.OutBack, Action onComplete = null)
    {
        if (target == null) return;

        if (target.activeSelf && Vector3.Distance(target.transform.localScale, Vector3.one) < 0.05f)
            return;

        target.SetActive(true);
        var rect = target.transform as RectTransform;
        rect.DOKill();

        rect.localScale = Vector3.one * startScale;
        rect.DOScale(Vector3.one, duration)
            .SetEase(ease)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// Ẩn object với hiệu ứng thu nhỏ, bỏ qua nếu đã tắt.
    /// </summary>
    public static void Popdown(GameObject target, float duration = 0.2f, Ease ease = Ease.InBack, Action onComplete = null)
    {
        if (target == null || !target.activeSelf)
            return;

        var rect = target.transform as RectTransform;
        rect.DOKill();

        rect.DOScale(Vector3.zero, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                target.SetActive(false);
                onComplete?.Invoke();
            });
    }
}