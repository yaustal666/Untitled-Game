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
                DamageData damageInfo = new DamageData
                {
                    Damage = 10000f
                };
                damagable.TakeDamage(damageInfo);
            }
        }
    }
}