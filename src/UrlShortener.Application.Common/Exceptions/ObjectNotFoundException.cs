﻿namespace UrlShortener.Application.Common.Exceptions;


[Serializable]
public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException() { }
    public ObjectNotFoundException(string message) : base(message) { }
    public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected ObjectNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

