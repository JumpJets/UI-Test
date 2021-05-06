using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AR : MonoBehaviour {
	// Singleton
	private static AR _instance;

	public static AR Instance { get { return _instance; } }

#if UNITY_EDITOR

	private int resolutionX;
	private int resolutionY;
	public int aspect; // Global value to use, like AR.Instance.aspect

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	private void Start() {
		resolutionX = Screen.width;
		resolutionY = Screen.height;
		aspect = Camera.main.aspect >= 1 ? 0 : 1;

		if (aspect == 1) {
			Debug.Log("Change to height layout");
			StartCoroutine(SwitchUI());
		}
	}

	// Update is called once per frame
	private void Update() {
		if (resolutionX == Screen.width && resolutionY == Screen.height) return;

		resolutionX = Screen.width;
		resolutionY = Screen.height;

		int _aspect = Camera.main.aspect >= 1 ? 0 : 1;

		if (_aspect == aspect) return;

		aspect = _aspect;
		Debug.Log(aspect == 0 ? "Change to width layout" : "Change to height layout");
		StartCoroutine(SwitchUI());
	}

	public IEnumerator SwitchUI() {
		GameObject panel = GameObject.Find("Panel");
		RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
		GridLayoutGroup topGridLayoutGroup = GameObject.Find("Top").GetComponent<GridLayoutGroup>();
		GridLayoutGroup bottomGridLayoutGroup = GameObject.Find("Bottom").GetComponent<GridLayoutGroup>();
		RectTransform imageRectTransform = GameObject.Find("Image").GetComponent<RectTransform>();

		if (aspect == 0) {
			// Change panel position for new layout
			panelRectTransform.anchorMin = new Vector2(0, 0);
			panelRectTransform.anchorMax = new Vector2(0, 1);
			panelRectTransform.pivot = new Vector2(0, 0.5f);
			panelRectTransform.anchoredPosition = new Vector3(0, 0, 0);
			panelRectTransform.sizeDelta = new Vector2(100, 0);

			// Replace Horizontal to Vertical layout
			Destroy(panel.GetComponent<HorizontalLayoutGroup>());

			// Wait 1 frame to take layout changes
			yield return 0;

			VerticalLayoutGroup lg = panel.AddComponent<VerticalLayoutGroup>() as VerticalLayoutGroup;
			lg.childAlignment = TextAnchor.MiddleCenter;

			// Same for nested elements
			topGridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			bottomGridLayoutGroup.childAlignment = TextAnchor.LowerCenter;

			// Change layout for image
			imageRectTransform.rotation = Quaternion.Euler(0f, 0f, 90f);
		} else {
			panelRectTransform.anchorMin = new Vector2(0, 1);
			panelRectTransform.anchorMax = new Vector2(1, 1);
			panelRectTransform.pivot = new Vector2(0.5f, 0);
			panelRectTransform.anchoredPosition = new Vector3(0, -100, 0);
			panelRectTransform.sizeDelta = new Vector2(0, 100);

			Destroy(panel.GetComponent<VerticalLayoutGroup>());

			yield return 0;

			HorizontalLayoutGroup lg = panel.AddComponent<HorizontalLayoutGroup>() as HorizontalLayoutGroup;
			lg.childAlignment = TextAnchor.MiddleCenter;

			topGridLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			bottomGridLayoutGroup.childAlignment = TextAnchor.MiddleRight;

			imageRectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

#endif //UNITY_EDITOR
}
