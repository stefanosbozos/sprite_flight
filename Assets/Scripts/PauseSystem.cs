using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseSystem : MonoBehaviour
{
    [SerializeField]
    private UIDocument UIDoc = null;

    private VisualElement pauseMenu;

    InputAction pauseGame;

    bool isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseGame = InputSystem.actions.FindAction("PauseGame");

        pauseMenu = UIDoc.rootVisualElement.Q<VisualElement>("PauseMenu");
        pauseMenu.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    void Pause()
    {
        if (pauseGame.WasPressedThisFrame())
        {
            Time.timeScale = isPaused ? 0 : 1;
            pauseMenu.style.display = DisplayStyle.Flex;
            isPaused = !isPaused;

        }
    }

    public bool GameIsPaused()
    {
        return isPaused;
    }

}
