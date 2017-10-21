using UnityEngine;

public class Data : MonoBehaviour {

	public static int towerStage = 0;
	public static int destroyedStructures = 0;

	public static readonly int winningCount = 4;
	public static readonly int losingCount = 3;

	public static int actionsTaken = 0;
	public static int killsCommited = 0;
	public static readonly float neutralThreshold = 0.5f;

	public enum intros { intro =0, epiloguePositive =1, epilogueNeutral =2, epilogueNegative =3 };
	public static readonly int introCount = 1;

	public enum events { storm =0, draught =1, fire =2, fish =3, bridge =4 };
	public static readonly int eventsCount = 5;

	public enum actions { boar =0, berry =1 };
	public static readonly int actionsCount = 2;

	public enum guardians { weather =0, harvest =1, earth =2 };
	public static readonly int guardiansCount = 3;
	public static readonly string[] guardiansText = new string[3] { "Weather", "Harvest", "Earth" };
	public static readonly int guardiansOptionWidth = -400;
	public static readonly int[] guardiansOptionRange = new int[2] { -48, -360 };

	public enum offers { boar =0, berry =1 };
	public static readonly int offersCount = 2;
	public static readonly string[] offersText = new string[2] { "Boar", "Berry" };
	public static readonly int offersOptionWidth = 400;
	public static readonly int[] offersOptionRange = new int[2] { -48, -360 };

	public enum succesRate { positive =0, neutral =1, negative =2 };

	public static void Reset() {
		towerStage = 0;
		destroyedStructures = 0;
		actionsTaken = 0;
		killsCommited = 0;
	}
}