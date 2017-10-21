using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// The button text.
	private Text text;

	// The normal button colour.
	public Color NormalColor;

	// The button highlight colour.
	public Color HighlightedColor;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponentInChildren<Text> ();
	}

	public void OnPointerEnter(PointerEventData pointerEventData) {
		text.color = HighlightedColor;
	}

	public void OnPointerExit(PointerEventData pointerEventdata) {
		text.color = NormalColor;
	}
}
