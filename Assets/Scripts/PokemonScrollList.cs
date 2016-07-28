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
    private bool sortAsc;

    public void Launch()
	{
		m_PanelManager = GetComponent<PanelManager>();
        nameSearch = "";
        Initialized = false;
        if (Loader != null)
            Loader.Load();
        sortAsc = true;
    }
	
	void Update ()
	{
        if (!Initialized && Loader != null && Loader.PokemonList != null)
		{
            int nbCells = Loader.PokemonList.Count;
            ClearList();

            GameObject cell;
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

    public void Sort()
    {
        sortAsc = !sortAsc;
        Quicksort(Loader.PokemonList, 0, Loader.PokemonList.Count - 1, "attack", sortAsc);
        Initialized = false;
    }

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

    private void Quicksort(SimpleJSON.JSONNode elements, int low, int high, string typeSort, bool asc)
    {
        int pivot_loc = 0;
        if (low < high)
        {
            pivot_loc = Partition(elements, low, high, typeSort, asc);
            Quicksort(elements, low, pivot_loc - 1, typeSort, asc);
            Quicksort(elements, pivot_loc + 1, high, typeSort, asc);
        }
    }

    private int Partition(SimpleJSON.JSONNode elements, int low, int high, string typeSort, bool asc)
    {
        int pivot = Int32.Parse(elements[high][typeSort]);

        while (low < high)
        {
            if(asc)
            {
                while (Int32.Parse(elements[low][typeSort]) < pivot)
                {
                    low++;
                }
                while (Int32.Parse(elements[high][typeSort]) > pivot)
                {
                    high--;
                }
            }
            else
            {
                while (Int32.Parse(elements[low][typeSort]) > pivot)
                {
                    low++;
                }
                while (Int32.Parse(elements[high][typeSort]) < pivot)
                {
                    high--;
                }
            }

            if (Int32.Parse(elements[low][typeSort]) == Int32.Parse(elements[high][typeSort]))
            {
                low++;
            }
            else if (low < high)
            {
                swap(elements, low, high);
            }
        }
        return high;
    }
    
    private void swap(SimpleJSON.JSONNode elements, int a, int b)
    {
        SimpleJSON.JSONNode temp = elements[a];
        elements[a] = elements[b];
        elements[b] = temp;
    }


    #endregion
}
