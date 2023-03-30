using UnityEngine;
using UnityEditor;


namespace FIMSpace.FSeamlessGenerator
{
    public class FChannelsSwapperWindow : FTextureProcessWindow
    {
        public static void Init()
        {
            FChannelsSwapperWindow window = (FChannelsSwapperWindow)EditorWindow.GetWindow(typeof(FChannelsSwapperWindow));
            window.titleContent = new GUIContent("Channels Swapper", "Extract single channels from texture file or inject channels");
            window.previewSize = 64;
            window.previewScale = FEPreview.m_1x1;
            window.drawPreviewScale = false;
            window.drawApplyButton = false;
            window.drawPreviewButton = false;
            window.Show();
        }

    }
}