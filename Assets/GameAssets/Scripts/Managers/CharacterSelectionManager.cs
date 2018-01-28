using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour {

	public List<CharacterSelection> Characters;
	Dictionary<int, Sprite> ConfirmedSelections = new Dictionary<int, Sprite>();

	// Use this for initialization
	void Start () {
		foreach (var c in Characters) {
			c.Confirm += Character_ConfirmSelection;
			c.Cancel += Character_CancelSelection;
		}
	}

	void Character_CancelSelection(object sender, EventArgs e)
	{
		var ch = (CharacterSelection)sender;
		var i = Characters.IndexOf(ch);
		ConfirmedSelections.Remove(i);
		GameManager.Instance.SetPlayerEnabled(ch.PlayerIndex, false);
		RefreshUses();
	}

	void Character_ConfirmSelection(object sender, CharacterSelection.ConfirmEventArgs e)
	{
		var ch = (CharacterSelection)sender;
		var i = Characters.IndexOf(ch);
		if (ConfirmedSelections.Any(inUse => inUse.Value == e.Character.Sprite && inUse.Key != i))
			e.Cancel();
		else {
			ConfirmedSelections[i] = e.Character.Sprite;
			GameManager.Instance.SetPlayerPrefab(ch.PlayerIndex, e.Character.GameplaySpritePrefab);
			GameManager.Instance.SetPlayerEnabled(ch.PlayerIndex, true);
		}
		RefreshUses();
	}

	void RefreshUses()
	{
		for (int i = 0; i < Characters.Count; i++) {
			var ch = Characters[i];
			foreach (var op in ch.AvailableSprites) {
				op.InUse = ConfirmedSelections.Any(inUse => inUse.Value == op.Sprite && inUse.Key != i);
			}
			ch.RefreshState();
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Return)) {
			foreach (var ch in Characters) {
				ConfirmedSelections[ch.PlayerIndex] = ch.CurrentSprite;
				SceneManager.LoadScene("gameplay");
			}
		}
		if ((Input.GetKey(KeyCode.Return) && ConfirmedSelections.Any()) || ConfirmedSelections.Count >= Characters.Count) {
			SceneManager.LoadScene("gameplay");
		}
	}
}
