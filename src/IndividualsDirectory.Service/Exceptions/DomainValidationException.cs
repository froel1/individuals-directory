namespace IndividualsDirectory.Service.Exceptions;

public class DomainValidationException(string messageKey, params object[] args) : Exception(messageKey)
{
    public string MessageKey { get; } = messageKey;
    public object[] Args { get; } = args;
}