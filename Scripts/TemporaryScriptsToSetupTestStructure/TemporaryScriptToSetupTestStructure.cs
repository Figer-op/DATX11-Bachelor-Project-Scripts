using UnityEngine;

public class TemporaryScriptToSetupTestStructure : MonoBehaviour
{
    [field: SerializeField]
    public int MyTestValue { get; private set; } = 5;
}
