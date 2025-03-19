using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using HomeAssistant.Blazor.Components.Pages.Kitchen;

public class FirestoreService
{
	private FirestoreDb _firestoreDb;

	public async Task<FirestoreDb> InitFirestore()
	{
		try
		{
			var stream = await FileSystem.OpenAppPackageFileAsync("firestore.json");
			var reader = new StreamReader(stream);
			var contents = reader.ReadToEnd();

			FirestoreClientBuilder fbc = new FirestoreClientBuilder { JsonCredentials = contents };
			return _firestoreDb = FirestoreDb.Create("homeassistant-1ae72", fbc.Build());
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<List<ShoppingListItemModel>> GetShoppingListItemsAsync()
	{
		var items = new List<ShoppingListItemModel>();
		var snapshot = await _firestoreDb.Collection("shoppinglist").GetSnapshotAsync();

		if(snapshot == null || !snapshot.Documents.Any())
		{
			return items;
		}

		foreach (var document in snapshot.Documents)
		{
			if (document.Exists)
			{
				var item = document.ConvertTo<ShoppingListItemModel>();
				items.Add(item);
			}
		}

		return items;
	}

	public async Task SetShoppingListItemsAsync(List<ShoppingListItemModel> items)
	{
		var collectionRef = _firestoreDb.Collection("shoppinglist");

		foreach (var item in items)
		{
			var docRef = collectionRef.Document(item.Name); // Assuming Name is unique
			await docRef.SetAsync(item);
		}
	}

	public async Task ReportTravelToDatabase(Location a, Location b, double travelTime)
	{
		var docRef = _firestoreDb.Collection("locations").Document(Guid.NewGuid().ToString());
		var locationData = new
		{
			StartPosLat = a.Latitude,
			StartPosLon = a.Longitude,
			EndPosLat = b.Latitude,
			EndPosLon = b.Longitude,
			TravelTime = travelTime,
			Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
		};

		await docRef.SetAsync(locationData);
	}
}
