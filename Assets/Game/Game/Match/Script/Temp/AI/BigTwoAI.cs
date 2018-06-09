using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoAI : MonoBehaviour {

	public enum WEIGHT {
		ONE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, ELEVEN, TWELVE, THIRTEEN,
	}

	public virtual string GetPlayCommand (List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		return null;
	}

	protected void GetBiggerSetForSingle (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (1 != lastSet.Count) {
			return;
		}
		BigTwoPoker lastPoker = lastSet [0];
		List<BigTwoPoker> listTargetPoker = new List<BigTwoPoker> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker = currentHand [i];
			if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPokerBigger (poker, lastPoker)) {
				int insertPos = 0;
				for (int j = listTargetPoker.Count - 1; j >= 0; j--) {
					BigTwoPoker pokerInList = listTargetPoker [j];
					if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPokerBigger (poker, pokerInList)) {
						insertPos = j + 1;
						break;
					}

				}
				listTargetPoker.Insert (insertPos, poker);
			}
		}
		if (0 == listTargetPoker.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTargetPoker.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTargetPoker.Count - 1);
		BigTwoPoker targetPoker = listTargetPoker [weightIndex];
		currentSet.Add (targetPoker);
	}

	protected void GetBiggerSetForPair (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (2 != lastSet.Count) {
			return;
		}
		for (int i = 0; i < lastSet.Count - 1; i++) {
			if (lastSet [i + 1]._pokerFace != lastSet [i]._pokerFace) {
				return;
			}
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker1._pokerFace != poker2._pokerFace) {
					continue;
				}
				List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
				listPoker.Add (poker1);
				listPoker.Add (poker2);
				// PrintListPoker (listPoker);
				// PrintListPoker (lastSet);
				if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsPairBigger (listPoker, lastSet)) {
					continue;
				}
				int insertPos = 0;
				for (int k = listTarget.Count - 1; k >= 0; k--) {
					List<BigTwoPoker> pairInList = listTarget [k];
					if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPairBigger (listPoker, pairInList)) {
						insertPos = k + 1;
						break;
					}

				}
				listTarget.Insert (insertPos, listPoker);
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForThree (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (3 != lastSet.Count) {
			return;
		}
		if (lastSet [0]._pokerFace != lastSet [1]._pokerFace || lastSet [0]._pokerFace != lastSet [2]._pokerFace) {
			return;
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker1._pokerFace != poker2._pokerFace) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (poker1._pokerFace != poker3._pokerFace) {
						continue;
					}
					List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
					listPoker.Add (poker1);
					listPoker.Add (poker2);
					listPoker.Add (poker3);
					if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsThreeBigger (listPoker, lastSet)) {
						continue;
					}
					int insertPos = 0;
					for (int l = listTarget.Count - 1; l >= 0; l--) {
						List<BigTwoPoker> pairInList = listTarget [l];
						if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsThreeBigger (listPoker, pairInList)) {
							insertPos = l + 1;
							break;
						}

					}
					listTarget.Insert (insertPos, listPoker);
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForStraight (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (5 != lastSet.Count) {
			return;
		}
		for (int i = 0; i < lastSet.Count - 1; i++) {
			if (1 != (lastSet [i + 1]._pokerFace - lastSet [i]._pokerFace)) {
				return;
			}
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (1 != poker2._pokerFace - poker1._pokerFace) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (1 != poker3._pokerFace - poker2._pokerFace) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (1 != poker4._pokerFace - poker3._pokerFace) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (1 != poker5._pokerFace - poker4._pokerFace) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsStraightBigger (listPoker, lastSet)) {
								continue;
							}
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> pairInList = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsStraightBigger (listPoker, pairInList)) {
									insertPos = n + 1;
									break;
								}

							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForFlower (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (5 != lastSet.Count) {
			return;
		}
		for (int i = 0; i < lastSet.Count - 1; i++) {
			if (lastSet [i + 1]._pokerType != lastSet [i]._pokerType) {
				return;
			}
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker2._pokerType != poker1._pokerType) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (poker3._pokerType != poker2._pokerType) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (poker4._pokerType != poker3._pokerType) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (poker5._pokerType != poker4._pokerType) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsFlowerBigger (listPoker, lastSet)) {
								continue;
							}
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> pairInList = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsFlowerBigger (listPoker, pairInList)) {
									insertPos = n + 1;
									break;
								}

							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForFullHouse (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (5 != lastSet.Count) {
			return;
		}
		if (lastSet [0]._pokerFace != lastSet [1]._pokerFace || lastSet [0]._pokerFace != lastSet [2]._pokerFace || lastSet [3]._pokerFace != lastSet [4]._pokerFace) {
			return;
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker2._pokerFace != poker1._pokerFace) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (poker3._pokerFace != poker2._pokerFace) {
						continue;
					}
					for (int l = 0; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (poker4._pokerFace == poker1._pokerFace) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (poker5._pokerFace != poker4._pokerFace) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsFullHouseBigger (listPoker, lastSet)) {
								continue;
							}
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> pairInList = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsFullHouseBigger (listPoker, pairInList)) {
									insertPos = n + 1;
									break;
								}

							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForKingKong (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (5 != lastSet.Count) {
			return;
		}
		if (lastSet [0]._pokerFace != lastSet [1]._pokerFace || lastSet [0]._pokerFace != lastSet [2]._pokerFace || lastSet [0]._pokerFace != lastSet [3]._pokerFace) {
			return;
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker2._pokerFace != poker1._pokerFace) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (poker3._pokerFace != poker2._pokerFace) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (poker4._pokerFace != poker3._pokerFace) {
							continue;
						}
						for (int m = 0; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (poker5._pokerFace == poker1._pokerFace) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsKingKongBigger (listPoker, lastSet)) {
								continue;
							}
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> pairInList = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsKingKongBigger (listPoker, pairInList)) {
									insertPos = n + 1;
									break;
								}

							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForStraightFlush (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (5 != lastSet.Count) {
			return;
		}
		for (int i = 0; i < lastSet.Count - 1; i++) {
			BigTwoPoker poker1 = lastSet [i];
			BigTwoPoker poker2 = lastSet [i + 1];
			if (1 != (poker2._pokerFace - poker1._pokerFace) || poker2._pokerType != poker1._pokerType) {
				return;
			}
		}
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (1 != (poker2._pokerFace - poker1._pokerFace) || poker2._pokerType != poker1._pokerType) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (1 != (poker3._pokerFace - poker2._pokerFace) || poker3._pokerType != poker2._pokerType) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (1 != (poker4._pokerFace - poker3._pokerFace) || poker4._pokerType != poker3._pokerType) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (1 != (poker5._pokerFace - poker4._pokerFace) || poker5._pokerType != poker4._pokerType) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							if (BigTwoRule.CompareResult.BIGGER != deck.Rule.IsStraightFlushBigger (listPoker, lastSet)) {
								continue;
							}
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> pairInList = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsStraightFlushBigger (listPoker, pairInList)) {
									insertPos = n + 1;
									break;
								}

							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		if (0 == listTarget.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTarget.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTarget.Count - 1);
		List<BigTwoPoker> targetPair = listTarget [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected bool IsStraight (List<BigTwoPoker> listPoker) {
		if (5 != listPoker.Count) {
			return false;
		}
		for (int i = 0; i < listPoker.Count - 1; i++) {
			if (1 != (listPoker [i + 1]._pokerFace - listPoker [i]._pokerFace)) {
				return false;
			}
		}
		return true;
	}

	protected void GetStraight (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<BigTwoPoker> listSortPoker = new List<BigTwoPoker> ();
		GetListPokerSortByFace (ref listSortPoker, currentHand);
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < listSortPoker.Count; i++) {
			BigTwoPoker poker1 = listSortPoker [i];
			for (int j = i + 1; j < listSortPoker.Count; j++) {
				BigTwoPoker poker2 = listSortPoker [j];
				if (1 != (poker2._pokerFace - poker1._pokerFace)) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (1 != (poker3._pokerFace - poker2._pokerFace)) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (1 != (poker4._pokerFace - poker3._pokerFace)) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (1 != (poker5._pokerFace - poker4._pokerFace)) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> target = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsStraightBigger (listPoker, target)) {
									insertPos = n + 1;
									break;
								}
							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		for (int i = 0; i < listTarget.Count; i++) {
			List<BigTwoPoker> target = listTarget [i];
			outputSet.Add (target);
		}
	}

	protected bool IsFlower (List<BigTwoPoker> listPoker) {
		if (5 != listPoker.Count) {
			return false;
		}
		for (int i = 0; i < listPoker.Count - 1; i++) {
			if (listPoker [i + 1]._pokerType != listPoker [i]._pokerType) {
				return false;
			}
		}
		return true;
	}

	protected void GetFlower  (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<List<BigTwoPoker>> listType = new List<List<BigTwoPoker>> ();
		GetListPokerType (ref listType, currentHand);
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < listType.Count; i++) {
			List<BigTwoPoker> listTypePoker = listType [i];
			if (listTypePoker.Count < 5) {
				continue;
			}
			List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
			for (int j = 0; j <= listTypePoker.Count - 5; j++) {
				for (int k = j; k < j + 5; k++) {
					listPoker.Add (listTypePoker [k]);
				}
			}
			int insertPos = 0;
			for (int n = listTarget.Count - 1; n >= 0; n--) {
				List<BigTwoPoker> target = listTarget [n];
				if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsFlowerBigger (listPoker, target)) {
					insertPos = n + 1;
					break;
				}

			}
			listTarget.Insert (insertPos, listPoker);
		}
		for (int i = 0; i < listTarget.Count; i++) {
			List<BigTwoPoker> target = listTarget [i];
			outputSet.Add (target);
		}
	}

	protected bool IsFullHouse (List<BigTwoPoker> listPoker) {
		if (5 != listPoker.Count) {
			return false;
		}
		if (listPoker [0]._pokerFace != listPoker [1]._pokerFace || listPoker [0]._pokerFace != listPoker [2]._pokerFace || listPoker [3]._pokerFace != listPoker [4]._pokerFace) {
			return false;
		}
		return true;
	}

	protected void GetFullHouse  (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<List<BigTwoPoker>> listFace = new List<List<BigTwoPoker>> ();
		GetListPokerFace (ref listFace, currentHand);
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < listFace.Count; i++) {
			List<BigTwoPoker> listFacePoker = listFace [i];
			if (listFacePoker.Count < 3) {
				continue;
			}
			List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
			for (int j = 0; j < 3; j++) {
				listPoker.Add (listFacePoker [j]);
			}
			List<BigTwoPoker> listPair = new List<BigTwoPoker> ();
			for (int j = 2; j < 5; j++) {
				for (int k = 0; k < listFace.Count; k++) {
					List<BigTwoPoker> listPokerPair = listFace [k];
					if (j == listPokerPair.Count) {
						if (k == i) {
							continue;
						}
						listPair.Add (listPokerPair [0]);
						listPair.Add (listPokerPair [1]);
						break;
					}
				}
			}
			if (0 == listPair.Count) {
				continue;
			}
			listPoker.Add (listPair [0]);
			listPoker.Add (listPair [1]);
			int insertPos = 0;
			for (int n = listTarget.Count - 1; n >= 0; n--) {
				List<BigTwoPoker> target = listTarget [n];
				if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsFullHouseBigger (listPoker, target)) {
					insertPos = n + 1;
					break;
				}

			}
			listTarget.Insert (insertPos, listPoker);
		}
		for (int i = 0; i < listTarget.Count; i++) {
			List<BigTwoPoker> target = listTarget [i];
			outputSet.Add (target);
		}
	}

	protected bool IsKingKong (List<BigTwoPoker> listPoker) {
		if (5 != listPoker.Count) {
			return false;
		}
		if (listPoker [0]._pokerFace != listPoker [1]._pokerFace || listPoker [0]._pokerFace != listPoker [2]._pokerFace || listPoker [0]._pokerFace != listPoker [3]._pokerFace) {
			return false;
		}
		return true;
	}

	protected void GetKingKong (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<List<BigTwoPoker>> listFace = new List<List<BigTwoPoker>> ();
		GetListPokerFace (ref listFace, currentHand);
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < listFace.Count; i++) {
			List<BigTwoPoker> listFacePoker = listFace [i];
			if (listFacePoker.Count != 4) {
				continue;
			}
			List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
			for (int j = 0; j < listFacePoker.Count; j++) {
				listPoker.Add (listFacePoker [j]);
			}
			BigTwoPoker poker5 = null;
			for (int j = 1; j < 5; j++) {
				for (int k = 0; k < listFace.Count; k++) {
					List<BigTwoPoker> listPoker5 = listFace [k];
					if (j == listPoker5.Count) {
						if (k == i) {
							continue;
						}
						poker5 = listPoker5 [0];
						break;
					}
				}
			}
			if (null == poker5) {
				continue;
			}
			listPoker.Add (poker5);
			int insertPos = 0;
			for (int n = listTarget.Count - 1; n >= 0; n--) {
				List<BigTwoPoker> target = listTarget [n];
				if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsKingKongBigger (listPoker, target)) {
					insertPos = n + 1;
					break;
				}

			}
			listTarget.Insert (insertPos, listPoker);
		}
		for (int i = 0; i < listTarget.Count; i++) {
			List<BigTwoPoker> target = listTarget [i];
			outputSet.Add (target);
		}
	}

	protected bool IsStraightFlush (List<BigTwoPoker> listPoker) {
		if (5 != listPoker.Count) {
			return false;
		}
		for (int i = 0; i < listPoker.Count - 1; i++) {
			BigTwoPoker poker1 = listPoker [i];
			BigTwoPoker poker2 = listPoker [i + 1];
			if (1 != (poker2._pokerFace - poker1._pokerFace) || poker2._pokerType != poker1._pokerType) {
				return false;
			}
		}
		return true;
	}

	protected void GetStraightFlush (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<BigTwoPoker> listSortPoker = new List<BigTwoPoker> ();
		GetListPokerSortByFace (ref listSortPoker, currentHand);
		List<List<BigTwoPoker>> listTarget = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < listSortPoker.Count; i++) {
			BigTwoPoker poker1 = listSortPoker [i];
			for (int j = i + 1; j < listSortPoker.Count; j++) {
				BigTwoPoker poker2 = listSortPoker [j];
				if (1 != (poker2._pokerFace - poker1._pokerFace) || poker2._pokerType != poker1._pokerType) {
					continue;
				}
				for (int k = j + 1; k < currentHand.Count; k++) {
					BigTwoPoker poker3 = currentHand [k];
					if (1 != (poker3._pokerFace - poker2._pokerFace) || poker3._pokerType != poker2._pokerType) {
						continue;
					}
					for (int l = k + 1; l < currentHand.Count; l++) {
						BigTwoPoker poker4 = currentHand [l];
						if (1 != (poker4._pokerFace - poker3._pokerFace) || poker4._pokerType != poker3._pokerType) {
							continue;
						}
						for (int m = l + 1; m < currentHand.Count; m++) {
							BigTwoPoker poker5 = currentHand [m];
							if (1 != (poker5._pokerFace - poker4._pokerFace) || poker5._pokerType != poker4._pokerType) {
								continue;
							}
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							listPoker.Add (poker4);
							listPoker.Add (poker5);
							int insertPos = 0;
							for (int n = listTarget.Count - 1; n >= 0; n--) {
								List<BigTwoPoker> target = listTarget [n];
								if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsStraightFlushBigger (listPoker, target)) {
									insertPos = n + 1;
									break;
								}
							}
							listTarget.Insert (insertPos, listPoker);
						}
					}
				}
			}
		}
		for (int i = 0; i < listTarget.Count; i++) {
			List<BigTwoPoker> target = listTarget [i];
			outputSet.Add (target);
		}
	}

	protected void GetFiveSetByType (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (IsStraight (lastSet)) {
			GetStraight (ref outputSet, currentHand, deck);
			GetFlower (ref outputSet, currentHand, deck);
			GetFullHouse (ref outputSet, currentHand, deck);
			GetKingKong (ref outputSet, currentHand, deck);
			GetStraightFlush (ref outputSet, currentHand, deck);
			return;
		}
		if (IsFlower (lastSet)) {
			GetFlower (ref outputSet, currentHand, deck);
			GetFullHouse (ref outputSet, currentHand, deck);
			GetKingKong (ref outputSet, currentHand, deck);
			GetStraightFlush (ref outputSet, currentHand, deck);
			return;
		}
		if (IsFullHouse (lastSet)) {
			GetFullHouse (ref outputSet, currentHand, deck);
			GetKingKong (ref outputSet, currentHand, deck);
			GetStraightFlush (ref outputSet, currentHand, deck);
			return;
		}
		if (IsKingKong (lastSet)) {
			GetKingKong (ref outputSet, currentHand, deck);
			GetStraightFlush (ref outputSet, currentHand, deck);
			return;
		}
		if (IsStraightFlush (lastSet)) {
			GetStraightFlush (ref outputSet, currentHand, deck);
			return;
		}
	}

	protected void GetFiveSetBigger (ref List<List<BigTwoPoker>> outputSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		List<List<BigTwoPoker>> fiveSet = new List<List<BigTwoPoker>> ();
		GetFiveSetByType (ref fiveSet, currentHand, lastSet, deck);
		for (int i = 0; i < fiveSet.Count; i++) {
			List<BigTwoPoker> listPoker = fiveSet [i];
			// if (IsStraightFlush (listPoker)) {
			// }
			// if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsStraightFlushBigger (listPoker, lastSet)) {
			// }
		}
	}

	private void GetListPokerSortByFace (ref List<BigTwoPoker> outputList, List<BigTwoPoker> inputList) {
		outputList.Clear ();
		for (int i = 0; i < inputList.Count; i++) {
			BigTwoPoker poker1 = inputList [i];
			int insertIndex = 0;
			for (int j = outputList.Count - 1; j >= 0; j--) {
				BigTwoPoker poker2 = outputList [j];
				if (poker1._pokerFace > poker2._pokerFace) {
					insertIndex = j + 1;
					break;
				}
				if (poker1._pokerFace == poker2._pokerFace) {
					if (poker1._pokerType > poker2._pokerType) {
						insertIndex = j + 1;
						break;
					}
				}
			}
			outputList.Insert (insertIndex, poker1);
		}
	}

	private void GetListPokerFace (ref List<List<BigTwoPoker>> outputList, List<BigTwoPoker> inputList) {
		outputList.Clear ();
		List<BigTwoPoker> listSortPoker = new List<BigTwoPoker> ();
		GetListPokerSortByFace (ref listSortPoker, inputList);
		for (int i = 0; i < listSortPoker.Count; i++) {
			BigTwoPoker poker = listSortPoker [i];
			bool hasInsert = false;
			for (int j = 0; j < outputList.Count; j++) {
				List<BigTwoPoker> listPoker = outputList [j];
				if (listPoker[0]._pokerFace == poker._pokerFace) {
					listPoker.Add (poker);
					hasInsert = true;
					break;
				}
			}
			if (!hasInsert) {
				List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
				listPoker.Add (poker);
				outputList.Add (listPoker);
			}
		}
	}

	private void GetListPokerType (ref List<List<BigTwoPoker>> outputList, List<BigTwoPoker> inputList) {
		outputList.Clear ();
		List<BigTwoPoker> listSortPoker = new List<BigTwoPoker> ();
		GetListPokerSortByFace (ref listSortPoker, inputList);
		for (int i = 0; i < listSortPoker.Count; i++) {
			BigTwoPoker poker = listSortPoker [i];
			bool hasInsert = false;
			for (int j = 0; j < outputList.Count; j++) {
				List<BigTwoPoker> listPoker = outputList [j];
				if (listPoker[0]._pokerType == poker._pokerType) {
					listPoker.Add (poker);
					hasInsert = true;
					break;
				}
			}
			if (!hasInsert) {
				List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
				listPoker.Add (poker);
				outputList.Add (listPoker);
			}
		}
	}

	protected void PrintListPoker (List<BigTwoPoker> listPoker) {
		string str = string.Format ("PokerList[{0}]:", listPoker.Count);
		for (int i = 0; i < listPoker.Count; i++) {
			BigTwoPoker poker = listPoker [i];
			str = string.Format ("{0} [{1}][{2}]", str, poker._pokerType.ToString (), poker._pokerFace.ToString ());
		}
		Debug.Log (str);
	}

}
