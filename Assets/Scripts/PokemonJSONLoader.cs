using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Text;

public class PokemonJSONLoader : MonoBehaviour
{
	public string filename = "PokemonList.json";

	public JSONNode PokemonList;

	public delegate void PkmnLoaded();
	public PkmnLoaded PkmnLoadedHandler = null;

	void Start()
	{
		PokemonList = null;
	}

	public void Load()
	{
		PokemonList = null;
		StartCoroutine(LoadJSONFile());
	}

	private IEnumerator LoadJSONFile()
	{
		WWW data;
		if (Application.platform == RuntimePlatform.Android)
			data = new WWW("jar:file://" + Application.dataPath + "!/assets/" + filename);
		else
			data = new WWW("file:///" + Application.streamingAssetsPath + "/" + filename);
		yield return data;

		if (string.IsNullOrEmpty(data.error))
		{
			OnFileLoaded(Encoding.Default.GetString(data.bytes));
		}
	}

	private void OnFileLoaded(string data)
	{
		var N = JSON.Parse(data);
		PokemonList = N["data"]["pokemonList"];
		if (PkmnLoadedHandler != null)
			PkmnLoadedHandler();
	}
}
