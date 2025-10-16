using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    [Header("Singleton Settings")]
    [Tooltip("Nếu true, singleton sẽ bị destroy khi reload scene.")]
    [SerializeField] private bool isDestroyOnLoad = true;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var singletonObj = new GameObject($"[Singleton] {typeof(T).Name}");
                        _instance = singletonObj.AddComponent<T>();
                        DontDestroyOnLoad(singletonObj);

                        Debug.Log($"[Singleton] Created new instance of {typeof(T)}.");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;

        if (!isDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (this == _instance)
        {
            _applicationIsQuitting = true;
            _instance = null;
        }
    }

    protected virtual void OnEnable()
    {
        _applicationIsQuitting = false;
    }
}
