using System.ComponentModel;

namespace HomeAssistant.Blazor.Components.Pages.Kitchen
{
	public class ShoppingListItemModel
	{
		public string Name { get; set; }
		public double Quantity { get; set; }
		public bool IsChecked { get; set; }
		public DateTime BoughtOn { get; set; }
	}
}
