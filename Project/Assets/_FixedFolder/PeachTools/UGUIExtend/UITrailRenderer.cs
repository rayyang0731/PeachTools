using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrailRenderer : Graphic {
    /// <summary>
    /// 拖尾的生命周期
    /// </summary>
    public float lifeTime = 0.2f;
    /// <summary>
    /// 改变宽度的时间
    /// </summary>
    public float changeTime = 0.01f;
    /// <summary>
    /// 起始宽度
    /// </summary>
    public float widthStart = 50.0f;
    /// <summary>
    /// 结束宽度
    /// </summary>
    public float widthEnd = 0.0f;
    /// <summary>
    /// 距离中心点的最小距离
    /// </summary>
    public float vertexDistanceMin = 1f;
    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool pausing = false;
    /// <summary>
    /// 锚点
    /// </summary>
    public Vector3 anchor = new Vector3 (200, 200, 0);
    /// <summary>
    /// 目标物
    /// </summary>
    public RectTransform rt;

    /// <summary>
    /// 真正要使用的目标点位置
    /// </summary>
    /// <value></value>
    private Vector3 targetPoint {
        get {
            if (rt == null)
                return anchor;
            else
                return rt.position;
        }
    }
    /// <summary>
    /// 中心点
    /// </summary>
    private List<Vector3> centerPositions;
    /// <summary>
    /// 左边的顶点
    /// </summary>
    private List<Vertex> leftVertices;
    /// <summary>
    /// 右边的顶点
    /// </summary>
    private List<Vertex> rightVertices;

    protected override void Awake () {
        centerPositions = new List<Vector3> ();
        centerPositions.Add (targetPoint);

        leftVertices = new List<Vertex> ();
        rightVertices = new List<Vertex> ();

        SetPivot ();
    }

    protected override void Reset () {
        SetPivot ();
    }
    public void SetPivot () {
        rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = Vector2.zero;
    }

    protected override void OnPopulateMesh (VertexHelper toFill) {
        if (!pausing) {
            //如果添加或删除顶点，则设置网格并调整宽度
            if (TryAddVertices () | TryRemoveVertices ()) {
                if (widthStart != widthEnd)
                    SetVertexWidths ();
                SetMesh (toFill);
            }
        }
    }

    /// <summary>
    /// 如果对象从最近的中心位置移动了“vertexDistanceMin”以上，则添加新顶点。
    /// 如果添加了一对顶点，则此方法返回true。
    /// </summary>
    private bool TryAddVertices () {
        bool vertsAdded = false;

        if ((centerPositions[0] - targetPoint).sqrMagnitude > vertexDistanceMin * vertexDistanceMin) {
            Vector3 dirToCurrentPos = (targetPoint - centerPositions[0]).normalized;

            Vector3 cross = Vector3.Cross (Vector3.back, dirToCurrentPos);
            Vector3 leftPos = targetPoint + (cross * -widthStart * 0.5f);
            Vector3 rightPos = targetPoint + (cross * widthStart * 0.5f);

            leftVertices.Insert (0, new Vertex (leftPos, targetPoint, (leftPos - targetPoint).normalized));
            rightVertices.Insert (0, new Vertex (rightPos, targetPoint, (rightPos - targetPoint).normalized));

            centerPositions.Insert (0, targetPoint);
            vertsAdded = true;
        }

        return vertsAdded;
    }

    /// <summary>
    /// 移除已超过指定寿命的任何一对顶点。
    /// 如果已删除一对顶点，则此方法返回true。
    /// </summary>
    private bool TryRemoveVertices () {
        bool vertsRemoved = false;
        if (leftVertices.Count <= 0)
            return vertsRemoved;
        Vertex leftVertNode = leftVertices[leftVertices.Count - 1];

        while (leftVertNode != null && leftVertNode.TimeAlive > lifeTime && leftVertices.Count > 0) {

            leftVertices.RemoveAt (leftVertices.Count - 1);
            if (leftVertices.Count > 0) {
                leftVertNode = leftVertices[leftVertices.Count - 1];
                rightVertices.RemoveAt (rightVertices.Count - 1);
                centerPositions.RemoveAt (centerPositions.Count - 1);
                vertsRemoved = true;
            }
        }

        return vertsRemoved;
    }

    /// <summary>
    /// 根据生命周期计算宽度
    /// </summary>
    private void SetVertexWidths () {
        float widthDelta = widthStart - widthEnd;
        float timeDelta = lifeTime - changeTime;

        for (int i = 0; i < leftVertices.Count; i++) {
            Vertex leftVert = leftVertices[i];
            Vertex rightVert = rightVertices[i];

            if (leftVert.TimeAlive > changeTime) {
                float width = widthStart - (widthDelta * ((leftVert.TimeAlive - changeTime) / timeDelta));

                float halfWidth = width * 0.5f;

                leftVert.AdjustWidth (halfWidth);
                rightVert.AdjustWidth (halfWidth);
            }
        }
    }

    /// <summary>
    /// 设置网格
    /// </summary>
    private void SetMesh (VertexHelper vh) {
        if (centerPositions.Count < 2)
            return;

        vh.Clear ();

        int[] triangles = new int[(centerPositions.Count - 1) * 6];

        float timeDelta = leftVertices[leftVertices.Count - 1].TimeAlive - leftVertices[0].TimeAlive;

        for (int i = 0; i < leftVertices.Count; ++i) {
            Vertex leftVert = leftVertices[i];
            Vertex rightVert = rightVertices[i];

            int vertIndex = i * 2;
            float uvValue = leftVert.TimeAlive / timeDelta;

            vh.AddVert (leftVert.Position, color, new Vector2 (uvValue, 0));
            vh.AddVert (rightVert.Position, color, new Vector2 (uvValue, 1));

            if (i > 0) {
                int triIndex = (i - 1) * 6;
                vh.AddTriangle (vertIndex - 2, vertIndex - 1, vertIndex + 1);
                vh.AddTriangle (vertIndex - 2, vertIndex + 1, vertIndex);
            }
        }
    }

    protected virtual void Update () {
        if (!pausing)
            SetAllDirty ();
    }

    protected override void UpdateMaterial () {
        if (!IsActive ())
            return;
        canvasRenderer.materialCount = 1;
        canvasRenderer.SetMaterial (materialForRendering, 0);
        canvasRenderer.SetTexture (materialForRendering.mainTexture);
    }

    /// <summary>
    /// 顶点
    /// </summary>
    private class Vertex {
        /// <summary>
        /// 中心点
        /// </summary>
        private Vector3 centerPosition;
        /// <summary>
        /// 方向
        /// </summary>
        private Vector3 derivedDirection;
        /// <summary>
        /// 创建时间
        /// </summary>
        private float creationTime;
        /// <summary>
        /// 坐标位置
        /// </summary>
        /// <value></value>
        public Vector3 Position { get; private set; }
        /// <summary>
        /// 存活时间
        /// </summary>
        /// <value></value>
        public float TimeAlive { get { return Time.time - creationTime; } }
        /// <summary>
        /// 调整宽度
        /// </summary>
        /// <param name="width">宽度</param>
        public void AdjustWidth (float width) {
            Position = centerPosition + (derivedDirection * width);
        }
        /// <summary>
        /// 顶点
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="centerPosition">中心点</param>
        /// <param name="derivedDirection">方向</param>
        public Vertex (Vector3 position, Vector3 centerPosition, Vector3 derivedDirection) {
            this.Position = position;
            this.centerPosition = centerPosition;
            this.derivedDirection = derivedDirection;
            creationTime = Time.time;
        }
    }
}