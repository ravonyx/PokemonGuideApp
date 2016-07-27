using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public static class StringExtensions
{
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source.IndexOf(toCheck, comp) >= 0;
    }
}

public class PokemonScrollList : UIList<ScrollCellTest>
{
	public PokemonJSONLoader Loader;

	private PanelManager m_PanelManager;

	private bool Initialized;

    public Text namePokemon;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    public int offsetSearch;

    // Use this for initialization
    public void Launch()
	{
		m_PanelManager = GetComponent<PanelManager>();
		Initialized = false;
		if (Loader != null)
			Loader.Load();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!Initialized && Loader != null && Loader.PokemonList != null)
		{
			CreateList();
			Initialized = true;
		}
	}

	public void CreateList()
	{
		int cells = Loader.PokemonList.Count;

		if (cells > 0)
		{
			ClearAllCells();
			Refresh();
		}
	}

	#region implemented abstract members of UIList

	public override int NumberOfCells()
	{
		if (Loader.PokemonList == null)
			return 0;
		return Loader.PokemonList.Count;
	}

	public override void UpdateCell(int index, ScrollCellTest cell)
	{
		cell.Init(Loader.PokemonList[index], m_PanelManager);
	}

    public void Search()
    {
        string name = namePokemon.text;
        Debug.Log(name);
        RectTransform target;
        for (int i = 0; i < Loader.PokemonList.Count; i++)
        {
            string nameFullPokemon = Loader.PokemonList[i]["name-fr"];
            if (nameFullPokemon.Contains(name, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log(nameFullPokemon);
                target = cells[i].gameObject.GetComponent<RectTransform>();
                SnapTo(target);
                break;
            }
        }
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x,
            scrollRect.transform.InverseTransformPoint(contentPanel.position).y
            - scrollRect.transform.InverseTransformPoint(target.position).y);

        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanel.anchoredPosition.y + offsetSearch);
    }

    #endregion
}
