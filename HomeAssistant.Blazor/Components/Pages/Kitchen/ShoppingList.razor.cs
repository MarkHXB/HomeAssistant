using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace HomeAssistant.Blazor.Components.Pages.Kitchen;
public partial class ShoppingList
{
	[CascadingParameter] public IModalService Modal { get; set; } = default!;

	private List<ShoppingListItemModel> ShoppingListItems = new List<ShoppingListItemModel>();
	private List<ShoppingListItemModel> CheckedItems = new List<ShoppingListItemModel>();

	protected override async Task OnInitializedAsync()
	{
		ShoppingListItems = await ShoppingListService.GetShoppingListItemsAsync();
	}

	private void OnItemCheckedChanged(ShoppingListItemModel item)
	{
		if(item is null)
		{
			return;
		}

		if (item.IsChecked)
		{
			ShoppingListItems.Remove(item);
			CheckedItems.Add(item);
		}
		else
		{
			CheckedItems.Remove(item);
			ShoppingListItems.Add(item);
		}
	}
}
