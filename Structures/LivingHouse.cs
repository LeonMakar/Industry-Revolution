using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingHouse : Structure
{
    public override void OnMouseDown()
    {
        if (_isPlaced && _global.CreateRootPointIsActive && Cursor.CursorIsEmpty)
        {
            _eventBus.Invoke(new StructureAddedForRootSignal(this));
        }
        if (_isPlaced && _global.CreateRootPointIsActive)
        {
        }
    }
}
