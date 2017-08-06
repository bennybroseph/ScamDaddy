using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ScrollingText))]
public class ScrollingTextEditor : TextEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_TextSpeed"));
        serializedObject.ApplyModifiedProperties();
    }
}
