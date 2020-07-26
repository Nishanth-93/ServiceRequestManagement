using MediatR;
using System;
using System.Collections.Generic;

namespace ServiceRequestManagement.Domain.Seeds
{
    /// <summary>
    /// Abstract implementation of the Entity class, that all entities will derive from.
    /// </summary>
    public abstract class Entity
    {
        int? _hashCode;
        Guid _id;

        public virtual Guid Id { get { return _id; } protected set { _id = value; } }

        // Don't think I'll be using domain events in this project, but they will be here if I need it.
        private List<INotification> _domainEvents;

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Adds a domain event to the domain event list.
        /// </summary>
        /// <param name="evnt">The domain event to be added to the list.</param>
        public void AddDomainEvents(INotification evnt)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(evnt);
        }

        /// <summary>
        /// Removes a domain event from the domain event list.
        /// </summary>
        /// <param name="evnt">The domain event to remove from the list.</param>
        public void RemoveDomainEvent(INotification evnt)
        {
            _domainEvents?.Remove(evnt);
        }

        /// <summary>
        /// Clears the domain event list.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }


        /// <summary>
        /// Determines whether the entity is transient.
        /// </summary>
        /// <returns>Returns true if the entity is transient; otherwise, false.</returns>
        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_hashCode.HasValue)
                    _hashCode = Id.GetHashCode() ^ 31;

                return _hashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        /// <summary>
        /// Operator overload for equality.
        /// </summary>
        /// <param name="left">Entity on the left side of the the operator.</param>
        /// <param name="right">Entity on the right side of the operator.</param>
        /// <returns>Returns true if the entities are equal; otherwise, false.</returns>
        public static bool operator ==(Entity left, Entity right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }

        /// <summary>
        /// Operator overload for inequality.
        /// </summary>
        /// <param name="left">Entity on the left side of the the operator.</param>
        /// <param name="right">Entity on the right side of the operator.</param>
        /// <returns>Returns true if the entities are inequal; otherwise, false.</returns>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
