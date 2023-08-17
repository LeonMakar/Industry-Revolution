using System;
using System.Runtime.Serialization;

[Serializable]
internal class NullReferendeExeption : Exception
{
    public NullReferendeExeption()
    {
    }

    public NullReferendeExeption(string message) : base(message)
    {
    }

    public NullReferendeExeption(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NullReferendeExeption(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}