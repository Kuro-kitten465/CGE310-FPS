using UnityEngine;
using static UnityEngine.Debug;

namespace KuroNeko.DesignPatterns
{
    /// <summary>
    /// Base class for MonoBehaviour-based singletons.
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        [SerializeField] private bool _dontDestroyOnLoad = true;

        private static T _instance;
        private static readonly object _lock = new();
        private static bool _isInitialized;

        /// <summary>
        /// Checks if the singleton instance has been initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Gets the singleton instance, finding or creating it if it doesn't exist.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance is not null) return _instance;

                    _instance = FindObjectOfType<T>();

                    if (_instance is not null) return _instance;

                    GameObject singletonObject = new(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();

                    if (_isInitialized) return _instance;

                    _instance.OnInitialize();
                    _isInitialized = true;
                    SingletonManager.Register<T>(_instance);

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == this)
            {
                LogWarning($"Duplicate singleton instance of type {typeof(T).Name} detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            if (_dontDestroyOnLoad)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            OnInitialize();
            _isInitialized = true;
            SingletonManager.Register<T>(_instance);
        }

        /// <summary>
        /// Called when the singleton instance is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                // Call cleanup logic before unregistering
                OnCleanup();

                // Unregister from manager
                SingletonManager.Unregister<T>(UnregisterMode.DontDestroyInstance);

                // Reset static references
                _instance = null;
                _isInitialized = false;

                Log($"MonoSingleton {typeof(T).Name} destroyed.");
            }
        }

        /// <summary>
        /// Called when the singleton is being destroyed.
        /// Override to perform custom cleanup.
        /// </summary>
        protected virtual void OnCleanup() { }

        /// <summary>
        /// Manually destroys this singleton instance.
        /// Use when you need to destroy the singleton before scene changes.
        /// </summary>
        public static void DestroyInstance()
        {
            lock (_lock)
            {
                if (_instance is null) return;

                Destroy(_instance.gameObject); // OnDestroy will handle the rest
            }
        }
    }
}
