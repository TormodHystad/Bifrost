using Bifrost.Events;
using Machine.Specifications;
using Moq;

namespace Bifrost.Specs.Events.for_EventRepository.given
{
    public class an_event_repository_with_persisted_events : a_persisted_stream_of_20_events_belonging_to_2_different_aggregate_roots
    {
        protected static EventRepository event_repository;
    	protected static Mock<IEventConverter> event_converter_mock;
        

    	Establish context = () =>
    	                    	{
									event_converter_mock = new Mock<IEventConverter>();
    	                    		event_repository = new EventRepository(entity_context_mock.Object, event_converter_mock.Object, event_migragtion_hierarchy_manager_mock.Object);
    	                    	};
    }
}