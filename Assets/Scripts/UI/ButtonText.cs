using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private Text text;

	public Color NormalColor;

	public Color HighlightedColor;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponentInChildren<Text> ();
		//text.color = NormalColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter(PointerEventData pointerEventData) {
		text.color = HighlightedColor;
	}

	public void OnPointerExit(PointerEventData pointerEventdata) {
		text.color = NormalColor;
	}
}
