using Google.Cloud.Firestore;

[FirestoreData]
public class LocationData
{
	[FirestoreProperty]
	public double Latitude { get; set; }

	[FirestoreProperty]
	public double Longitude { get; set; }

	[FirestoreProperty]
	public string Type { get; set; } // "start" vagy "end"

	[FirestoreProperty]
	public Timestamp Timestamp { get; set; }
}
