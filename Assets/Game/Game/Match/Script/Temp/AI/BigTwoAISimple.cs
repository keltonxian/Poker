using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoAISimple : BigTwoAI {

	public override string GetPlayCommand (List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<BigTwoPoker> currentSet = new List<BigTwoPoker> ();
		GeneratePlaySet (ref currentSet, currentHand, deck);
		string cmd = deck.CombinePokerListToCMD (currentSet);
		return cmd;
	}

	private void GeneratePlaySet (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		if (deck.IsFirstPlay () || deck.IsRoundPassed ()) {
			GetBestInHand (ref currentSet, currentHand, deck);
			return;
		}
		List<BigTwoPoker> lastSet = deck.GetLastValidPlayPokerList ();
		if (0 == lastSet.Count) {
			return;
		}
		GetBiggerSet (ref currentSet, currentHand, lastSet, deck);
	}

	private void GetBestInHand (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		currentSet.Add (currentHand [0]);
		return;
	}

	private void GetBiggerSet (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (1 == lastSet.Count) {
			GetBiggerSetForSingle (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			return;
		}
		if (2 == lastSet.Count) {
			GetBiggerSetForPair (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			return;
		}
		if (3 == lastSet.Count) {
			GetBiggerSetForThree (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			return;
		}
		if (5 == lastSet.Count) {
			List<List<BigTwoPoker>> fiveSet = new List<List<BigTwoPoker>> ();
			Debug.Log ("=====Get Five Set=====");
			GetFiveSetBigger (ref fiveSet, currentHand, lastSet, deck);
			for (int i = 0; i < fiveSet.Count; i++) {
				PrintListPoker (fiveSet [i]);
			}
			Debug.Log ("=====Get Five Set=====");
			// TODO 
			GetBiggerSetForStraightFlush (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			if (currentSet.Count == 0) {
				GetBiggerSetForKingKong (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			}
			if (currentSet.Count == 0) {
				GetBiggerSetForFullHouse (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			}
			if (currentSet.Count == 0) {
				GetBiggerSetForFlower (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			}
			if (currentSet.Count == 0) {
				GetBiggerSetForStraight (WEIGHT.ONE, ref currentSet, currentHand, lastSet, deck);
			}
			return;
		}
	}

}
