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
        isPaused = false;
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
            //Change the pause state, set timescale and show the menu.
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            if (isPaused)
            {
                pauseMenu.style.display = DisplayStyle.Flex;
            }
            else
            {
                pauseMenu.style.display = DisplayStyle.None;
            }
        }
    }

    public bool GameIsPaused()
    {
        // This is used from other parts of the game to pause execution.
        return isPaused;
    }

}
