using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class EditorDebugUtil : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (target is IDamagable damagable)
        {
            if (GUILayout.Button("Kill"))
            {
                damagable.TakeDamage(10000);
            }
        }
    }
}