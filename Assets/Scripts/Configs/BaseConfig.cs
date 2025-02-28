using System;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
public class ScriptableObjectIdDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;
        if (string.IsNullOrEmpty(property.stringValue)) {
            property.stringValue = Guid.NewGuid().ToString();
        }
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

[CreateAssetMenu(fileName = "config", menuName = "Configs/ Config")]
public class BaseConfig : ScriptableObject
{
    [ScriptableObjectId]
    [SerializeField]
    [ContextMenuItem("Regenerate", "RegenerationId")]
    private  string id;
    
    public string ID => id;
    
    [ContextMenu("RegenerationId")]
    private void RegenerationId()
    {
        id = Guid.NewGuid().ToString();
    }
}