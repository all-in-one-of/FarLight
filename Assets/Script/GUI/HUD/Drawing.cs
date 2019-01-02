using System;
using UnityEngine;

namespace fl
{
    public class Drawing
    {
        public static Texture2D lineTex;

        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = GUI.matrix;
            if (!lineTex) { lineTex = new Texture2D(1, 1); }
            Color savedColor = GUI.color;
            GUI.color = color;
            float angle = Vector3.Angle(pointB - pointA, Vector2.right);
            if (pointA.y < pointB.y) { angle = -angle; }
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(angle, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }
    }
}