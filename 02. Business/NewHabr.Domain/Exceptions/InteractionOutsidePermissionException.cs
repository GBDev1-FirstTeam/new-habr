namespace NewHabr.Domain.Exceptions;

public class InteractionOutsidePermissionException<TInteractionInitiator, TInteractionSubject> : Exception
{
    public InteractionOutsidePermissionException(string additionalSpecifics = null!)
        : base(string.Concat(
            $"Initiator {typeof(TInteractionInitiator).Name}, does not have rights to access with subject {typeof(TInteractionSubject).Name}.",
            additionalSpecifics is not null
                ? $"\n{additionalSpecifics}"
                : string.Empty))
    {
    }
}
