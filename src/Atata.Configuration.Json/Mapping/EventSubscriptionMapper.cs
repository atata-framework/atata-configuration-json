using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atata.Configuration.Json;

public sealed class EventSubscriptionMapper
{
    private readonly Assembly[] _assembliesToFindEventTypes;

    private readonly Assembly[] _assembliesToFindEventHandlerTypes;

    private readonly IObjectCreator _objectCreator;

    public EventSubscriptionMapper(
        string assemblyNamePatternToFindEventTypes,
        string assemblyNamePatternToFindEventHandlerTypes,
        string defaultAssemblyNamePatternToFindTypes)
    {
        _assembliesToFindEventTypes = AssemblyFinder.FindAllByPattern(assemblyNamePatternToFindEventTypes);
        _assembliesToFindEventHandlerTypes = AssemblyFinder.FindAllByPattern(assemblyNamePatternToFindEventHandlerTypes);

        IObjectConverter objectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = defaultAssemblyNamePatternToFindTypes
        };
        IObjectMapper objectMapper = new ObjectMapper(objectConverter);
        _objectCreator = new ObjectCreator(objectConverter, objectMapper);
    }

    public EventSubscriptionItem Map(EventSubscriptionJsonSection section)
    {
        if (string.IsNullOrEmpty(section.HandlerType))
            throw new ConfigurationException(
                $"\"{nameof(EventSubscriptionJsonSection.HandlerType)}\" configuration property of event subscription section is not specified.");

        Type handlerType = TypeFinder.FindInAssemblies(section.HandlerType, _assembliesToFindEventHandlerTypes);

        if (string.IsNullOrEmpty(section.EventType))
        {
            return CreateSubscription(handlerType, section.ExtraPropertiesMap);
        }
        else
        {
            Type eventType = TypeFinder.FindInAssemblies(section.EventType, _assembliesToFindEventTypes);
            return CreateSubscription(eventType, handlerType, section.ExtraPropertiesMap);
        }
    }

    private EventSubscriptionItem CreateSubscription(Type eventType, Type eventHandlerType, Dictionary<string, object> eventHandlerValuesMap)
    {
        Type expectedType = typeof(IEventHandler<>).MakeGenericType(eventType);

        if (!expectedType.IsAssignableFrom(eventHandlerType))
            throw new ConfigurationException(
                $"\"{nameof(EventSubscriptionJsonSection.HandlerType)}\" configuration property of {eventHandlerType.FullName} type doesn't implement {expectedType.FullName}.");

        object eventHandler = _objectCreator.Create(eventHandlerType, eventHandlerValuesMap);

        return new EventSubscriptionItem(eventType, eventHandler);
    }

    private EventSubscriptionItem CreateSubscription(Type eventHandlerType, Dictionary<string, object> eventHandlerValuesMap)
    {
        Type expectedInterfaceType = typeof(IEventHandler<>);

        Type eventHanderType = eventHandlerType.GetGenericInterfaceType(expectedInterfaceType)
            ?? throw new ConfigurationException(
                $"\"{nameof(EventSubscriptionJsonSection.HandlerType)}\" configuration property of {eventHandlerType.FullName} type doesn't implement {expectedInterfaceType.FullName}.");

        Type eventType = eventHanderType.GetGenericArguments()[0];

        object eventHandler = _objectCreator.Create(eventHandlerType, eventHandlerValuesMap);

        return new EventSubscriptionItem(eventType, eventHandler);
    }
}
