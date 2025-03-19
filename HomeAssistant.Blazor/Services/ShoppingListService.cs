using HomeAssistant.Blazor.Components.Pages.Kitchen;

namespace HomeAssistant.Blazor.Services
{
	public class ShoppingListService
	{
		private readonly FirestoreService _firestoreService;

		public ShoppingListService(FirestoreService firestoreService)
		{
			_firestoreService = firestoreService;
		}

		public async Task<List<ShoppingListItemModel>> GetShoppingListItemsAsync()
		{
			await _firestoreService.InitFirestore();

			return await _firestoreService.GetShoppingListItemsAsync();
		}

		public async Task SetShoppingListItemsAsync(List<ShoppingListItemModel> models)
		{
			await _firestoreService.SetShoppingListItemsAsync(models);
		}
	}
}
