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
		var i = Characters.IndexOf((CharacterSelection)sender);
		ConfirmedSelections.Remove(i);
		RefreshUses();
	}

	void Character_ConfirmSelection(object sender, CharacterSelection.ConfirmEventArgs e)
	{
		var i = Characters.IndexOf((CharacterSelection)sender);
		if (ConfirmedSelections.Any(inUse => inUse.Value == e.Character.Sprite && inUse.Key != i))
			e.Cancel();
		else
			ConfirmedSelections[i] = e.Character.Sprite;
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
		if (ConfirmedSelections.Count >= Characters.Count) {
			SceneManager.LoadScene("gameplay");
		}
	}
}
