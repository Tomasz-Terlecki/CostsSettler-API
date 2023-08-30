﻿using System.Runtime.Serialization;

namespace CostsSettler.Auth.Exceptions;

/// <summary>
/// Exception that informs the generated access token is null.
/// </summary>
[Serializable]
public class NullAccessTokenException : Exception
{
    /// <summary>
    /// Creates new default NullAccessTokenException instance.
    /// </summary>
    public NullAccessTokenException()
        : base("The access token could not be generated by Keycloak server") { }

    protected NullAccessTokenException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
