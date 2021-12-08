using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YsoCorp {
    namespace GameUtils {

        [ExecuteInEditMode]
        [RequireComponent(typeof(YCManager))]
        public class YCManagerEditor : MonoBehaviour {
#if UNITY_EDITOR
            void OnEnable() {
                EditorApplication.hierarchyWindowItemOnGUI += YCManagerEditor.HierarchyIconCallback;
            }

            void OnDisable() {
                EditorApplication.hierarchyWindowItemOnGUI -= YCManagerEditor.HierarchyIconCallback;
            }

            public static void HierarchyIconCallback (int instanceID, Rect rectangle) {
                GameObject go = (GameObject)EditorUtility.InstanceIDToObject(instanceID);
                if (go != null && go.GetComponent<YCManager>() != null) {
                    Texture2D ycIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/GameUtils/Sprites/YCIcon.png", typeof(Texture2D));
                    Graphics.DrawTexture(new Rect(GUILayoutUtility.GetLastRect().width - rectangle.height - 5, rectangle.y, rectangle.height, rectangle.height), ycIcon);
                }
            }
#endif
        }
    }
}
