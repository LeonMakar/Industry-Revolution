using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureAddedForRootSignal
{
    public StructureInformation StructureInformation {  get; private set; }
    public StructureAddedForRootSignal(StructureInformation info)
    {
       StructureInformation = info;
    }
}
