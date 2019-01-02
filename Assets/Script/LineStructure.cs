using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineStructure : MonoBehaviour
{
    public float width = 1f;
    public int radius = 20;
    public int countPoint = 20;

    public enum StructureFigure { Line, Polygon};
    public StructureFigure structureFigure = StructureFigure.Polygon;

    public Transform LineStructureChild;

    private LineRenderer lineRenderer;

	void Start ()
    {
        lineRenderer = LineStructureChild.GetComponent<LineRenderer>();
        lineRenderer.positionCount = countPoint + 2;
        lineRenderer.widthCurve = AnimationCurve.Constant(0f, 1f, width);
        switch (structureFigure)
        {
            case StructureFigure.Line:
                break;
            case StructureFigure.Polygon:
                BuildPolygon();
                break;
        }

    }

    private void BuildPolygon()
    {
        float angle = 0f;
        float interval = 360 / countPoint;
        int currentPointNumber = 0;
        while (currentPointNumber <= countPoint)
        {
            Vector3 positionPoint = CirclePolygonPoint(radius, angle);
            lineRenderer.SetPosition(currentPointNumber, positionPoint);
            currentPointNumber++;
            if (angle < 360)
            {
                angle += interval;
            }
            else
            {
                angle = 0;
            }
        }
        lineRenderer.SetPosition(countPoint + 1, lineRenderer.GetPosition(0));
    }

    private Vector2 CirclePolygonPoint(float radius, float angle)
    {
        float x = (float)(radius * Mathf.Cos(angle * Mathf.PI / 180f));
        float y = (float)(radius * Mathf.Sin(angle * Mathf.PI / 180f));

        return new Vector2(x, y);
    }
}
