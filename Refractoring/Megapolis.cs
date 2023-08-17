using UnityEngine;

public abstract class Megapolis
{
    public int Citizens => _citizens;

    private int _citizens;


    public Megapolis(int citizens)
    {
        _citizens = citizens;
    }


}
