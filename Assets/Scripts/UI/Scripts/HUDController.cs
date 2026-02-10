using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    VisualElement root;
    Label scoreLabel;
    VisualElement buttons;
    Button restart;
    Button home;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        scoreLabel = root.Q<Label>();
        buttons = root.Q<VisualElement>("Buttons");
        restart = root.Q<Button>("Restart");
        home = root.Q<Button>("Home");

        buttons.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        GameManager.ScoreUpdated += On_ScoreChanged;
        GameManager.BlocPlaceMissed += On_BlocPlaceMissed;
        restart.clicked += OnRestartClicked;
        home.clicked += OnHomeClicked;
    }

    private void OnDisable()
    {
        GameManager.ScoreUpdated -= On_ScoreChanged;
        GameManager.BlocPlaceMissed -= On_BlocPlaceMissed;
        restart.clicked -= OnRestartClicked;
        home.clicked -= OnHomeClicked;
    }

    private void On_BlocPlaceMissed(object sender, System.EventArgs e)
    {
        buttons.style.display = DisplayStyle.Flex;
    }

    private void On_ScoreChanged(object sender, float newScore)
    {
        scoreLabel.text = newScore.ToString();
    }

    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnHomeClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
