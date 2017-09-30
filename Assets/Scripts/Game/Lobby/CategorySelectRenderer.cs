using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelectRenderer : MonoBehaviour {
    public TriviaJSONParser.TriviaCategory category;
    [HideInInspector] public bool selected = true;

    public Text name;
    public Image bg;
	void Update () {
	    name.text = (selected ? "■ " : "□ ") +category.name;
	}

    public void ToggleSelected () {
        selected = !selected;
    }
}
