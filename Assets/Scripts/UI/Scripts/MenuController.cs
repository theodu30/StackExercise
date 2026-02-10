using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    VisualElement root;
    Button play;
    Button quit;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        play = root.Q<Button>("Play");
        quit = root.Q<Button>("Quit");
    }

    private void OnEnable()
    {
        play.clicked += OnPlayClicked;
        quit.clicked += OnQuitClicked;
    }

    private void OnDisable()
    {
        play.clicked -= OnPlayClicked;
        quit.clicked -= OnQuitClicked;
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
