public class SelectedObjectSignal
{
    public SelectedObjectForBilding SelectedObject { get; private set; }
    public SelectedObjectSignal(SelectedObjectForBilding selectedObject)
    {
        SelectedObject = selectedObject;
    }
}