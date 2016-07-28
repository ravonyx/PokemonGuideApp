using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class LoaderNearbyPkmn : MonoBehaviour
{
	public string Latitude;
	public string Longitude;

	private PokemonJSONLoader JsonLoader;

	// Use this for initialization
	void Start()
	{
		JsonLoader = GetComponent<PokemonJSONLoader>();
		JsonLoader.PkmnLoadedHandler = OnPkmnLoaded;
		JsonLoader.Load();
	}

	void OnPkmnLoaded()
	{
		//if (JsonLoader.PokemonList != null)
		//{
		//	int pkmnCount = JsonLoader.PokemonList.Count;
		//	for (int i = 0; i < pkmnCount; ++i)
		//		Debug.Log(JsonLoader.PokemonList[i]["name-fr"]);
		//}
		StartCoroutine(LoadNearbyPkmn());
	}

	IEnumerator LoadNearbyPkmn()
	{
		string url = "https://pokevision.com/map/data/" + Latitude + "/" + Longitude;
		WWW www = new WWW(url);
		yield return www;
		OnNearbyPkmnLoaded(www.text);
	}

	void OnNearbyPkmnLoaded(string jsonNearbyPkmn)
	{
		var N = JSON.Parse(jsonNearbyPkmn);
		JSONNode pkmnList = N["pokemon"];

		var playerLat = double.Parse(Latitude);
		var playerLon = double.Parse(Longitude);

		TimeSpan currentTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));

		for (int i = 0; i < pkmnList.Count; ++i)
		{
			int pkmnId = int.Parse(pkmnList[i]["pokemonId"].Value) - 1;
			if (pkmnId >= 0 && pkmnId < JsonLoader.PokemonList.Count)
			{
				double pkmnLat = double.Parse(pkmnList[i]["latitude"].Value);
				double pkmnLon = double.Parse(pkmnList[i]["longitude"].Value);
				int distance = (int)(DistanceTo(playerLat, playerLon, pkmnLat, pkmnLon) * 1000d);
				//if (distance < 100)
				{
					TimeSpan despawnTime = new TimeSpan(0, 0, 0, int.Parse(pkmnList[i]["expiration_time"]));
					TimeSpan remainingTime = despawnTime.Subtract(currentTime);
					Debug.Log(	JsonLoader.PokemonList[pkmnId]["name-fr"] + " " + distance + " meters " + pkmnList[i]["uid"].Value +
								"(" + remainingTime.Minutes + ":" + remainingTime.Seconds + ")");
				}
			}
			else
				Debug.LogWarning("INVALID POKEMON ID " + pkmnId);
		}
	}

	public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
	{
		double rlat1 = Math.PI * lat1 / 180d;
		double rlat2 = Math.PI * lat2 / 180d;
		double theta = lon1 - lon2;
		double rtheta = Math.PI * theta / 180d;
		double dist =
			Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
			Math.Cos(rlat2) * Math.Cos(rtheta);
		dist = Math.Acos(dist);
		dist = dist * 180d / Math.PI;
		dist = dist * 60d * 1.1515d;

		switch (unit)
		{
			case 'K': //Kilometers -> default
				return dist * 1.609344d;
			case 'N': //Nautical Miles 
				return dist * 0.8684d;
			case 'M': //Miles
				return dist;
		}

		return dist;
	}
}
