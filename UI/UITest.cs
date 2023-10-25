using UnityEngine;
using UnityEngine.UIElements;

public class UITest : MonoBehaviour
{
    public Global house = new Global();

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button buttonStart = root.Q<Button>("Button");

        buttonStart.clicked += house.Test;
    }
}
