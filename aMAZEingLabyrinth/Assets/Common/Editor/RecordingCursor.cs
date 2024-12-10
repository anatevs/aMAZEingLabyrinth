using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
namespace GameUI
{
    public class RecordingCursor : MonoBehaviour
    {
        void Start ()
        {
            Cursor.visible = true;
            Cursor.SetCursor(PlayerSettings.defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
#endif