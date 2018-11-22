using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.EventBus
{
	public interface IEventConsumersContainer
	{
		void RegisterConsumer<T>(IEventConsumer<T> consumer) where T : EventInfoBase;
		void StartListening();
		void StopListening();
	}
}