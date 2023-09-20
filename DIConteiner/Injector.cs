using UnityEngine;

public class Injector : MonoBehaviour
{
    public static Injector Instance { get; private set; }

    [SerializeField] private BilderSystem _bilderSystem;
    [SerializeField] private GameInputSystem _gameInputSystem;

    private DIContainer _container;
    private GridSystem _grid;

    public void Awake()
    {
        Instance = this;
        _container = new DIContainer();
        _grid = new GridSystem(10, 10);

        _container.RegisterService(new RoadFixer());
        _container.RegisterService(new EventBus());
        _container.RegisterService(new AStarSearch());
    }

    public void Start()
    {
        _bilderSystem.Inject<IMainService, IService>(_grid, _container.GetService<RoadFixer>(), _container.GetService<EventBus>());
        _gameInputSystem.Inject<IMainService, IService>(_container.GetService<EventBus>(), _container.GetService<AStarSearch>());
    }

}
