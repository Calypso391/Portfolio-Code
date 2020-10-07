using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class IntToUIText : MonoBehaviour
{
    [SerializeField] private IntVariable intVariable = null;
    private Text text = null;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (intVariable) {
            text.text = intVariable.Value.ToString();
        }
    }
}
