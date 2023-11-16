using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DiactivateOldLineRoots()
    {
        foreach (var root in OldLine.LineRoots)
        { 
            root.gameObject.SetActive(false);
        }
    }
    public void ActivateCurrentLineRoots()
    {
        foreach (var root in CurrentLine.LineRoots)
        {
            root.gameObject.SetActive(true);
        }
    }
    public void SetCurrentLine(Line line)
    {
        CurrentLine = line;
        foreach (var item in _lines)
        {
            if (line == item)
            {

            }
        }
    }


}
