using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CSVTest : MonoBehaviour {

    public TextAsset csv;
    public Text text;
    string[,] dataArray;
    // string[] dialogueLines;
    string fullText;
    string[] textLines;
    int lineIndex = 0;

	// Use this for initialization
	void Start () {
        dataArray = CSVReader.SplitCsvGrid(csv.text);
	}

}
