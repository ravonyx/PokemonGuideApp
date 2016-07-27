using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour
{
	public GameObject m_ScrollGrid;
	public InfoPanel m_InfoPanel;

	public void DisplayPokemonInfos(ScrollCellTest cell)
	{
		StartCoroutine(LoadPokemonImage(cell.PokemonInfos["img"]));
		m_InfoPanel.Name.text = cell.PokemonInfos["name-fr"];
		m_InfoPanel.Food.text = cell.PokemonInfos["type"];
		m_InfoPanel.gameObject.SetActive(true);
		m_ScrollGrid.SetActive(false);
	}

	private IEnumerator LoadPokemonImage(string filename)
	{
		WWW data;
		if (Application.platform == RuntimePlatform.Android)
			data = new WWW("jar:file://" + Application.dataPath + "!/assets/Img/" + filename);
		else
			data = new WWW("file:///" + Application.streamingAssetsPath + "/Img/" + filename);
		yield return data;

		if (string.IsNullOrEmpty(data.error))
		{
			m_InfoPanel.Img.sprite = Sprite.Create(data.texture, new Rect(0, 0, data.texture.width, data.texture.height), Vector2.zero);
		}
	}

	public void OnInfoPanelBack()
	{
		m_InfoPanel.gameObject.SetActive(false);
		m_ScrollGrid.SetActive(true);
	}
}
