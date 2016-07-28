using UnityEngine;
using System.Collections;
using SimpleJSON;

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

		for (int i = 0; i < pkmnList.Count; ++i)
		{
			int pkmnId = int.Parse(pkmnList[i]["pokemonId"].Value);
			if (pkmnId > 0 && pkmnId <= JsonLoader.PokemonList.Count)
				Debug.Log(JsonLoader.PokemonList[pkmnId]["name-fr"]);
			else
				Debug.LogWarning("INVALID POKEMON ID " + pkmnId);
		}
	}
}
