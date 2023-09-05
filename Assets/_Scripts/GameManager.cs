using DG.Tweening;
using StarterAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private StarterAssetsInputs personInput;
    private Gun gun;

    private void Awake()
    {
        personInput = FindObjectOfType<StarterAssetsInputs>();
        gun = FindObjectOfType<Gun>();
        ToGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchCursor();
    }

    public void ToGame()
    {
        Cursor.visible = false;
        personInput.Activate();
        gun.Activate();
    }

    private void SwitchCursor()
    {
        if(!Cursor.visible)
            Cursor.visible = true;
        personInput.SwitchCursor();
        gun.SwitchCursor();
    }

    public void ResetScene()
    {
        StopAllCoroutines();
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
