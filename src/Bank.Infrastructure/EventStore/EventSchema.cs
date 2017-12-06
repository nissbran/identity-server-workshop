namespace Bank.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Domain;

    public abstract class EventSchema<TBaseEvent> : IEventSchema where TBaseEvent : IDomainEvent
    {
        private readonly Dictionary<EventDefinition, Type> _definitionToType = new Dictionary<EventDefinition, Type>();
        private readonly Dictionary<Type, EventDefinition> _typeToDefinition = new Dictionary<Type, EventDefinition>();

        public abstract string Name { get; }

        protected EventSchema()
        {
            var baseEvent = typeof(TBaseEvent);
            var types = baseEvent.GetTypeInfo().Assembly.GetTypes()
                .Where(p => baseEvent.IsAssignableFrom(p));

            foreach (var type in types)
            {
                if (type.GetTypeInfo().GetCustomAttribute(typeof(EventNameAttribute)) is EventNameAttribute eventName)
                {
                    var eventDefinition = new EventDefinition(eventName.Name);
                    _definitionToType.Add(eventDefinition, type);
                    _typeToDefinition.Add(type, eventDefinition);
                }
            }
        }

        public Type GetDomainEventType(string eventType)
        {
            var eventDefinition = new EventDefinition(eventType);

            if (_definitionToType.TryGetValue(eventDefinition, out var domainEvent))
                return domainEvent;

            throw new NotImplementedException();
        }

        public EventDefinition GetEventDefinition(IDomainEvent domainEvent)
        {
            if (_typeToDefinition.TryGetValue(domainEvent.GetType(), out var eventDefinition))
                return eventDefinition;

            throw new NotImplementedException();
        }
    }
}