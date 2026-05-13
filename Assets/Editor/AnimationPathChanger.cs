using UnityEngine;
using UnityEditor;

public class AnimationPathChanger : EditorWindow
{
    private string newPath = "ChildObjeAdi";

    [MenuItem("Tools/Animation Path Changer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationPathChanger>("Path Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Path Güncelleyici", EditorStyles.boldLabel);
        newPath = EditorGUILayout.TextField("Child Obje Adý:", newPath);

        if (GUILayout.Button("Seçili Animasyonlarý Dönüþtür"))
        {
            UpdatePaths();
        }
    }

    private void UpdatePaths()
    {
        Object[] selectedClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);

        foreach (AnimationClip clip in selectedClips)
        {
            // Sprite (Object Reference) deðiþimlerini yakala
            EditorCurveBinding[] objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (var binding in objectBindings)
            {
                // Eðer halihazýrda boþsa (root) veya yanlýþ path ise deðiþtir
                if (binding.path != newPath)
                {
                    ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

                    // Eski yolu tamamen temizle
                    AnimationUtility.SetObjectReferenceCurve(clip, binding, null);

                    // Yeni yolu tanýmla
                    EditorCurveBinding newBinding = binding;
                    newBinding.path = newPath;

                    // Yeni yola keyframeleri bas
                    AnimationUtility.SetObjectReferenceCurve(clip, newBinding, keyframes);
                }
            }

            // Normal float deðerleri (Alpha, Color vb.) varsa onlarý da taþý
            EditorCurveBinding[] floatBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in floatBindings)
            {
                if (binding.path != newPath)
                {
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                    AnimationUtility.SetEditorCurve(clip, binding, null);

                    EditorCurveBinding newBinding = binding;
                    newBinding.path = newPath;
                    AnimationUtility.SetEditorCurve(clip, newBinding, curve);
                }
            }

            EditorUtility.SetDirty(clip);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Ýþlem tamamlandý. Deðiþen klip sayýsý: " + selectedClips.Length);
    }
}