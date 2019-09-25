using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class IU 
{
    private SerializedObject _obj;

    private GUIStyle _stdStyle;
    private GUIStyle _fontStyle;

    private Dictionary<string,Texture2D> _colors;

    public IU()
    {
        _stdStyle = new GUIStyle ();
        _fontStyle = new GUIStyle ();
        _colors = new Dictionary<string, Texture2D> ();
    }
        
    //
    // Init and save
    //
    public void Init(SerializedObject obj)
    {
        _obj = obj;
        _obj.Update();
    }

    public void Save()
    {
        _obj.ApplyModifiedProperties();
    }

    //
    // Texture colors
    //
    public void AddColor(string key, Color color)
    {
        Color[] pix = new Color[1*1];

        for(int i = 0; i < pix.Length; i++)
            pix[i] = color;

        Texture2D result = new Texture2D(1, 1);
        result.SetPixels(pix);
        result.Apply();

        _colors.Add(key, result);
    }

    //
    // Getters
    //
    public SerializedProperty GetProperty(string name)
    {
        return _obj.FindProperty(name);
    }

    public SerializedProperty GetProperty(SerializedObject parent, string name)
    {
        return parent.FindProperty(name);
    }

    public SerializedProperty GetRelativeProperty(SerializedProperty parent, string name)
    {
        return parent.FindPropertyRelative(name);
    }


    //
    // Blocks and rows
    //
    Stack<GUIStyle> _styles = new Stack<GUIStyle>(10);

    public void BeginRow()
    {
        EditorGUILayout.BeginHorizontal(_stdStyle);
    }

    public void BeginCleanRow()
    {
        EditorGUILayout.BeginHorizontal();
    }

    public void BeginRow(string color)
    {
        var _oldStyle = _stdStyle;
        _stdStyle.normal.background = _colors [color];
        EditorGUILayout.BeginHorizontal(_stdStyle);
        _stdStyle = _oldStyle;
    }

    public void EndRow()
    {
        EditorGUILayout.EndHorizontal();
    }

    public void BeginBlock()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical(_stdStyle);
    }

    public void BeginCleanBlock()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical();
    }

    public void EndBlock()
    {
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
    }

    public void BeginCustomBlock(string color)
    {
        _styles.Push(_stdStyle);
        _stdStyle.normal.background = _colors [color];
        EditorGUILayout.BeginVertical(_stdStyle);
        EditorGUI.indentLevel++;
    }

    public void EndCustomBlock()
    {
        _stdStyle = _styles.Pop();
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
    }

    //
    // Space
    //
    public void Space ()
    {
        EditorGUILayout.BeginHorizontal(_stdStyle);
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }

    //
    // Standard Property
    //
    public void Property (ref SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property);
    }

    //
    // Custom Property
    //
    public void String(string label, ref SerializedProperty text)
    {
        text.stringValue = EditorGUILayout.TextField(label, text.stringValue);
    }

    //
    // Titles
    //
    public void SubTitle(string text)
    {
        _fontStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField(text, _fontStyle);
        _fontStyle.fontStyle = FontStyle.Normal;
    }

    public void Label(string text)
    {
        EditorGUILayout.LabelField(text, _fontStyle);
    }

    //
    // Button
    //
    public bool ButtonIsPressing (string label)
    {
        return GUILayout.Button(label);
    }

    public bool ButtonIsPressing (string label,  GUILayoutOption option)
    {
        return GUILayout.Button(label, option);
    }

    public void Button (string label, Action func)
    {
        if (GUILayout.Button(label))
            func();
    }

    public void Button (string label, Action func, GUILayoutOption option)
    {
        if (GUILayout.Button(label, option))
            func();
    }

    //
    // Foldout
    //
    public void Foldout(string label, ref SerializedProperty property)
    {
        property.boolValue = EditorGUILayout.Foldout(property.boolValue, label);
    }

    //
    // Dropdown
    //
    public int Dropdown(string label,ref SerializedProperty intKey, string[] stringValues)
    {
        intKey.intValue = EditorGUILayout.Popup(label, intKey.intValue, stringValues);
        return intKey.intValue;
    }

    //
    // Slider
    //
    public void Slider(string label, ref SerializedProperty valueFloat, float min, float max, string leftTip = "", string rightTip = "")
    {
        Slider(label, ref valueFloat, min, max, leftTip, rightTip, false);
    }

    public void IntSlider(string label, ref SerializedProperty valueInt, float min, float max, string leftTip = "", string rightTip = "")
    {
        Slider(label, ref valueInt, min, max, leftTip, rightTip, true);
    }

    private void Slider(string label, ref SerializedProperty value, float min, float max, string leftTip, string rightTip, bool isInt)
    {
        var footerLabel = new GUIStyle();
        footerLabel.fontSize = 9;
        footerLabel.normal.textColor = Color.gray;
        var padding = new GUIStyle ();
        padding.padding.bottom = 8;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        float totalWidth = GUILayoutUtility.GetLastRect().width;

        EditorGUILayout.BeginHorizontal(padding);
        EditorGUILayout.LabelField(label);
        Rect r = GUILayoutUtility.GetLastRect();
        r.x += 120; 
        r.width = totalWidth - 50 - 120;

        if (isInt)
            value.intValue = (int)GUI.HorizontalSlider(r, (float)value.intValue, min, max);
        else
            value.floatValue = GUI.HorizontalSlider(r, value.floatValue, min, max);
        
        r.y += 14;
        footerLabel.alignment = TextAnchor.UpperLeft;
        GUI.Label(r, leftTip, footerLabel);
        footerLabel.alignment = TextAnchor.UpperRight;
        GUI.Label(r, rightTip, footerLabel);

        if (isInt)
            value.intValue = (int)EditorGUILayout.FloatField((float)value.intValue, GUILayout.MaxWidth(68));
        else
            value.floatValue = EditorGUILayout.FloatField(value.floatValue, GUILayout.MaxWidth(68));

        EditorGUILayout.EndHorizontal();
    }

    //
    // Decorations
    //
    public void HorizontalBar()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1f);
        rect.height = 1f;
        EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
    }

    //
    // Utils
    //
    public string ValueOrDefault(string value, string defaultValue)
    {
        if (string.IsNullOrEmpty(value))
            return defaultValue;
        else
            return value;
    }
}
