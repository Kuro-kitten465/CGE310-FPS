using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

namespace KuroNeko.DesignPatterns
{
    /// <summary>
    /// Use to controls behavior of Unregister function
    /// </summary>
    public enum UnregisterMode
    {
        DestroyInstance, DontDestroyInstance
    }

    /// <summary>
    /// Manages all singletons in the system, providing tracking and control functionality.
    /// </summary>
    public static class SingletonManager
    {
        private static readonly Dictionary<Type, object> _singletons = new();

        /// <summary>
        /// Register a singleton instance with the manager.
        /// </summary>
        /// <param name="type">Type of the singleton</param>
        /// <param name="instance">Instance of the singleton</param>
        public static void Register<T>(object instance)
        {
            if (_singletons.ContainsKey(typeof(T)))
            {
                LogWarning($"Singleton of type {typeof(T).Name} already registered.");
                return;
            }

            _singletons.Add(typeof(T), instance);
            Log($"Singleton registered: {typeof(T).Name}");
        }

        /// <summary>
        /// Unregister a singleton instance from the manager.
        /// </summary>
        /// <param name="type">Type of the singleton to unregister</param>
        /// <param name="mode">
        /// Clear instance after when unregister or not.
        /// They will clear instance by default.
        ///  </param>
        public static void Unregister<T>(UnregisterMode mode = UnregisterMode.DestroyInstance)
        {
            if (!_singletons.ContainsKey(typeof(T))) return;

            switch (mode)
            {
                case UnregisterMode.DestroyInstance:
                    DestroySingleton<T>();
                    break;
                case UnregisterMode.DontDestroyInstance:
                    _singletons.Remove(typeof(T));
                    Log($"Singleton unregistered: {typeof(T).Name}");
                    break;
            }
        }

        /// <summary>
        /// Get all registered singleton instances.
        /// </summary>
        /// <returns>Array of all singleton instances</returns>
        public static object[] GetAllSingletons()
        {
            object[] instances = new object[_singletons.Count];
            _singletons.Values.CopyTo(instances, 0);
            return instances;
        }

        /// <summary>
        /// Get a singleton instance by its type.
        /// </summary>
        /// <typeparam name="T">Type of singleton to retrieve</typeparam>
        /// <returns>The singleton instance or null if not found</returns>
        public static T GetSingleton<T>() where T : class
        {
            if (_singletons.TryGetValue(typeof(T), out object instance))
                return instance as T;

            return null;
        }

        /// <summary>
        /// Check if a singleton of the specified type is registered.
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <returns>True if registered, false otherwise</returns>
        public static bool HasSingleton<T>() where T : class => _singletons.ContainsKey(typeof(T));

        /// <summary>
        /// Destroys a specific singleton by type, calling its destroy/cleanup methods if possible.
        /// </summary>
        /// <typeparam name="T">Type of singleton to destroy</typeparam>
        /// <returns>True if successfully destroyed, false if not found</returns>
        public static bool DestroySingleton<T>()
        {
            if (_singletons.TryGetValue(typeof(T), out object instance))
            {
                if (instance is MonoBehaviour monoBehaviour)
                {
                    var destroyMethod = instance.GetType().BaseType.GetMethod("DestroyInstance", BindingFlags.Public | BindingFlags.Static);
                    if (destroyMethod is not null)
                    {
                        destroyMethod.Invoke(null, null);
                        return true;
                    }
                    else
                    {
                        GameObject.Destroy(monoBehaviour.gameObject);
                        return true;
                    }
                }
                else
                {
                    var destroyMethod = instance.GetType().BaseType.GetMethod("Destroy", BindingFlags.Public | BindingFlags.Static);
                    if (destroyMethod is not null)
                    {
                        destroyMethod.Invoke(null, null);
                        return true;
                    }
                    else
                    {
                        Unregister<T>();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Destroys all singletons registered with the manager.
        /// </summary>
        public static void DestroyAllSingletons()
        {
            Type[] singletonTypes = new Type[_singletons.Count];
            _singletons.Keys.CopyTo(singletonTypes, 0);

            foreach (Type type in singletonTypes)
            {
                object instance = _singletons[type];

                try
                {
                    if (instance is MonoBehaviour monoBehaviour)
                    {
                        var destroyMethod = instance.GetType().BaseType.GetMethod("DestroyInstance", BindingFlags.Public | BindingFlags.Static);
                        if (destroyMethod != null)
                        {
                            destroyMethod.Invoke(null, null);
                        }
                        else
                        {
                            GameObject.Destroy(monoBehaviour.gameObject);
                        }
                    }
                    else
                    {
                        var destroyMethod = instance.GetType().BaseType.GetMethod("Destroy", BindingFlags.Public | BindingFlags.Static);
                        destroyMethod?.Invoke(null, null);
                    }
                }
                catch (Exception e)
                {
                    LogError($"Error destroying singleton of type {type.Name}: {e.Message}");
                }
            }

            if (_singletons.Count > 0) throw new Exception("Some singleton are not clear collectly.");
            // Clear any remaining registrations (in case any weren't properly unregistered during destruction)
            _singletons.Clear();
            Log("All singletons destroyed and manager cleared.");
        }
    }
}
