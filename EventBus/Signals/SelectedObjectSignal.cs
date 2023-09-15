public class SelectedObjectSignal
{
    public ObjectDataForBilding ObjectData { get; private set; }

    public SelectedObjectSignal(ObjectDataForBilding objectData)
    {
        ObjectData = objectData;
    }
}