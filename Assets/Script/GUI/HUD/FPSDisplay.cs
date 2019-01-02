using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public float deltaMeasurement = 1f;
    private float pastTense;
    private int m_frameCount;
    private float m_fps;

    private void Start()
    {
        pastTense = 0.0f;
    }

    private void Update()
    {
        ++m_frameCount;
        pastTense += Time.deltaTime;
        if (pastTense <= deltaMeasurement)
            return;
        m_fps = (float)m_frameCount / pastTense;
        pastTense = 0.0f;
        m_frameCount = 0;
    }

    private void OnGUI()
    {
        GUI.color = new Color(0.29f, 0.4f, 0.2f);
        string text = string.Format("{0:0.} fps", m_fps);
        GUILayout.Label(text);
    }
}
