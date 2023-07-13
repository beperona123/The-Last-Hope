using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class YesOrNoText 
{
	[TextArea(3,10)]
	public string[] portugueseSentencesBefore;
	[TextArea(3,10)]
	public string portugueseSentenceDuring;
	[TextArea(3,10)]
	public string[] portugueseSentencesAfterYes;
	[TextArea(3,10)]
	public string[] portugueseSentencesAfterNo;
	[TextArea(3,10)]
	public string[] englishSentencesBefore;
	[TextArea(3,10)]
	public string englishSentenceDuring;
	[TextArea(3,10)]
	public string[] englishSentencesAfterYes;
	[TextArea(3,10)]
	public string[] englishSentencesAfterNo;
}
