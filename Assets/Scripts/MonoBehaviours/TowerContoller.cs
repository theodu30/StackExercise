using UnityEngine;

public class TowerContoller : MonoBehaviour
{
    public GameObject PerfectPlaceEffect;

    public Material blocMaterial;

    private int currentHue = 0;

    private GameObject previousBloc;
    private GameObject currentBloc;

    private bool spawningFromLeft = true;
    private float previousBlocWidth;
    private float currentBlocWidth;
    private float previousBlocLength;
    private float currentBlocLength;
    private float spawnDistance = 5f;
    private float xPositionDelta = 0f;
    private float zPositionDelta = 0f;
    private float currentDeltaX = 0f;
    private float currentDeltaZ = 0f;

    private float towerHeight = 0f;

    private int combo = 0;
    private float speed = 3f;

    private bool isGameOver = false;

    private void OnEnable()
    {
        GameManager.PlayerClicked += On_PlayerClicked;
        GameManager.BlocPlacePerfect += On_BlocPlacePerfect;
        GameManager.BlocPlaceSuccess += On_BlocPlaceSuccess;
        GameManager.BlocPlaceFailed += On_BlocPlaceFailed;
        GameManager.BlocPlaceMissed += On_BlocPlaceMissed;
    }

    private void OnDisable()
    {
        GameManager.PlayerClicked -= On_PlayerClicked;
        GameManager.BlocPlacePerfect -= On_BlocPlacePerfect;
        GameManager.BlocPlaceSuccess -= On_BlocPlaceSuccess;
        GameManager.BlocPlaceFailed -= On_BlocPlaceFailed;
        GameManager.BlocPlaceMissed -= On_BlocPlaceMissed;
    }

    private void Start()
    {
        currentHue = Random.Range(0, 360);

        previousBlocWidth = GameManager.Instance.MaxBlocWidth;
        previousBlocLength = GameManager.Instance.MaxBlocWidth;
        currentBlocWidth = GameManager.Instance.MaxBlocWidth;
        currentBlocLength = GameManager.Instance.MaxBlocWidth;

        spawningFromLeft = Random.value > 0.5f;
        previousBloc = GenerateNextBloc();
        previousBloc.transform.position = new Vector3(0f, 1f, 0f);

        towerHeight++;
        currentBloc = GenerateNextBloc();
        currentBloc.transform.position = ComputeSpawnPosition();
    }

    private void Update()
    {
        if (isGameOver) return;

        if (spawningFromLeft)
        {
            currentBloc.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            if (currentBloc.transform.position.x > spawnDistance)
            {
                currentBloc.transform.position = new Vector3(-spawnDistance, currentBloc.transform.position.y, currentBloc.transform.position.z);
            }
        }
        else
        {
            currentBloc.transform.position += new Vector3(0f, 0f, -speed * Time.deltaTime);
            if (currentBloc.transform.position.z < -spawnDistance)
            {
                currentBloc.transform.position = new Vector3(currentBloc.transform.position.x, currentBloc.transform.position.y, spawnDistance);
            }
        }
    }

    private Color ComputeNextColor()
    {
        currentHue = (currentHue + 10) % 360;
        return Color.HSVToRGB(currentHue / 360f, .39f, .93f);
    }

    private GameObject GenerateNextBloc()
    {
        GameObject bloc = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bloc.GetComponent<Renderer>().material = blocMaterial;
        bloc.GetComponent<Renderer>().material.color = ComputeNextColor();
        bloc.transform.parent = this.transform;
        bloc.transform.localScale = new Vector3(currentBlocWidth, 1f, currentBlocLength);
        return bloc;
    }

    private Vector3 ComputeSpawnPosition()
    {
        if (spawningFromLeft)
        {
            return new Vector3(-spawnDistance, towerHeight + 1f, zPositionDelta + previousBloc.transform.position.z);
        }
        else
        {
            return new Vector3(xPositionDelta + previousBloc.transform.position.x, towerHeight + 1f, spawnDistance);
        }
    }

    private void On_PlayerClicked(object sender, System.EventArgs args)
    {
        if (isGameOver) return;
        PlaceCurrentBloc();
    }

    private void PlaceCurrentBloc()
    {
        Vector3 previousPos = previousBloc.transform.position;
        Vector3 currentPos = currentBloc.transform.position;

        if (spawningFromLeft)
        {
            currentDeltaX = currentPos.x - previousPos.x;
            if (Mathf.Abs(currentDeltaX) >= currentBlocWidth)
            {
                GameManager.OnBlocPlaceMissed();
                return;
            }
            if (Mathf.Abs(currentDeltaX) < .05 * currentBlocWidth)
            {
                combo++;
                GameManager.OnBlocPlacePerfect();
            }
            else
            {
                combo = 0;
                GameManager.OnBlocPlaceFailed();
            }
        }
        else
        {
            currentDeltaZ = currentPos.z - previousPos.z;
            if (Mathf.Abs(currentDeltaZ) >= currentBlocLength)
            {
                GameManager.OnBlocPlaceMissed();
                return;
            }
            if (Mathf.Abs(currentDeltaZ) < .05 * currentBlocLength)
            {
                combo++;
                GameManager.OnBlocPlacePerfect();
            }
            else
            {
                combo = 0;
                GameManager.OnBlocPlaceFailed();
            }
        }

        GameManager.OnBlocPlaceSuccess();
    }

    private void On_BlocPlacePerfect(object sender, System.EventArgs args)
    {
        if (combo >= 5)
        {
            if (spawningFromLeft)
            {
                float newWidth = Mathf.Min(currentBlocWidth * 1.15f, GameManager.Instance.MaxBlocWidth);
                float widthIncrease = newWidth - currentBlocWidth;
                currentBlocWidth = newWidth;
                currentBloc.transform.localScale = new Vector3(currentBlocWidth, 1f, currentBlocLength);
                float newX = previousBloc.transform.position.x + widthIncrease / 2f * Mathf.Sign(currentDeltaX);
                currentBloc.transform.position = new Vector3(newX, currentBloc.transform.position.y, previousBloc.transform.position.z);
            }
            else
            {
                float newLength = Mathf.Min(currentBlocLength * 1.15f, GameManager.Instance.MaxBlocWidth);
                float lengthIncrease = newLength - currentBlocLength;
                currentBlocLength = newLength;
                currentBloc.transform.localScale = new Vector3(currentBlocWidth, 1f, currentBlocLength);
                float newZ = previousBloc.transform.position.z - lengthIncrease / 2f * Mathf.Sign(currentDeltaZ);
                currentBloc.transform.position = new Vector3(previousBloc.transform.position.x, currentBloc.transform.position.y, newZ);
            }
        }
        else
        {
            if (spawningFromLeft)
            {
                currentBloc.transform.position = new Vector3(previousBloc.transform.position.x, currentBloc.transform.position.y, currentBloc.transform.position.z);
            }
            else
            {
                currentBloc.transform.position = new Vector3(currentBloc.transform.position.x, currentBloc.transform.position.y, previousBloc.transform.position.z);
            }
        }

        GameObject perfectEffect = Instantiate(PerfectPlaceEffect);
        perfectEffect.transform.position = currentBloc.transform.position + new Vector3(0f, -0.5f, 0f);
        PerfectEffectController effectController = perfectEffect.GetComponent<PerfectEffectController>();
        effectController.SizeFromBlocSize(new Vector2(currentBlocWidth, currentBlocLength));
        effectController.ExecuteEffect();
    }

    private void On_BlocPlaceSuccess(object sender, System.EventArgs args)
    {
        previousBloc = currentBloc;
        towerHeight++;
        GameManager.OnScoreUpdated(towerHeight);
        spawningFromLeft = !spawningFromLeft;
        currentBloc = GenerateNextBloc();
        currentBloc.transform.position = ComputeSpawnPosition();

        if (towerHeight % 15 == 0)
        {
            speed *= 1.1f;
        }
    }

    private void On_BlocPlaceMissed(object sender, System.EventArgs args)
    {
        if (isGameOver) return;
        isGameOver = true;
        currentBloc.AddComponent<Rigidbody>();
        //Debug.Log("Game Over! Final Tower Height: " + (towerHeight));
    }

    private void On_BlocPlaceFailed(object sender, System.EventArgs args)
    {
        // Create a falling bloc for the cut-off part
        GameObject fallingBloc = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingBloc.GetComponent<Renderer>().material = blocMaterial;
        fallingBloc.GetComponent<Renderer>().material.color = Color.red;

        if (spawningFromLeft)
        {
            previousBlocWidth = currentBlocWidth;
            currentBlocWidth -= Mathf.Abs(currentDeltaX);
            float newX = previousBloc.transform.position.x + (currentDeltaX / 2f);
            currentBloc.transform.position = new Vector3(newX, currentBloc.transform.position.y, currentBloc.transform.position.z);
            currentBloc.transform.localScale = new Vector3(currentBlocWidth, 1f, currentBlocLength);

            zPositionDelta = currentBloc.transform.position.z - previousBloc.transform.position.z;

            float cutWidth = previousBlocWidth - currentBlocWidth;
            float cutX;
            if (previousBloc.transform.position.x < currentBloc.transform.position.x)
            {
                cutX = currentBloc.transform.position.x + (currentBlocWidth / 2f) + (cutWidth / 2f);
            }
            else
            {
                cutX = currentBloc.transform.position.x - (currentBlocWidth / 2f) - (cutWidth / 2f);
            }
            fallingBloc.transform.position = new Vector3(cutX, currentBloc.transform.position.y, currentBloc.transform.position.z);
            fallingBloc.transform.localScale = new Vector3(cutWidth, 1f, currentBlocLength);
        }
        else
        {
            previousBlocLength = currentBlocLength;
            currentBlocLength -= Mathf.Abs(currentDeltaZ);
            float newZ = previousBloc.transform.position.z + (currentDeltaZ / 2f);
            currentBloc.transform.position = new Vector3(currentBloc.transform.position.x, currentBloc.transform.position.y, newZ);
            currentBloc.transform.localScale = new Vector3(currentBlocWidth, 1f, currentBlocLength);

            xPositionDelta = currentBloc.transform.position.x - previousBloc.transform.position.x;

            float cutLength = previousBlocLength - currentBlocLength;
            float cutZ;
            if (previousBloc.transform.position.z < currentBloc.transform.position.z)
            {
                cutZ = currentBloc.transform.position.z + (currentBlocLength / 2f) + (cutLength / 2f);
            }
            else
            {
                cutZ = currentBloc.transform.position.z - (currentBlocLength / 2f) - (cutLength / 2f);
            }
            fallingBloc.transform.position = new Vector3(currentBloc.transform.position.x, currentBloc.transform.position.y, cutZ);
            fallingBloc.transform.localScale = new Vector3(currentBlocWidth, 1f, cutLength);
        }

        fallingBloc.AddComponent<Rigidbody>();

        Destroy(fallingBloc, 5f);
    }
}
