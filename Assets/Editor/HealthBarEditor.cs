/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/26/2021
    Desc    : Health Bar Editor
*/
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthBar))]
public class HealthBarEditor : Editor
{
    public HealthBar healthBar = null;

    public override void OnInspectorGUI()
    {
        healthBar = (HealthBar)target;
        if (healthBar == null)
            return;
        base.OnInspectorGUI();

        if (GUILayout.Button("Update/Show"))
        {
            Status status = healthBar.gameObject.GetComponent<Status>();
            if (status != null)
            {
                healthBar.RemoveUIandHealthBar();
                healthBar.Initialize(status.maxHealth);
            }
        }

    }
}
