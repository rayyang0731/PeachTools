using UnityEngine;
using UnityEditor;

public class BMFontEditor : EditorWindow
{
    [MenuItem("Tools/BMFont Maker")]
    static public void OpenBMFontMaker()
    {
        EditorWindow.GetWindow<BMFontEditor>(true, "BMFont Maker", true).ShowUtility();
    }

    [SerializeField]
    private Font targetFont;
    [SerializeField]
    private TextAsset fntData;
    [SerializeField]
    private Material fontMaterial;
    [SerializeField]
    private Texture2D fontTexture;

    private BMFont bmFont = new BMFont();

    public BMFontEditor()
    {
    }

    void OnGUI()
    {
        targetFont = EditorGUILayout.ObjectField("Target Font", targetFont, typeof(Font), false) as Font;
        fntData = EditorGUILayout.ObjectField("Fnt Data", fntData, typeof(TextAsset), false) as TextAsset;
        fontMaterial = EditorGUILayout.ObjectField("Font Material", fontMaterial, typeof(Material), false) as Material;
        fontTexture = EditorGUILayout.ObjectField("Font Texture", fontTexture, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Create BMFont"))
        {
            BMFontReader.Load(bmFont, fntData.name, fntData.bytes); // 借用NGUI封装的读取类
            CharacterInfo[] characterInfo = new CharacterInfo[bmFont.glyphs.Count];
            for (int i = 0; i < bmFont.glyphs.Count; i++)
            {
                BMGlyph bmInfo = bmFont.glyphs[i];
                CharacterInfo info = new CharacterInfo();

                info.glyphHeight = bmFont.texHeight;
                info.glyphWidth = bmFont.texWidth;
                info.index = bmInfo.index;

                info.uvTopLeft = new Vector2((float)bmInfo.x / bmFont.texWidth, 1 - (float)bmInfo.y / bmFont.texHeight);
                info.uvTopRight = new Vector2((float)(bmInfo.x + bmInfo.width) / bmFont.texWidth, 1 - (float)bmInfo.y / bmFont.texHeight);
                info.uvBottomLeft = new Vector2((float)bmInfo.x / bmFont.texWidth, 1 - (float)(bmInfo.y + bmInfo.height) / bmFont.texHeight);
                info.uvBottomRight = new Vector2((float)(bmInfo.x + bmInfo.width) / bmFont.texWidth, 1 - (float)(bmInfo.y + bmInfo.height) / bmFont.texHeight);

                info.minX = 0;
                info.minY = -bmInfo.height;
                info.maxX = bmInfo.width;
                info.maxY = 0;

                info.advance = bmInfo.advance;
                characterInfo[i] = info;
            }
            targetFont.characterInfo = characterInfo;
            if (fontMaterial)
            {
                fontMaterial.mainTexture = fontTexture;
            }
            targetFont.material = fontMaterial;
            fontMaterial.shader = Shader.Find("UI/Default Font");
			AssetDatabase.Refresh();
            Debug.Log("create font <" + targetFont.name + "> success");
            Close();
        }
    }
}
