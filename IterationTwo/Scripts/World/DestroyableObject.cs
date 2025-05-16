using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public string ObjectID { get; private set; }

    private void Start()
    {
        if (string.IsNullOrEmpty(ObjectID))
        {
            ObjectID = gameObject.name + "_" + transform.position.ToString();
        }
    }
}
