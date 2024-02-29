using UnityEngine;
using UnityEditor;
using System.Linq;

namespace FlipBooks.Editor
{
    public class CreateFlipBookAssetFromSprites
    {
        private const float DefaultFrameRate = 24f;

        [MenuItem("Assets/Generate FlipBookAsset", true)]
        private static bool CreateFlipBookAssetValidation() {
            return Selection.objects.OfType<Sprite>().Any();
        }

        [MenuItem("Assets/Generate FlipBookAsset")]
        private static void CreateFlipBookAsset() {
            var selectedSprites = Selection.objects.OfType<Sprite>().ToArray();
            if (selectedSprites.Length == 0) {
                EditorUtility.DisplayDialog("No Sprites Selected", "Please select some sprites to create a FlipBookAsset.", "OK");
                return;
            }

            FlipBookAsset flipBookAsset = ScriptableObject.CreateInstance<FlipBookAsset>();
            flipBookAsset.flipBook = new FlipBook("NULL_ID", DefaultFrameRate, true, selectedSprites);

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) {
                path = "Assets";
            } else if (System.IO.Path.GetExtension(path) != "") {
                path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New FlipBookAsset.asset");
            AssetDatabase.CreateAsset(flipBookAsset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = flipBookAsset;
        }
    }
}