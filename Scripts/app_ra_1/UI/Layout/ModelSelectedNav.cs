using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ModelSelectedNav : MonoBehaviour {

	[SerializeField]
	private HorizontalScrollSnap horizintalScrollSnap;

	[SerializeField]
	RectTransform[] models;

	// Use this for initialization
	void Start () {

		for (int i = 0; i<models.Length; i++)
		{
			models[i].anchoredPosition = new Vector3(models[i].anchoredPosition.x, -200, 0);
		}
		
	}

	int oldPage;
	float coolDown;
	// Update is called once per frame
	void Update () {

		if (horizintalScrollSnap.CurrentPage != oldPage)
			coolDown = 0.15f;

		if(coolDown <= 0)
			for (int i = 0; i < models.Length; i++)
			{
				if (horizintalScrollSnap.CurrentPage == i)
					models[i].anchoredPosition = Vector3.Lerp(models[i].anchoredPosition, new Vector3(models[i].anchoredPosition.x, 0, 0), Time.deltaTime*7);
				else
					models[i].anchoredPosition = Vector3.Lerp(models[i].anchoredPosition, new Vector3(models[i].anchoredPosition.x, -200, 0), Time.deltaTime*7);
			}

		coolDown -= Time.deltaTime;

		oldPage = horizintalScrollSnap.CurrentPage;

	}
}
