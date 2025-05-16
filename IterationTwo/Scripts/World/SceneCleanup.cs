using UnityEngine;
public class SceneCleanup : MonoBehaviour
{
    private void Start()
    {
        var allDestroyables = FindObjectsByType<DestroyableObject>(0);
        foreach (var obj in allDestroyables)
        {
            if (ObjectTracker.DestroyedObjectIDs.Contains(obj.ObjectID))
            {
                Destroy(obj.gameObject);
            }
        }
    }
}
