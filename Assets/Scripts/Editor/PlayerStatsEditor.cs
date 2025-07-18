using UnityEditor;

[CustomEditor(typeof(PlayerStatsSO))]
public class PlayerStatsEditor : Editor
{
    private Editor editorInstance;

    private void OnEnable()
    {
        // reset the edito instance;
        editorInstance = null;
    }

    public override void OnInspectorGUI()
    {
        // the inspected target component
        PlayerStatsSO playerStats = (PlayerStatsSO)target;

        if (editorInstance == null)
        {
            editorInstance = Editor.CreateEditor(playerStats.config);
        }

        // show the variables from the MonoBehaviour
        base.OnInspectorGUI();

        // draw the ScriptableObjects inspector
        editorInstance.DrawDefaultInspector();
    }
}