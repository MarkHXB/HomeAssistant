namespace HomeAssistant.Blazor.Services
{
	internal class FirestoreActionsService
	{
		private readonly FirestoreService _firestoreService;

		public FirestoreActionsService(FirestoreService firestoreService)
		{
			_firestoreService = firestoreService;
		}

		
	}
}
