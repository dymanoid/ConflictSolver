// <copyright file="GameConnection.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using UnityEngine;

namespace ConflictSolver
{
    /// <summary>
    /// A service class for interacting with the Unity engine.
    /// </summary>
    internal static class GameConnection
    {
        /// <summary>
        /// Gets an instance of the <typeparamref name="T"/> type by querying the Unity engine
        /// for a component of that type. If there is no such component available, it will be created.
        /// </summary>
        /// <typeparam name="T">The type to get an instance of. Must be a <see cref="Component"/>.</typeparam>
        /// <returns>An instance of the <typeparamref name="T"/> type.</returns>
        public static T GetInstance<T>()
            where T : Component
        {
            string objectTag = GetObjectTag<T>();
            var gameObject = GameObject.Find(objectTag) ?? CreateGameObject(objectTag);
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Destroys the instance of the <typeparamref name="T"/> type if it exists in the game.
        /// If no instance is available, nothing occurs.
        /// </summary>
        /// <typeparam name="T">A type to destroy an instance of. Must be a <see cref="Component"/>.</typeparam>
        public static void DestroyInstance<T>()
            where T : Component
            => DestroyInstance<T>(cleanUp: null);

        /// <summary>
        /// Destroys the instance of the <typeparamref name="T"/> type if it exists in the game.
        /// If no instance is available, nothing occurs.
        /// </summary>
        /// <typeparam name="T">A type to destroy an instance of. Must be a <see cref="Component"/>.</typeparam>
        /// <param name="cleanUp">A delegate that will be called for user-specific instance clean-up.
        /// This delegate will not be called if there is nothing to clean up.</param>
        public static void DestroyInstance<T>(Action<T> cleanUp)
             where T : Component
        {
            string objectTag = GetObjectTag<T>();
            var gameObject = GameObject.Find(objectTag);
            if (gameObject is null)
            {
                return;
            }

            try
            {
                var instance = gameObject.GetComponent<T>();
                if (instance != null && cleanUp != null)
                {
                    cleanUp(instance);
                }
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        private static GameObject CreateGameObject(string objectTag)
        {
            var result = new GameObject(objectTag);
            UnityEngine.Object.DontDestroyOnLoad(result);
            return result;
        }

        private static string GetObjectTag<T>() => typeof(T).FullName;
    }
}
