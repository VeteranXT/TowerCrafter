// 8/15/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public abstract class BaseMonoBehaviour : MonoBehaviour
{
    #region Unity Lifecycle

    // Called when the script instance is being loaded
    protected virtual void Awake()
    {
        FindReferences();
    }

    // Called before the first frame update
    protected virtual void Start()
    {
        Subscribe();
    }

    // Called when the MonoBehaviour is destroyed
    protected virtual void OnDestroy()
    {
        Unsubscribe();
    }

    #endregion

    #region Virtual Methods

    /// <summary>
    /// Override this method to find and cache references to components or objects.
    /// </summary>
    protected virtual void FindReferences()
    {
        // Example: Find references to components or objects
    }

    /// <summary>
    /// Override this method to subscribe to events or initialize logic.
    /// </summary>
    protected virtual void Subscribe()
    {
        // Example: Subscribe to events
    }

    /// <summary>
    /// Override this method to unsubscribe from events or clean up resources.
    /// </summary>
    protected virtual void Unsubscribe()
    {
        // Example: Unsubscribe from events
    }

    #endregion
}