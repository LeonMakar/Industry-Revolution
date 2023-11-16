using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Line : MonoBehaviour, IPointerClickHandler, IInjectable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image _image;
    [SerializeField] public LineVisualization _lineVisualization;

    public TextMeshProUGUI LineName;

    public List<LineRootPoint> LineRoots = new List<LineRootPoint>();
    private List<Mark> _marksToMoove = new List<Mark>();

    private LineManager _lineManager;

    private bool CanChangeImageAlfaChanel = true;

    private CarAI _carAI;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(LineManager)] = typeof(LineManager),
        [typeof(CarAI)] = typeof(CarAI),
    };

    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case (nameof(LineManager)):
                    _lineManager = (LineManager)service;
                    break;
                case (nameof(CarAI)):
                    _carAI = (CarAI)service;
                    break;
            }
        }
    }
    public void SetUnActiveRootPoints()
    {
        foreach (var root in LineRoots)
        {
            root.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _lineManager.SetCurrentLine(this);
        _lineManager.ActivateCurrentLineRoots();
        if (_lineManager.OldLine != null)
        {
            _lineManager.DiactivateOldLineRoots();
            _lineManager.OldLine.CanChangeImageAlfaChanel = true;
            _lineManager.OldLine.OnPointerExit(eventData);
        }
        _lineManager.OldLine = this;
        ChangeImageAlfaChanel(0.2f);
        CanChangeImageAlfaChanel = false;

    }
    public void ChangeImageAlfaChanel(float alfaValue)
    {
        Color color = _image.color;
        color.a = alfaValue;
        _image.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CanChangeImageAlfaChanel)
            ChangeImageAlfaChanel(0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CanChangeImageAlfaChanel)
            ChangeImageAlfaChanel(0);
    }

    public void CreatPath()
    {
        _lineVisualization.DeleteLine();
        _marksToMoove.Clear();
        if (LineRoots.Count > 1)
        {
            for (int i = 0; i < LineRoots.Count; i++)
            {
                Vector3Int nextPosition;
                Vector3Int currentPoint = LineRoots[i].RootPisition;
                if ((i + 1) < LineRoots.Count)
                {
                    nextPosition = LineRoots[i + 1].RootPisition;
                    _marksToMoove.AddRange(_carAI.CreatPathGridPoints(currentPoint, nextPosition));
                }

            }
            _marksToMoove.AddRange(_carAI.CreatPathGridPoints(LineRoots[LineRoots.Count - 1].RootPisition, LineRoots[0].RootPisition));
            _lineVisualization.DrowLine(_marksToMoove.Count, _marksToMoove);

        }
    }
}
