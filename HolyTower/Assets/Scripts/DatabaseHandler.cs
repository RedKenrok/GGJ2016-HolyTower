using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public struct TableIntro {

	private string ImagePath;
	public string imagePath {
		get {
			return ImagePath;
		}
	}

	private string[] Text;
	public string[] text {
		get {
			return Text;
		}
	}

	public TableIntro(string _imagePath, string[] _text) {
		ImagePath = _imagePath;
		Text = _text;
	}
}

public struct TableEvent {

	private RowEvent Intro;
	public RowEvent intro {
        get {
            return Intro;
        }
    }

	private RowEvent[] Results;
	public RowEvent result(int guardian, int outcome) {
		return Results[(guardian * 3) + outcome];
    }

	public TableEvent(RowEvent _intro, RowEvent[] _results) {
        Intro = _intro;
        Results = _results;
    }
}

public struct RowEvent {

    private int NextEvent;
    public int nextEvent {
        get {
            return NextEvent;
        }
    }

    private int DestroyedStructure;
    public int destroyedStructure {
        get {
            return DestroyedStructure;
        }
    }

    private string ImagePath;
    public string imagePath {
        get {
            return ImagePath;
        }
    }

    private string[] Text;
    public string[] text {
		get {
			return Text;
		}
	}

	public RowEvent(int _nextEvent, int _destroyedStructure, string _imagePath, string[] _text) {
        NextEvent = _nextEvent;
        DestroyedStructure = _destroyedStructure;
        ImagePath = _imagePath;
        Text = _text;
    }
}

public struct TableActions {

	private RowActions[] Rows;
	public RowActions[] rows {
		get {
			return Rows;
		}
	}

	public TableActions(RowActions[] _rows) {
		Rows = _rows;
	}
}

public struct RowActions {

	private string[] Intro;
	public string[] intro {
		get {
			return Intro;
		}
	}

	private string[] Positive;
	public string[] positive {
		get {
			return Positive;
		}
	}

	private string[] Neutral;
	public string[] neutral {
		get {
			return Neutral;
		}
	}

	private string[] Negative;
	public string[] negative {
		get {
			return Negative;
		}
	}

	public RowActions(string[] _intro, string[] _positive, string[] _neutral, string[] _negative) {
		Intro = _intro;
		Positive = _positive;
		Neutral = _neutral;
		Negative = _negative;
	}
}

public class DatabaseHandler {

	private static readonly char[] splitCharacters = new char[] { '|' };

	public static TableIntro GetFromDatabaseIntro(Data.intros id) {
		IDbConnection databaseConnection;
		databaseConnection = new SqliteConnection("URI=file:" + Application.dataPath + "/Database/Intro.db");
		databaseConnection.Open();

		IDbCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandText = "SELECT imagepath, text FROM " + "tableId_" + id.ToString() + " ORDER BY id ASC";
		IDataReader databaseResults = databaseCommand.ExecuteReader();

		string imagePath = "";
		string[] textArray = new string[0];
		while (databaseResults.Read()) {
			imagePath = databaseResults.GetString(0);
			textArray = databaseResults.GetString(1).Split(splitCharacters, 999);
		}

		databaseResults.Close();
		databaseResults = null;
		databaseCommand.Dispose();
		databaseCommand = null;
		databaseConnection.Close();
		databaseConnection = null;

		return new TableIntro(imagePath, textArray);
	}

	public static TableEvent GetFromDatabaseEvents(Data.events id) {
        IDbConnection databaseConnection;
		databaseConnection = new SqliteConnection("URI=file:" + Application.dataPath + "/Database/Events.db");
		databaseConnection.Open();

		IDbCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandText = "SELECT nextevent, destroyedstructure, imagepath, text FROM " + "tableId_" + ((int)id).ToString() + " ORDER BY id ASC";
		IDataReader databaseResults = databaseCommand.ExecuteReader();

		RowEvent rowIntro = new RowEvent(-1, -1, "", new string[0]);
		List<RowEvent> rowList = new List<RowEvent>();
		bool intro = false;
		while(databaseResults.Read()) {
			string[] textArray = databaseResults.GetString(3).Split(splitCharacters, 999);
			RowEvent row = new RowEvent (databaseResults.GetInt32(0), databaseResults.GetInt32(1), databaseResults.GetString(2), textArray);

			if (!intro) {
				rowIntro = row;
				intro = true;
			}
			else {
				rowList.Add (row);
			}
		}
		RowEvent[] rowArray = new RowEvent[rowList.Count];
		for (int i = 0; i < rowArray.Length; i++) {
			rowArray [i] = rowList [i];
		}
        
		databaseResults.Close();
		databaseResults = null;
		databaseCommand.Dispose();
		databaseCommand = null;
		databaseConnection.Close();
		databaseConnection = null;

		return new TableEvent(rowIntro, rowArray);
	}

	public static TableActions GetFromDatabaseActions(Data.actions id) {
		IDbConnection databaseConnection;
		databaseConnection = new SqliteConnection("URI=file:" + Application.dataPath + "/Database/Actions.db");
		databaseConnection.Open();

		IDbCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandText = "SELECT intro, positive, neutral, negative FROM " + "tableId_" + ((int)id).ToString() + " ORDER BY id ASC";
		IDataReader databaseResults = databaseCommand.ExecuteReader();

		List<RowActions> rowList = new List<RowActions> ();
		while (databaseResults.Read()) {
			string[] textArray0 = databaseResults.GetString(0).Split(splitCharacters, 999);
			string[] textArray1 = databaseResults.GetString(1).Split(splitCharacters, 999);
			string[] textArray2 = databaseResults.GetString(2).Split(splitCharacters, 999);
			string[] textArray3 = databaseResults.GetString(3).Split(splitCharacters, 999);
			rowList.Add(new RowActions(textArray0, textArray1, textArray2, textArray3));
		}
		RowActions[] rowArray = new RowActions[rowList.Count];
		for (int i = 0; i < rowArray.Length; i++) {
			rowArray [i] = rowList [i];
		}

		databaseResults.Close();
		databaseResults = null;
		databaseCommand.Dispose();
		databaseCommand = null;
		databaseConnection.Close();
		databaseConnection = null;

		return new TableActions(rowArray);
	}
}