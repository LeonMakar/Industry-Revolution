using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Injector : MonoBehaviour
{
    public static Injector Instance { get; private set; }

    [SerializeField] private BilderSystem _bilderSystem;
    [SerializeField] private GameInputSystem _gameInputSystem;

    private IContainer _container;
    private GridSystem _grid;
    private IMainService _eventBus;

    public void Awake()
    {
        Instance = this;

        _container = new Container();
        _grid = new GridSystem(50, 50);
    }

    public void Start()
    {
        _container.Register<IService, RoadFixer>();
        _container.Register<IMainService, EventBus>();
        _container.Register<IService, AStarSearch>();
        _container.Register<Factory, RoadFactory>();
        _eventBus = _container.Resolve<IMainService, EventBus>();

        _bilderSystem.Inject(_grid, _container.Resolve<IService, RoadFixer>(typeof(RoadFactory)), _eventBus);
        _gameInputSystem.Inject(_eventBus, _container.Resolve<IService, AStarSearch>());


    }


}
