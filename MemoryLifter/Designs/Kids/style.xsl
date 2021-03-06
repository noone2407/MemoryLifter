<?xml version='1.0' encoding='utf-16'?>
<!--  (c) 2006 LearnLift   -->
<!--  MemoryLifter Stylesheet   -->
<!--  Version 2.0 Date: 2007-10-08   -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template name="style">
    <![CDATA[

	.question
	{
		color:		#000000;
	}

	.questionExample
	{
		color:		#000000;
	}

	.answer
	{
		color:		#000000;
	}

	.answerExample
	{
		color:		#000000;
	}

  .answerCorrect
	{
		background-color:	#ffffff;
		color:		#000000;
	}

	.answerWrong
	{
		background-color:	#ffffff;
		color:		#000000;
	}

  .closebarCorrect { background-color: #53905C; }

  .closebarWrong { background-color: #E7504A; }

  .feedbackCorrect { background-color: #ffffff; }

  .feedbackWrong { background-color: #ffffff; }

    #questionHeader:hover
  {
	  background-color: #e6f3c4;
  }
    #questionExample:hover
  {
	  background-color: #e6f3c4;
  }
  #answerHeader:hover
  {
	  background-color: #e6f3c4;
  }
  #answerExample:hover
  {
	  background-color: #e6f3c4;
  }
  #feedbackImageCorrect
  {
	height: 121px;
	width: 107px;
	padding: 10px;
	padding-left: 20px;
  }
  #feedbackImageWrong
  {
	height: 120px;
	width: 107px;
	padding: 10px;
	padding-left: 20px;
  }
]]>
  </xsl:template>
</xsl:stylesheet>