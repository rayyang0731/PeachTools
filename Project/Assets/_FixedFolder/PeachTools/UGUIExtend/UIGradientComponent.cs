using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 渐变色图片
/// </summary>
[AddComponentMenu ("UI/Effects/Gradient")]
public class UIGradientComponent : BaseMeshEffect {
    /// <summary>
    /// 方向
    /// </summary>
    private enum DIRECTION {
        /// <summary>
        /// 水平方向
        /// </summary>
        HORIZONTAL,
        /// <summary>
        /// 垂直方向
        /// </summary>
        VERTICAL
    }
    /// <summary>
    /// 方向
    /// </summary>
    [SerializeField]
    private DIRECTION Direction = DIRECTION.HORIZONTAL;
    /// <summary>
    /// 左侧颜色
    /// </summary>
    [SerializeField]
    private Color32 StartColor = Color.white;
    /// <summary>
    /// 右侧颜色
    /// </summary>
    [SerializeField]
    private Color32 EndColor = Color.black;

    public override void ModifyMesh (VertexHelper vh) {
        if (!IsActive ()) {
            return;
        }

        var vertextList = new List<UIVertex> ();
        vh.GetUIVertexStream (vertextList);
        int count = vertextList.Count;

        ApplyGradient (vertextList, 0, count);
        vh.Clear ();
        vh.AddUIVertexTriangleStream (vertextList);

    }

    private void ApplyGradient (List<UIVertex> vertexList, int start, int end) {
        float startColor = 0;
        float endColor = 0;

        for (int i = 0; i < end; i++) {
            float val = Direction == DIRECTION.HORIZONTAL?
            vertexList[i].position.x:
                vertexList[i].position.y;

            if (val > endColor)
                endColor = val;
            else if (val < startColor)
                startColor = val;
        }

        float uiElementWeight = endColor - startColor;

        for (int i = 0; i < end; i++) {
            UIVertex uiVertex = vertexList[i];
            float val = Direction == DIRECTION.HORIZONTAL?
            uiVertex.position.x:
                uiVertex.position.y;
            uiVertex.color = Color32.Lerp (StartColor, EndColor, (val - startColor) / uiElementWeight);
            vertexList[i] = uiVertex;
        }
    }
}