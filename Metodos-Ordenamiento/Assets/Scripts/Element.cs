using System.Collections;
using UnityEngine;

public class Element : MonoBehaviour
{
    public delegate void ElementClick(byte elementID);
    public delegate void ElementMove(byte elementID, bool isMoving);

    public static event ElementClick elementClick;
    public static event ElementMove elementMove;

    private Color initColor = Color.white;
    public byte elementID { get; set; } = 0;

    public Color InitColor
    {
        get => initColor;
        set
        {
            initColor = value;
            GetComponent<SpriteRenderer>().color = initColor;
        }
    }

    public void SetColor(Color color) => GetComponent<SpriteRenderer>().color = color;
    public void ResetColor() => SetColor(initColor);

    private void OnMouseUp() => elementClick?.Invoke(elementID);

    /// <summary>
    /// Inicia movimiento hacia una posición
    /// </summary>
    public void MoveTo(Vector2 pos) => StartCoroutine(Move(transform.localPosition, pos));

    private IEnumerator Move(Vector2 startPos, Vector2 destPos)
    {
        elementMove?.Invoke(elementID, true);
        yield return StartCoroutine(MoveElement(startPos, destPos));
        elementMove?.Invoke(elementID, false);
    }

    /// <summary>
    /// Movimiento con interpolación
    /// </summary>
    private IEnumerator MoveElement(Vector3 startPos, Vector3 destPos)
    {
        float totalMovementTime = 1f;
        float currentMovementTime = 0f;

        // Mover en Y
        startPos = transform.localPosition;
        while (Mathf.Abs(transform.localPosition.y - destPos.y) > 0.01f)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x, destPos.y), currentMovementTime / totalMovementTime);
            yield return null;
        }

        // Mover en X
        totalMovementTime = 1f;
        currentMovementTime = 0f;
        startPos = transform.localPosition;

        while (Mathf.Abs(transform.localPosition.x - destPos.x) > 0.01f)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, new Vector3(destPos.x, 0.0f), currentMovementTime / totalMovementTime);
            yield return null;
        }
    }

    // Métodos de coloreado rápido
    public void HighlightRed() => SetColor(Color.red);
    public void HighlightGreen() => SetColor(Color.green);
    public void HighlightBlue() => SetColor(Color.blue);
}