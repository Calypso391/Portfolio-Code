using UnityEngine;
using UnityEngine.Events;

public class StartBehaviour : MonoBehaviour
{
    [SerializeField] private bool DoesSceneStartAlreadyActive = true;
    [SerializeField] private UnityEvent OnSceneStart = null;
    private bool isSceneActive = false;

    private void Update()
    {
        bool shouldStart =
            DoesSceneStartAlreadyActive ||
            (Input.GetKeyDown(KeyCode.Space) && !isSceneActive);

        if (shouldStart) { // do once
            isSceneActive = true;
            OnSceneStart.Invoke();
            DoesSceneStartAlreadyActive = false;
        }
    }
}
