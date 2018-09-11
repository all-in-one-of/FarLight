using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour {

    public bool CustomCursor;
    public Texture2D cursorTexture;

    private void Awake()
    {
        if (CustomCursor)
        {
            Cursor.SetCursor(cursorTexture, new Vector2(14, 20), CursorMode.Auto);
        }
    }
}
