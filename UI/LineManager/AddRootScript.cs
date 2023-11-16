using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AddRootScript : MonoBehaviour, IInjectable
{
    [SerializeField] private Volume _globalVolum;
    [SerializeField] private Global _global;
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private string _pathToRootPointPrefab;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private LineManager _lineManager;


    private ColorAdjustments _colorAdjustments;
    private bool _diactivationFlag = false;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(EventBus)] = typeof(EventBus),
        [typeof(Global)] = typeof(Global),
    };
    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(EventBus):
                    _eventBus = (EventBus)service;
                    break;
                case nameof(Global):
                    _global = (Global)service;
                    break;

            }
        }
    }
    private void OnEnable()
    {
        _eventBus.Subscrube<StructureAddedForRootSignal>(AddPoint);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<StructureAddedForRootSignal>(AddPoint);
    }

    public void ActivateRootEditor()
    {
        if (_colorAdjustments == null)
            _globalVolum.profile.TryGet(out _colorAdjustments);
        if (!_diactivationFlag && _lineManager.CurrentLine != null)
        {
            _colorAdjustments.active = true;
            _global.RootCreatedIsActive();
        }
    }

    public void AddPoint(StructureAddedForRootSignal structure)
    {
        if (_lineManager.CurrentLine != null)
        {
            CanvasFactory canvasFactory = new CanvasFactory();
            canvasFactory.PathForCanvasPrefab = _pathToRootPointPrefab;
            GameObject rootPoint = canvasFactory.Bild(BildingType.CanvasElement);
            rootPoint.transform.SetParent(_parentTransform);
            _lineManager.AddNewRootToCurrentLine(rootPoint.GetComponent<LineRootPoint>());
            rootPoint.TryGetComponent(out LineRootPoint root);
            root.ChangeRootPosition(structure.StructureInformation.CurrentPosition);
            _lineManager.CurrentLine.CreatPath();
        }


    }
    public void DeactivateRootEditor()
    {
        if (_diactivationFlag)
        {
            _colorAdjustments.active = false;
            _diactivationFlag = false;
            _global.RootCreatedIsUnActive();
            return;
        }
        _diactivationFlag = true;
    }


}
