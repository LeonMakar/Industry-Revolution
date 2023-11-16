using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineManager : MonoBehaviour, IService
{
    private List<Line> _lines = new List<Line>();
    public Line CurrentLine { get; private set; }
    public Line OldLine;

    public void AddLineToManager(Line line)
    {
        _lines.Add(line);
    }

    public void AddNewRootToCurrentLine(LineRootPoint root)
    {
        CurrentLine.LineRoots.Add(root);
    }

    public void DiactivateOldLineRoots(PointerEventData eventData)
    {
        if (OldLine != null && OldLine != CurrentLine)
        {
            foreach (var root in OldLine.LineRoots)
                root.gameObject.SetActive(false);
            OldLine.DiactivateLineVisualization();
            OldLine.CanChangeImageAlfaChanel = true;
            OldLine.IsLineActive = false;
            OldLine.OnPointerExit(eventData);
           
        }
    }
    public void ActivateCurrentLineRoots()
    {
        foreach (var root in CurrentLine.LineRoots)
        {
            root.gameObject.SetActive(true);
            CurrentLine.ActivateLineVisualization();
        }
    }
    public void SetCurrentLine(Line line)
    {
        CurrentLine = line;

    }


}
