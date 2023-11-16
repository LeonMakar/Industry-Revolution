using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineTextEditor : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] TMP_Text _text;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _parentGameObject;

    private string _lineName;

    private void OnEnable() => _inputField.onValueChanged.AddListener(ChangeLineName);
    public void DestroyLine() => Destroy(_parentGameObject);

    public void ChangeLineName(string lineName)
    {
        _lineName = lineName;
        _text.text = _lineName;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _inputField.gameObject.SetActive(false);
        _text.enabled = true;
    }
}
