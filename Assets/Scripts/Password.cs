using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Password {
	public string passwordName;
	[TextArea(3,10)]
	public string[] portugueseSentencesBefore;
	[TextArea(3,10)]
	public string portugueseSentenceDuring;
	[TextArea(3,10)]
	public string[] portugueseSentencesAfterRight;
	[TextArea(3,10)]
	public string[] portugueseSentencesAfterWrong;
	[TextArea(3,10)]
	public string[] englishSentencesBefore;
	[TextArea(3,10)]
	public string englishSentenceDuring;
	[TextArea(3,10)]
	public string[] englishSentencesAfterRight;
	[TextArea(3,10)]
	public string[] englishSentencesAfterWrong;
	

	
}
