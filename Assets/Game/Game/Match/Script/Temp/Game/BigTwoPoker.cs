using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigTwoPoker : MonoBehaviour {

	public enum TYPE {
		NONE = 0, DIAMOND = 1, CLUB = 2, HEART = 3, SPADE = 4,
	}
	public enum FACE {
		NONE = 0, THREE = 1, FOUR = 2, FIVE = 3, SIX = 4, SEVEN = 5, EIGHT = 6, NINE = 7, TEN = 8, JACK = 9, QUEEN = 10, KING = 11, ACE = 12, TWO = 13,
	}

	public TYPE _pokerType = TYPE.NONE;
	public FACE _pokerFace = FACE.NONE;

	public SpriteRenderer _faceSR = null;
	public SpriteRenderer _backSR = null;

	private bool _isSelected = false;
	public bool IsSelected {
		get {
			return _isSelected;
		}
		set {
			SetSelected (value);
		}
	}
	private bool _isAnimating = false;
	private Vector3 _faceDefaultPos;
	private bool _isTouchEnabled = true;
	public bool IsTouchEnabled {
		get {
			return _isTouchEnabled;
		}
		set {
			_isTouchEnabled = value;
		}
	}

	public void Init () {
		SetFace ();
		_faceDefaultPos = _faceSR.transform.localPosition;
	}

	public void SetBackVisible (bool isVisible) {
		_backSR.gameObject.SetActive (isVisible);
	}

	public void Reset () {
		IsSelected = false;
		IsTouchEnabled = false;
	}

	private void SetFace () {
		string path = "";
		if (_pokerType == TYPE.SPADE) {
			path += "Spade/";
		} else if (_pokerType == TYPE.HEART) {
			path += "Heart/";
		} else if (_pokerType == TYPE.CLUB) {
			path += "Club/";
		} else if (_pokerType == TYPE.DIAMOND) {
			path += "Diamond/";
		} else {
			return;
		}
		path += string.Format ("{0}", (int)_pokerFace);
		ResManager.Instance.LoadSpriteFromResourceAsync (path, (Sprite sprite) => {
			_faceSR.GetComponent<SpriteRenderer> ().sprite = sprite; 
		}, () => {
		});
	}

	public void SetSelected (bool isSelected) {
		if (!IsTouchEnabled) {
		}
		if (_isAnimating) {
			return;
		}
		if (isSelected == _isSelected) {
			return;
		}
		_isAnimating = true;
		float toY = _faceDefaultPos.y;
		if (!_isSelected) {
			toY += 0.2f;
		}
		_faceSR.transform.DOKill ();
		_faceSR.transform.DOLocalMoveY (toY, 0.15f).SetEase (Ease.Linear).OnComplete (() => {
			_isAnimating = false;
		});
		_isSelected = !_isSelected;
	}

}
