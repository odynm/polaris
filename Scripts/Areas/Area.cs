// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class Area : MonoBehaviour {

    void Awake()
    {
        if (GetComponent<Collider>() == null)
        {
            Collider col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        DrawIcon(gameObject, 0);
#endif
    }

#if UNITY_EDITOR
    private void DrawIcon(GameObject gameObject, int idx)
    {
        var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);
        var icon = largeIcons[idx];
        var egu = typeof(EditorGUIUtility);
        var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        var args = new object[] { gameObject, icon.image };
        var setIcon = egu.GetMethod("SetIconForObject", flags, null, new System.Type[] { typeof(UnityEngine.Object), typeof(Texture2D) }, null);
        setIcon.Invoke(null, args);
    }

    private GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
    {
        GUIContent[] array = new GUIContent[count];
        for (int i = 0; i < count; i++)
        {
            array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
        }
        return array;
    }
#endif
}
