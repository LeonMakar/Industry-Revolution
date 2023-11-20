using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Miner : Structure
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

    public override bool CheckThatNodeIsFree(int positionX, int positionY)
    {
        if (_gridSystem[positionX, positionY].TypeOfNode == NodeType.CoalMine)
            return true;
        else
            return false;
    }
}
