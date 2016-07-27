using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class ScrollCellTest : MonoBehaviour
{
    public Image image;

    public Text cellLabel;
	public Text typeLabel;
	public Text attackLabel;

    private JSONNode m_PokemonInfos;
	private PanelManager m_PanelManager;

	public JSONNode PokemonInfos { get { return m_PokemonInfos; } }

	public void Init(JSONNode pokemonInfos, PanelManager panelManager)
	{
        m_PokemonInfos = pokemonInfos;
		m_PanelManager = panelManager;
        StartCoroutine(LoadPokemonImg(m_PokemonInfos["img"]));
        cellLabel.text = m_PokemonInfos["name-fr"];
        typeLabel.text = m_PokemonInfos["type"];
        attackLabel.text = m_PokemonInfos["attack"];
    }

	public void OnButtonClick()
	{
		m_PanelManager.DisplayPokemonInfos(this);
	}

    private IEnumerator LoadPokemonImg(string filename)
    {
        WWW data;
        if (Application.platform == RuntimePlatform.Android)
            data = new WWW("jar:file://" + Application.dataPath + "!/assets/Img/" + filename);
        else
            data = new WWW("file:///" + Application.streamingAssetsPath + "/Img/" + filename);
        yield return data;

        if (string.IsNullOrEmpty(data.error))
        {
            image.sprite = Sprite.Create(data.texture, new Rect(0, 0, data.texture.width, data.texture.height), Vector2.zero);
        }
    }
}

