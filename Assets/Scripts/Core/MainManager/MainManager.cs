using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [Header("Prefabs / Scenes")]
    [SerializeField] private GameObject m_expGO;
    [SerializeField] private GameObject m_tutorialGO;
    [SerializeField] private GameObject m_menuGO;

    [Header("Transition Setting")]
    [SerializeField] private TransitionSettings m_transitionSettings;
    // Danh sách prefab cần preload
    private List<GameObject> preloadList = new List<GameObject>();
    private bool isFirstLoad = true;

    protected override void Awake()
    {
        base.Awake();
     
    }


    private IEnumerator PreloadPrefabs()
    {
        yield return new WaitForSeconds(0.5f); // Đợi 1 frame để đảm bảo mọi thứ đã sẵn sàng
        foreach (var prefab in preloadList)
        {
            if (prefab == null) continue;

            // Tạo instance ẩn để buộc Unity load tài nguyên
            prefab.SetActive(false);

            // Đợi 1 frame để Unity thực sự khởi tạo mesh/material
            yield return null;

            // Xóa object tạm nhưng asset vẫn cached
            //Destroy(prefab);
        }
      
        Debug.Log("[MainManager] Prefabs preloaded.");
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
        GameObject _tmp = null;
        if(isFirstLoad)
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
