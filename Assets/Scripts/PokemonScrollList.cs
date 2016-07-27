using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public static class StringExtensions
{
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source.IndexOf(toCheck, comp) >= 0;
    }
}

public class PokemonScrollList : MonoBehaviour
{
	public PokemonJSONLoader Loader;
	private PanelManager m_PanelManager;

	private bool Initialized;

    public InputField searchInput;
    private string nameSearch;
    public VerticalLayoutGroup grid;
    [SerializeField]
    private GameObject cellPrefab;

    public void Launch()
	{
		m_PanelManager = GetComponent<PanelManager>();
        nameSearch = "";
        Initialized = false;
        if (Loader != null)
            Loader.Load();
    }
	
	void Update ()
	{
        if (!Initialized && Loader != null && Loader.PokemonList != null)
		{
            int nbCells = Loader.PokemonList.Count;
            ClearList();

            GameObject cell;
            Debug.Log(nbCells);
            for (int i = 0; i < nbCells; i++)
            {
                if(nameSearch != "")
                {
                    string nameFullPokemon = Loader.PokemonList[i]["name-fr"];
                    if (nameFullPokemon.Contains(nameSearch, StringComparison.OrdinalIgnoreCase) && nameFullPokemon.IndexOf(nameSearch, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Debug.Log("name find");
                        cell = GameObject.Instantiate(cellPrefab) as GameObject;
                        cell.name = cell.name.Replace("(Clone)", "");
                        cell.transform.SetParent(grid.transform);
                        cell.GetComponent<ScrollCellTest>().Init(Loader.PokemonList[i], m_PanelManager);
                    }
                }
                else
                {
                    cell = GameObject.Instantiate(cellPrefab) as GameObject;
                    cell.name = cell.name.Replace("(Clone)", "");
                    cell.transform.SetParent(grid.transform);
                    cell.GetComponent<ScrollCellTest>().Init(Loader.PokemonList[i], m_PanelManager);
                }
            }
            Initialized = true;
		}
	}

    void ClearList()
    {
        Transform gridT = grid.transform;

        if (grid != null)
        {
            for (int i = 0; i < gridT.childCount; i++)
            {
                Destroy(grid.transform.GetChild(i).gameObject);
            }
        }
    }

	#region implemented abstract members of UIList

	public int NumberOfCells()
	{
		if (Loader.PokemonList == null)
			return 0;
		return Loader.PokemonList.Count;
	}

    public void Search()
    {
        nameSearch = searchInput.text;
        Debug.Log(nameSearch);
        Initialized = false;
    }

    #endregion
}
