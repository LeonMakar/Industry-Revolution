using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureAddedForRootSignal
{
    public Structure StructureInformation {  get; private set; }
    public StructureAddedForRootSignal(Structure info)
    {
       StructureInformation = info;
    }
}
