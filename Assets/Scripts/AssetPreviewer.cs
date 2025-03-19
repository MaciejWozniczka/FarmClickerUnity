using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClickItem))]
public class AssetPreviewer : Editor
{
    ClickItem item;
    Texture2D tex1;
    Texture2D tex2;

    public override void OnInspectorGUI()
    {
        item = (ClickItem)target;

        GUILayout.BeginHorizontal();

        tex1 = AssetPreview.GetAssetPreview(item.ItemImage);
        GUILayout.Label(tex1);

        tex2 = AssetPreview.GetAssetPreview(item.UnknownItemImage);
        GUILayout.Label(tex2);

        GUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}