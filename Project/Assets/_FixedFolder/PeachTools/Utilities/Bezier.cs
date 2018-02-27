using UnityEngine;

/// <summary>
/// 贝塞尔曲线
/// </summary>
[System.Serializable]
public class Bezier {
    public Vector3 p0, p1, p2, p3;

    private Vector3 b0 = Vector3.zero,
        b1 = Vector3.zero,
        b2 = Vector3.zero,
        b3 = Vector3.zero;

    private float Ax, Ay, Az;

    private float Bx, By, Bz;

    private float Cx, Cy, Cz;

    /// <summary>
    /// 初始化贝塞尔曲线
    /// </summary>
    /// <param name="v0">起点</param>
    /// <param name="v1">起点控制手柄控制点</param>
    /// <param name="v2">结束点控制手柄控制点</param>
    /// <param name="v3">结束点</param>
    public Bezier (Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3) {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
        this.p3 = v3;
    }

    /// <summary>
    /// 获得某个时间上的点坐标
    /// </summary>
    /// <param name="t">时间,区间[0,1]</param>
    /// <returns></returns>
    public Vector3 GetPointAtTime (float t) {
        this.CheckConstant ();

        float t2 = t * t;
        float t3 = t * t * t;

        float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;
        float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;
        float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + p0.z;

        return new Vector3 (x, y, z);
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="pointCount">路径点数量(包含起点和终点)</param>
    /// <returns></returns>
    public Vector3[] GetPath (int pointCount) {
        if (pointCount < 2)
            throw new System.Exception ("路径点数量不能低于2点");

        Vector3[] arr_v3 = new Vector3[pointCount];
        pointCount--;
        float detal = 1f / pointCount;
        for (int i = 0; i <= pointCount; i++) {
            arr_v3[i] = GetPointAtTime (detal * i);
        }
        return arr_v3;
    }

    /// <summary>
    /// 设置恒量
    /// </summary>
    private void SetConstant () {
        this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);
        this.Bx = 3f * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;
        this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;

        this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);
        this.By = 3f * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;
        this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;

        this.Cz = 3f * ((this.p0.z + this.p1.z) - this.p0.z);
        this.Bz = 3f * ((this.p3.z + this.p2.z) - (this.p0.z + this.p1.z)) - this.Cz;
        this.Az = this.p3.z - this.p0.z - this.Cz - this.Bz;
    }

    /// <summary>
    /// 检测恒量是否已经改变
    /// </summary>
    private void CheckConstant () {
        if (this.p0 != this.b0 ||
            this.p1 != this.b1 ||
            this.p2 != this.b2 ||
            this.p3 != this.b3) {
            this.SetConstant ();

            this.b0 = this.p0;
            this.b1 = this.p1;
            this.b2 = this.p2;
            this.b3 = this.p3;
        }
    }
}