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

        _container.Register<IService, RoadFixer>();
        _container.Register<IService, AStarSearch>();
        _container.Register<IMainService, EventBus>();
        _container.Register<Factory, RoadFactory>();
        _container.Register<Injector, Injector>();

        _injector.AddExistingSingletoneService<Injector>(_injector);
        _injector.AddExistingSingletoneService<GridSystem>(_grid);

        _injector.BuildSingletoneService<IService, RoadFixer>(typeof(RoadFactory));
        _injector.BuildSingletoneService<IMainService, EventBus>();
        _injector.BuildSingletoneService<IService, AStarSearch>();


        _injector.Injecting<BilderSystem>(_bilderSystem);
        _injector.Injecting<GameInputSystem>(_gameInputSystem);

    }


}
