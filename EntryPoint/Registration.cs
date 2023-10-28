using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField] private BilderSystem _bilderSystem;
    [SerializeField] private GameInputSystem _gameInputSystem;
    //[SerializeField] private HouseDisplay _houseDisplay;
    [SerializeField] private CarAI _carAI;
    [SerializeField] private Cursor _cursor;

    private IContainer _container = new Container();
    private GridSystem _grid;

    private Injector _injector;

    private void Awake()
    {
        _grid = new GridSystem(50, 50);
        _injector = new Injector(_container);
        Injector.Instance = _injector;

        _container.Register<RoadFixer, RoadFixer>();
        _container.Register<AStarSearch, AStarSearch>();
        _container.Register<EventBus, EventBus>();
        _container.Register<Factory, RoadFactory>();
        _container.Register<Injector, Injector>();
        _container.Register<AStarSearchForCar, AStarSearchForCar>();
        _container.Register<Global, Global>();
        _container.Register<Factory, CanvasFactory>();

        _injector.AddExistingSingletoneService<Injector, Injector>(_injector);
        _injector.AddExistingSingletoneService<GameInputSystem, GameInputSystem>(_gameInputSystem);
        _injector.AddExistingSingletoneService<GridSystem, GridSystem>(_grid);
        _injector.AddExistingSingletoneService<BilderSystem, BilderSystem>(_bilderSystem);
        _injector.AddExistingSingletoneService<CarAI, CarAI>(_carAI);
        _injector.AddExistingSingletoneService<Cursor, Cursor>(_cursor);

        _injector.BuildSingletoneService<AStarSearch, AStarSearch>();
        _injector.BuildSingletoneService<RoadFixer, RoadFixer>();
        _injector.BuildSingletoneService<EventBus, EventBus>();
        _injector.BuildSingletoneService<Global, Global>();

        _injector.BuildRepeatedService<Factory, RoadFactory>();
        _injector.BuildRepeatedService<Factory, CanvasFactory>();

        InitInjectableSingletoneServices();

    }
    public void InitInjectableSingletoneServices()
    {
        foreach (var service in _injector.SingletonServices)
            if (service.Value is IInjectable injectableObject)
                injectableObject.Injecting();
    }
}

