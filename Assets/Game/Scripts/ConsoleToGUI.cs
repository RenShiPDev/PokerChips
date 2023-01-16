using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    // Adjust via the Inspector
    public int maxLines = 8;
    private Queue<string> queue = new Queue<string>();
    private string currentText = "";
    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        currentText = "";
        if (queue.Count >= maxLines) queue.Dequeue();
        if(logString.Length > 40)
            queue.Enqueue(logString.Substring(0,40));
        else
            queue.Enqueue(logString);
        
        foreach (string st in queue)
        {
            currentText += st + "\n";
        }
    }
    void OnGUI()
    {
        GUI.Label(
           new Rect(
               5,                   // x, left offset
               Screen.height - 150, // y, bottom offset
               300f,                // width
               300f                 // height
           ),
           currentText,             // the display text
           GUI.skin.textArea        // use a multi-line text area
        );
    }
}
