using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Vehicle vihacle);
        if (vihacle != null)
        {
            Debug.Log("������� ���������");
            Debug.Log(vihacle.GetType()); 
        }
        else
        {
            Debug.Log("��������� �� �������");

        }
    }
}
