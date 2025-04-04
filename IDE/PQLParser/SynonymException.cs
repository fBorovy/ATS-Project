using System;

namespace IDE.PQLParser;

internal class SynonymException : Exception
{
    public SynonymException(string? message) : base(message)
    {
    }

}