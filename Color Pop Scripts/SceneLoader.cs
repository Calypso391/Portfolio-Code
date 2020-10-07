using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneReference scene = null;

    public void Load(float delay)
    {
        Load(delay, LoadSceneMode.Single);
    }

    public void Load(SceneReference scene, float delay = 0.0f)
    {
        this.scene = scene;
        Load(delay);
    }

    public void Load(float delay = 0.0f, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (delay == 0.0f) {
            SceneManager.LoadScene(scene.ScenePath, mode);
        }
        else {
            StartCoroutine(LoadAsync(delay, mode));
        }
    }

    private IEnumerator LoadAsync(float delay = 0.0f, LoadSceneMode mode = LoadSceneMode.Single)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene.ScenePath, mode);
    }
}
