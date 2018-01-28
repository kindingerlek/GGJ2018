using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class CharacterSelection : MonoBehaviour {

	[System.Serializable]
	public class CharacterConfig {
		public Sprite Sprite;
		public string Name;
		public GameObject GameplaySpritePrefab;
		[System.NonSerialized] public bool InUse;
	}

	public class ConfirmEventArgs : System.EventArgs
	{
		public readonly CharacterConfig Character;

		public ConfirmEventArgs(CharacterConfig character)
		{
			Character = character;
		}

		bool cancel;
		public bool IsCanceled { get { return cancel; } }

		public void Cancel() { cancel = true; }
	}

	public event System.EventHandler<ConfirmEventArgs> Confirm;
	public event System.EventHandler Cancel;

	public CharacterConfig[] AvailableSprites;
	[SerializeField] int SelectedIndex;
	[SerializeField] Image TempImage1;
	[SerializeField] Image TempImage2;
	[SerializeField] Text NameText;
	[SerializeField] Image NamePanel;
	[SerializeField] float animationDuration;

	PlayerInput playerInput;
	bool animating;
	bool confirmed;

	bool temp1InUse = true;

	public int PlayerIndex { get { return playerInput.myPlayerIndex; } }
	public CharacterConfig CurrentSprite { get { return AvailableSprites[SelectedIndex]; } }

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		if (SelectedIndex < 0) {
			SelectedIndex = (playerInput.myPlayerIndex - 1) % AvailableSprites.Length;
		}
		if (AvailableSprites.Length > 0) {
			var ch = AvailableSprites[SelectedIndex % AvailableSprites.Length];
			CurrentImage.sprite = ch.Sprite;
			NameText.text = ch.Name;
		}
		CurrentImage.enabled = true;
		TempImage.enabled = true;
	}

	public void RefreshState()
	{
		if (AvailableSprites[SelectedIndex].InUse) {
			CurrentImage.color = new Color(0.5f, 0.5f, 0.5f);
		}
		else {
			CurrentImage.color = Color.white;
		}
		TempImage.color = Color.white;
	}

	void RefreshColor()
	{
		NamePanel.color = GameManager.Instance.GetPlayerColor(playerInput.myPlayerIndex);
		NamePanel.color *= new Color(1, 1, 1, confirmed? 0.6f : 0.2f);
	}

	/// <summary>
	/// Changes to next character selection.
	/// </summary>
	public bool Change(bool next)
	{
		if (animating || confirmed)
			return false;

		var nextIndex = (SelectedIndex + (next? 1 : AvailableSprites.Length - 1)) % AvailableSprites.Length;
		var ch = AvailableSprites[nextIndex];
		TempImage.sprite = ch.Sprite;
		NameText.text = ch.Name;
		SelectedIndex = nextIndex;
		RefreshState();

		StartCoroutine(RunAnimation(next? 1 : -1));
		animating = true;
		return true;
	}

	public bool ConfirmSelection()
	{
		if (confirmed || Confirm == null)
			return false;

		var e = new ConfirmEventArgs(AvailableSprites[SelectedIndex]);
		Confirm(this, e);
		if (!e.IsCanceled) {
			confirmed = true;
			RefreshColor();
			return true;
		}
		return false;
	}

	public bool CancelSelection()
	{
		if (!confirmed || Cancel == null)
			return false;

		Cancel(this, System.EventArgs.Empty);
		confirmed = false;
		RefreshColor();
		return true;
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return null;
		RefreshColor();
		TempImage.rectTransform.position = GetTempImageStartPosition(1);
	}

	Image CurrentImage {
		get {
			if (temp1InUse)
				return TempImage1;
			return TempImage2;
		}
	}

	Image TempImage {
		get {
			if (temp1InUse)
				return TempImage2;
			return TempImage1;
		}
	}

	Vector3 GetTempImageStartPosition(float direction)
	{
		return CurrentImage.transform.position + new Vector3(direction * CurrentImage.rectTransform.rect.width, 0, 0);
	}

	Vector3 GetImageEndPosition(float direction)
	{
		return CurrentImage.transform.position + new Vector3(-direction * CurrentImage.rectTransform.rect.width, 0, 0);
	}

	IEnumerator RunAnimation(float direction)
	{
		var currentImage = this.CurrentImage;
		var tempImage = this.TempImage;

		var baseCurrent = currentImage.transform.position;
		var baseTemp = GetTempImageStartPosition(direction);
		var targetCurrent = GetImageEndPosition(direction);

		temp1InUse = !temp1InUse;

		var moveCurrent = targetCurrent - baseCurrent;
		var moveTemp = baseCurrent - baseTemp;

		yield return Interpolate(animationDuration, v => {
			currentImage.rectTransform.position = baseCurrent + moveCurrent * v;
			tempImage.rectTransform.position = baseTemp + moveTemp * v;
		});

		animating = false;
	}

	IEnumerator Interpolate(float duration, System.Action<float> callback)
	{
		for (float i = 0; i < 1; i+= Time.deltaTime / duration) {
			callback(i);
			yield return null;
		}
		callback(1);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (playerInput.attack) {
			ConfirmSelection();
		}
		else if (playerInput.defence) {
			CancelSelection();
		}
		else if (playerInput.isAxisOn) {
			Change(playerInput.axisHorizontal > 0);
		}

		if (playerInput.myPlayerIndex == 1) {
			if (Input.GetKey(KeyCode.Return)) {
				ConfirmSelection();
			}
			else if (Input.GetKey(KeyCode.Escape)) {
				CancelSelection();
			}
			else if (Input.GetKey(KeyCode.RightArrow)) {
				Change(true);
			}
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				Change(false);
			}
		}
	}
}
