using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float MaxBlocWidth = 4f;

    public float TowerHeight = 0;

    // Events
    public static EventHandler PlayerClicked;
    public static void OnPlayerClicked()
    {
        PlayerClicked?.Invoke(null, EventArgs.Empty);
    }

    public static EventHandler BlocPlacePerfect;
    public static void OnBlocPlacePerfect()
    {
        BlocPlacePerfect?.Invoke(null, EventArgs.Empty);
    }

    public static EventHandler BlocPlaceSuccess;
    public static void OnBlocPlaceSuccess()
    {
        BlocPlaceSuccess?.Invoke(null, EventArgs.Empty);
    }

    public static EventHandler BlocPlaceFailed;
    public static void OnBlocPlaceFailed()
    {
        BlocPlaceFailed?.Invoke(null, EventArgs.Empty);
    }

    public static EventHandler BlocPlaceMissed;
    public static void OnBlocPlaceMissed()
    {
        BlocPlaceMissed?.Invoke(null, EventArgs.Empty);
    }

    public static EventHandler<float> ScoreUpdated;
    public static void OnScoreUpdated(float newScore)
    {
        Instance.TowerHeight = newScore;
        ScoreUpdated?.Invoke(null, newScore);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public InputAction PlayerClick;

    private void OnEnable()
    {
        PlayerClick.Enable();
        PlayerClick.performed += OnPlayerClickPerformed;
    }

    private void OnDisable()
    {
        PlayerClick.performed -= OnPlayerClickPerformed;
        PlayerClick.Disable();
    }

    private void OnPlayerClickPerformed(InputAction.CallbackContext context)
    {
        OnPlayerClicked();
    }
}
