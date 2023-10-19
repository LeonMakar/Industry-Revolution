using Unity.VisualScripting;
using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField] private BilderSystem _bilderSystem;
    [SerializeField] private GameInputSystem _gameInputSystem;

    private IContainer _container = new Container();
    private GridSystem _grid;

    private Injector _injector;

    private void Awake()
    {
        _grid = new GridSystem(50, 50);
        _injector = new Injector(_container);

        _container.Register<RoadFixer, RoadFixer>();
        _container.Register<AStarSearch, AStarSearch>();
        _container.Register<EventBus, EventBus>();
        _container.Register<Factory, RoadFactory>();
        _container.Register<Injector, Injector>();
        _container.Register<AStarSearchForCar, AStarSearchForCar>();
        _container.Register<CarAI, CarAI>();

        _injector.AddExistingSingletoneService<Injector, Injector>(_injector);
        _injector.AddExistingSingletoneService<GridSystem, GridSystem>(_grid);
        _injector.AddExistingSingletoneService<BilderSystem, BilderSystem>(_bilderSystem);

        _injector.BuildSingletoneService<Factory, RoadFactory>();
        _injector.BuildSingletoneService<AStarSearch, AStarSearch>();
        _injector.BuildSingletoneService<RoadFixer, RoadFixer>();
        _injector.BuildSingletoneService<EventBus, EventBus>();
        //_injector.BuildSingletoneService<AStarSearchForCar, AStarSearchForCar>();
        _injector.BuildSingletoneService<CarAI, CarAI>();


        _injector.InjectingSingletoneServices<BilderSystem>(_bilderSystem);
        _injector.InjectingSingletoneServices<RoadFixer>(_injector.GetSingletoneService<RoadFixer, RoadFixer>());
        _injector.InjectingSingletoneServices<GameInputSystem>(_gameInputSystem);
        _injector.GetSingletoneService<CarAI, CarAI>().Injecting(_injector);

    }

}
