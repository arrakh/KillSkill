using UnityEditor;
using UnityEngine;

namespace FlipBooks.Editor
{
    [CustomEditor(typeof(FlipBookAsset))]
    public class FlipBookAssetEditor : UnityEditor.Editor
    {
        private FlipBookAsset flipBookAsset;
        private float animationTime;
        private double lastUpdateTime;
        private float playSpeed = 1f; // Play speed factor, similar to the in-game script
        private bool isPlaying = true;

        private void OnEnable()
        {
            flipBookAsset = (FlipBookAsset) target;
            EditorApplication.update += UpdateAnimation;
            lastUpdateTime = EditorApplication.timeSinceStartup;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateAnimation;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Sprite currentFrame = flipBookAsset.flipBook.GetFrame(animationTime);

            GUILayout.Label("Preview:", EditorStyles.boldLabel);
            Rect rect = GUILayoutUtility.GetRect(256, 256);
            if (currentFrame != null) {
                DrawSpriteWithFrame(rect, currentFrame);
            }

            // Replay button
            if (GUILayout.Button("Replay Animation")) {
                animationTime = 0f;
                isPlaying = true;
            }

            // Update the animation time based on a controlled frame rate
            if (!isPlaying) return;
            double currentTime = EditorApplication.timeSinceStartup;
            double deltaTime = currentTime - lastUpdateTime;
            animationTime += (float)(deltaTime * playSpeed); // Adjusted to include playSpeed
            lastUpdateTime = currentTime;

            Repaint();
        }

        private void UpdateAnimation()
        {
            if (!isPlaying) return;
            
            double currentTime = EditorApplication.timeSinceStartup;
            double deltaTime = currentTime - lastUpdateTime;
            animationTime += (float)(deltaTime * playSpeed); // Adjusted to include playSpeed
            lastUpdateTime = currentTime;

            Repaint();
        }

        private void DrawSpriteWithFrame(Rect position, Sprite sprite) {
            // Calculate aspect ratio and adjust position
            float aspectRatio = sprite.rect.width / sprite.rect.height;
            float height = position.width / aspectRatio;
            if (height > position.height) {
                float width = position.height * aspectRatio;
                position.x += (position.width - width) / 2;
                position.width = width;
            } else {
                position.y += (position.height - height) / 2;
                position.height = height;
            }

            // Draw the frame and sprite
            GUI.Box(new Rect(position.x - 5, position.y - 5, position.width + 10, position.height + 10), GUIContent.none, EditorStyles.helpBox);
            Rect spriteRect = sprite.rect;
            Texture2D texture = sprite.texture;
            Rect texCoords = new Rect(spriteRect.x / texture.width, spriteRect.y / texture.height, spriteRect.width / texture.width, spriteRect.height / texture.height);
            GUI.DrawTextureWithTexCoords(position, texture, texCoords, true);
        }
    }
}
