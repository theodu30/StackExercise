using UnityEngine;

public class PerfectEffectController : MonoBehaviour
{
    private float width = 1f;
    private float length = 1f;

    private float maxWidth = 2f;
    private float maxLength = 2f;

    private bool running = false;

    private void Update()
    {
        if (running)
        {
            width = Mathf.Lerp(width, maxWidth, Time.deltaTime * 10f);
            length = Mathf.Lerp(length, maxLength, Time.deltaTime * 10f);

            if (width >= maxWidth * 0.99f && length >= maxLength * 0.99f)
            {
                running = false;
                Destroy(gameObject);
            }
            else
            {
                UpdateTransform();
            }
        }
    }

    public void SizeFromBlocSize(Vector2 blocSize)
    {
        width = blocSize.x / 10f;
        length = blocSize.y / 10f;
        maxWidth = 2 * width;
        maxLength = 2 * length;
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        transform.localScale = new Vector3(width, 1f, length);
    }

    public void ExecuteEffect()
    {
        running = true;
    }
}
