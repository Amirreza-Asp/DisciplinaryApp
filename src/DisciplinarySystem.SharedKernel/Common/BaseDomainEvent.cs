using MediatR;

namespace DisciplinarySystem.SharedKernel.Common
{
    public class BaseDomainEvent : IRequest
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
