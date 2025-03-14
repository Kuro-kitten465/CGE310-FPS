using UnityEngine;
using static UnityEngine.Debug;

namespace KuroNeko.DesignPatterns
{
    /// <summary>
    /// Base class for non-MonoBehaviour singletons.
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public abstract class LegacySingleton<T> where T : LegacySingleton<T>, new()
    {
        private static T _instance;
        private static readonly object _lock = new();
        private static bool _isInitialized;

        /// <summary>
        /// Gets the singleton instance, creating it if it doesn't exist.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance is not null) return _instance;

                    _instance = new T();
                    _instance.OnInitialize();
                    _isInitialized = true;
                    SingletonManager.Register<T>(_instance);

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Called when the singleton instance is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Destroys the singleton instance and performs cleanup.
        /// </summary>
        public static void Destroy()
        {
            lock (_lock)
            {
                if (_instance is null) return;

                // Call cleanup method
                _instance.OnDestroy();

                // Unregister from manager
                SingletonManager.Unregister<T>(UnregisterMode.DontDestroyInstance);

                // Release the instance
                _instance = null;
                _isInitialized = false;

                Log($"Singleton {typeof(T).Name} destroyed.");
            }
        }

        /// <summary>
        /// Called when the singleton is being destroyed.
        /// Override to perform custom cleanup.
        /// </summary>
        protected virtual void OnDestroy() { }

        /// <summary>
        /// Checks if the singleton instance has been initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;
    }
}