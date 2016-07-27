﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PokemonScrollList pokemonList;
    public GameObject startPanel;
    public GameObject listPokemonPanel;
    public GameObject typesPanel;

    void Start()
    {
        pokemonList = GetComponent<PokemonScrollList>();
    }

	public void ClickList()
    {
        startPanel.SetActive(false);
        listPokemonPanel.SetActive(true);
        pokemonList.Launch();
        pokemonList.enabled = true;
    }
    public void ClickTypes()
    {
        startPanel.SetActive(false);
        typesPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(listPokemonPanel.activeSelf == true)
            {
                startPanel.SetActive(true);
                listPokemonPanel.SetActive(false);
                pokemonList.enabled = false;
            }
            if (typesPanel.activeSelf == true)
            {
                startPanel.SetActive(true);
                typesPanel.SetActive(false);
            }
            return;
        }
    }
}
