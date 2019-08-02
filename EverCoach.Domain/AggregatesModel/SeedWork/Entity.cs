using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverCoach.Domain.AggregatesModel.SeedWork
{
    public abstract class Entity
    {
        int? _requestedHashCode;
        int _Id;
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents ? .AsReadOnly();
    }
}
