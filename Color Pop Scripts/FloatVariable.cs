using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{

#if UNITY_EDITOR
    [TextArea]
    public string DeveloperDescription = "";
#endif
    public float InitialValue;
    public float Value;

    private void OnEnable()
    {
        Value = InitialValue;
    }

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(float amount)
    {
        Value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        Value += amount.Value;
    }
}