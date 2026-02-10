using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 currentPosition;
    Vector3 currentCameraPosition;
    Vector3 currentCameraRotation;

    private bool isGameOver = false;

    private void Start()
    {
        currentPosition = transform.position;
        currentCameraPosition = Camera.main.transform.localPosition;
        currentCameraRotation = Camera.main.transform.localEulerAngles;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentPosition, 0.075f);

        if (isGameOver)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, currentCameraPosition, 0.05f);
            Camera.main.transform.localEulerAngles = Vector3.Lerp(Camera.main.transform.localEulerAngles, currentCameraRotation, 0.05f);
        }
    }

    private void OnEnable()
    {
        GameManager.BlocPlaceSuccess += On_BlocPlaceSuccess;
        GameManager.BlocPlaceMissed += On_BlocPlaceMissed;
    }

    private void OnDisable()
    {
        GameManager.BlocPlaceSuccess -= On_BlocPlaceSuccess;
        GameManager.BlocPlaceMissed -= On_BlocPlaceMissed;
    }

    private void On_BlocPlaceSuccess(object sender, System.EventArgs args)
    {
        currentPosition += Vector3.up;
    }

    private void On_BlocPlaceMissed(object sender, System.EventArgs args)
    {
        if (isGameOver) return;

        isGameOver = true;
        float endPositionY = GameManager.Instance.TowerHeight / 2f;
        currentPosition = new Vector3(0f, endPositionY, 0f);
        Camera.main.orthographicSize = endPositionY + 5f;

        currentCameraPosition = new Vector3(0f, 0f, -endPositionY - 2f);
        currentCameraRotation = new Vector3(0f, 0f, 0f);
    }
}
