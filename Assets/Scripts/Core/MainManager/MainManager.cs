using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartMode
{
    Menu,
    ExpProcess,
    Tutorial
}

public class MainManager : Singleton<MainManager>
{
    [Header("Prefabs / Scenes")]
    [SerializeField] private GameObject m_expGO;
    [SerializeField] private GameObject m_tutorialGO;
    [SerializeField] private GameObject m_menuGO;

    [Header("Transition Setting")]
    [SerializeField] private TransitionSettings m_transitionSettings;

    [Header("Start Mode")]
    [SerializeField] private StartMode m_startMode = StartMode.Menu;

    // Danh sách prefab cần preload
    private List<GameObject> preloadList = new List<GameObject>();
    private bool isFirstLoad = true;

    protected override void Awake()
    {
        base.Awake();
    }

    private IEnumerator PreloadPrefabs()
    {
        //yield return new WaitForSeconds(0.1f); // Đợi 1 chút để đảm bảo hệ thống sẵn sàng

        foreach (var prefab in preloadList)
        {
            if (prefab == null) continue;

            // Tạo instance tạm để Unity load tài nguyên

            prefab.SetActive(false);

            // Đợi 1 frame để Unity thực sự khởi tạo mesh/material
            yield return null;

            
        }

        Debug.Log("[MainManager] Prefabs preloaded.");

        // Sau khi preload xong, nếu không phải chế độ Menu thì load prefab tương ứng luôn
        if (m_startMode == StartMode.ExpProcess)
        {
            LoadNewScene(m_expGO);
        }
        else if (m_startMode == StartMode.Tutorial)
        {
            LoadNewScene(m_tutorialGO);
        }
    }

    private void Start()
    {
        // Gom tất cả prefab cần preload
        preloadList.Add(m_expGO);
        preloadList.Add(m_tutorialGO);

        // Bắt đầu preload
        StartCoroutine(PreloadPrefabs());
    }

    private void LoadNewScene(GameObject prefab)
    {
        AudioManager.Instance.StopAll();
        if (TransitionManager.Instance().IsRunningTransition) return;
        GameObject _tmp = null;
        if (isFirstLoad)
        {
            _tmp = m_menuGO;
            isFirstLoad = false;
        }

        TransitionManager.Instance().Transition(prefab, transform, m_transitionSettings, 0.2f, 0.5f, _tmp);
    }

    public void LoadExp() => LoadNewScene(m_expGO);
    public void LoadTutorial() => LoadNewScene(m_tutorialGO);
    public void LoadMenu() => LoadNewScene(m_menuGO);

    public void Exit()
    {
        Debug.Log("[MainManager] Exit game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
