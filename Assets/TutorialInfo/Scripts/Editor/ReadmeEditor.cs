using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor
{
    private static readonly string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";

    private static readonly float kSpace = 16f;


    private bool m_Initialized;

    static ReadmeEditor()
    {
        EditorApplication.delayCall += SelectReadmeAutomatically;
    }

    [field: SerializeField] private GUIStyle LinkStyle { get; set; }

    [field: SerializeField] private GUIStyle TitleStyle { get; set; }

    [field: SerializeField] private GUIStyle HeadingStyle { get; set; }

    [field: SerializeField] private GUIStyle BodyStyle { get; set; }

    private static void SelectReadmeAutomatically()
    {
        if (!SessionState.GetBool(kShowedReadmeSessionStateName, false))
        {
            var readme = SelectReadme();
            SessionState.SetBool(kShowedReadmeSessionStateName, true);

            if (readme && !readme.loadedLayout)
            {
                LoadLayout();
                readme.loadedLayout = true;
            }
        }
    }

    private static void LoadLayout()
    {
        var assembly = typeof(EditorApplication).Assembly;
        var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
        var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
        method.Invoke(null, new object[] {Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false});
    }

    [MenuItem("Tutorial/Show Tutorial Instructions")]
    private static Readme SelectReadme()
    {
        var ids = AssetDatabase.FindAssets("Readme t:Readme");
        if (ids.Length == 1)
        {
            var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

            Selection.objects = new[] {readmeObject};

            return (Readme) readmeObject;
        }

        Debug.Log("Couldn't find a readme");
        return null;
    }

    protected override void OnHeaderGUI()
    {
        var readme = (Readme) target;
        Init();

        var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

        GUILayout.BeginHorizontal("In BigTitle");
        {
            GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
            GUILayout.Label(readme.title, TitleStyle);
        }
        GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        var readme = (Readme) target;
        Init();

        foreach (var section in readme.sections)
        {
            if (!string.IsNullOrEmpty(section.heading)) GUILayout.Label(section.heading, HeadingStyle);
            if (!string.IsNullOrEmpty(section.text)) GUILayout.Label(section.text, BodyStyle);
            if (!string.IsNullOrEmpty(section.linkText))
                if (LinkLabel(new GUIContent(section.linkText)))
                    Application.OpenURL(section.url);
            GUILayout.Space(kSpace);
        }
    }

    private void Init()
    {
        if (m_Initialized)
            return;
        BodyStyle = new GUIStyle(EditorStyles.label);
        BodyStyle.wordWrap = true;
        BodyStyle.fontSize = 14;

        TitleStyle = new GUIStyle(BodyStyle);
        TitleStyle.fontSize = 26;

        HeadingStyle = new GUIStyle(BodyStyle);
        HeadingStyle.fontSize = 18;

        LinkStyle = new GUIStyle(BodyStyle);
        LinkStyle.wordWrap = false;
        // Match selection color which works nicely for both light and dark skins
        LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        LinkStyle.stretchWidth = false;

        m_Initialized = true;
    }

    private bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, LinkStyle);
    }
}