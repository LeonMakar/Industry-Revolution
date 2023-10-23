using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField] private BilderSystem _bilderSystem;
    [SerializeField] private GameInputSystem _gameInputSystem;
    [SerializeField] private HouseDisplay _houseDisplay;
    [SerializeField] private CarAI _carAI;

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

        _injector.AddExistingSingletoneService<Injector, Injector>(_injector);
        _injector.AddExistingSingletoneService<GameInputSystem, GameInputSystem>(_gameInputSystem);
        _injector.AddExistingSingletoneService<GridSystem, GridSystem>(_grid);
        _injector.AddExistingSingletoneService<BilderSystem, BilderSystem>(_bilderSystem);
        _injector.AddExistingSingletoneService<HouseDisplay, HouseDisplay>(_houseDisplay);
        _injector.AddExistingSingletoneService<CarAI, CarAI>(_carAI);

        _injector.BuildSingletoneService<Factory, RoadFactory>();
        _injector.BuildSingletoneService<AStarSearch, AStarSearch>();
        _injector.BuildSingletoneService<RoadFixer, RoadFixer>();
        _injector.BuildSingletoneService<EventBus, EventBus>();
        _injector.BuildSingletoneService<Global, Global>();


        InitInjectableSingletoneServices();

    }
    public void InitInjectableSingletoneServices()
    {
        foreach (var service in _injector.SingletonServices)
        {
            if (service.Value is IInjectable)
            {
                var injectableObject = service.Value as IInjectable;
                injectableObject.Injecting();
            }
        }
    }
}

