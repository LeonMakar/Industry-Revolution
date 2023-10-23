using UnityEngine;

public abstract class Factory : IService
{
    public abstract GameObject Bild(BildingType bildingType);

}


