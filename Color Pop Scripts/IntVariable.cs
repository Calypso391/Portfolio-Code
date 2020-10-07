using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject {

#if UNITY_EDITOR
    [TextArea]
    public string DeveloperDescription = "";
#endif
    public int InitialValue;
    public int Value;

    private void OnEnable() {
        Value = InitialValue;
    }

    public void SetValue(int value) {
        Value = value;
    }

    public void SetValue(IntVariable value) {
        Value = value.Value;
    }

    public void ApplyChange(int amount) {
        Value += amount;
    }

    public void ApplyChange(IntVariable amount) {
        Value += amount.Value;
    }
}