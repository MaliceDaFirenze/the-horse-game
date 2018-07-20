using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dir{
	LEFT,
	DOWN,
	RIGHT,
	UP
}

public class ArrowSequences : MonoBehaviour {

	public static dir[] GetArrowSequence(actionID id){
		switch (id) {
		case actionID.NO_SEQUENCE:
			return null;
		case actionID.BRUSH_HORSE:
			return BrushHorse ();
		case actionID.PET_HORSE:
			return PetHorse ();
		case actionID.FEED_HORSE:
			return FeedHorse();
		case actionID.WATER_HORSE:
			return WaterHorse();
		case actionID.CLEAN_MANURE:
			return CleanManure();
		case actionID.FILL_BUCKET:
			return FillBucket();
		case actionID.PUT_ON_HALTER:
			return PutOnHalter();
		case actionID.PUT_ON_LEAD:
			return PutOnLead();
		case actionID.PUT_ON_HALTER_AND_LEAD:
			return PutOnHalterAndLead();
		case actionID.LEAD_HORSE:
			return LeadHorse();
		case actionID.TIE_HORSE_TO_POST:
			return TieHorseToPost();
		case actionID.PUT_ON_SADDLE_WITH_PAD:
			return PutOnSaddleWithPad();
		case actionID.PUT_ON_BRIDLE:
			return PutOnBridle();
		default:
			return null;
		}
	}

	private static dir[] BrushHorse (){
		dir[] result = new dir[4];

		result [0] = dir.DOWN;
		result [1] = dir.RIGHT;
		result [2] = dir.DOWN;
		result [3] = dir.LEFT;
		return result;
	}

	private static dir[] FeedHorse (){
		dir[] result = new dir[3];

		result [0] = dir.LEFT;
		result [1] = dir.LEFT;
		result [2] = dir.UP;
		return result;
	}

	private static dir[] WaterHorse (){
		dir[] result = new dir[3];

		result [0] = dir.RIGHT;
		result [1] = dir.LEFT;
		result [2] = dir.DOWN;
		return result;
	}

	private static dir[] CleanManure (){
		dir[] result = new dir[3];

		result [0] = dir.RIGHT;
		result [1] = dir.UP;
		result [2] = dir.RIGHT;
		return result;
	}

	private static dir[] FillBucket (){
		dir[] result = new dir[2];

		result [0] = dir.DOWN;
		result [1] = dir.DOWN;
		return result;
	}

	private static dir[] PutOnHalter (){
		dir[] result = new dir[2];

		result [0] = dir.RIGHT;
		result [1] = dir.UP;
		return result;
	}

	private static dir[] PutOnHalterAndLead (){
		dir[] result = new dir[3];

		result [0] = dir.RIGHT;
		result [1] = dir.UP;
		result [2] = dir.UP;
		return result;
	}

	private static dir[] PutOnLead (){
		dir[] result = new dir[2];

		result [0] = dir.UP;
		result [1] = dir.RIGHT;
		return result;
	}

	private static dir[] LeadHorse (){
		dir[] result = new dir[2];

		result [0] = dir.RIGHT;
		result [1] = dir.RIGHT;
		return result;
	}

	private static dir[] TieHorseToPost (){
		dir[] result = new dir[3];

		result [0] = dir.LEFT;
		result [1] = dir.DOWN;
		result [2] = dir.RIGHT;
		return result;
	}

	private static dir[] PutOnSaddleWithPad (){
		dir[] result = new dir[4];

		result [0] = dir.UP;
		result [1] = dir.UP;
		result [2] = dir.DOWN;
		result [3] = dir.RIGHT;
		return result;
	}

	private static dir[] PutOnBridle (){
		dir[] result = new dir[3];

		result [0] = dir.RIGHT;
		result [1] = dir.UP;
		result [2] = dir.RIGHT;
		return result;
	}

	private static dir[] PetHorse (){
		return GetRandomArray (3);
	}

	private static dir[] GetRandomArray(int length){
		dir[] result = new dir[length];

		for (int i = 0; i < result.Length; ++i) {
			result [i] = (dir)Random.Range (0, 3);
			if (i >= 2) {
				while (result [i - 1] == result [i] && result [i - 2] == result [i]) {
					result [i] = (dir)Random.Range (0, 3);
				}
			}
		}
		return result;
	}
}
