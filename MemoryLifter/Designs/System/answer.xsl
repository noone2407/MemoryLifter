<?xml version='1.0' encoding='utf-16'?>
<!--  (c) 2006 LearnLift   -->
<!--  MemoryLifter Stylesheet   -->
<!--  Version 2.0 Date: 2007-10-08   -->
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:CardObject="urn:cardobject"
                exclude-result-prefixes="CardObject">
  <xsl:output method="xml" version="1.0"
              doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"
              doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" indent="no" omit-xml-declaration="no" />

  <xsl:param name="correctAnswerText" select="'Correct answer is:'" />
  <xsl:param name="correctFeedbackText" select="'Your answer was correct!'" />
  <xsl:param name="wrongFeedbackText" select="'Your answer was wrong!'" />
  <xsl:param name="youEnteredText" select="'You entered:'" />
  <xsl:param name="resizePicture" select="'Click image to show original size.'" />
  <xsl:param name="restorePicture" select="'Click image to show smaller size.'" />
  <xsl:param name="listeningModeText" select="'Click here to show the image.'" />

  <xsl:param name="clickForQuestion" select="'Click button for question:'" />
  <xsl:param name="clickForExample" select="'Click button for example:'" />

  <xsl:param name="slideshowMode" select="'false'" />
  <xsl:param name="selfAssessmentMode" select="'false'" />
  <xsl:param name="learningBox" select="0" />
  <xsl:param name="cardBox" select="0" />

  <xsl:param name="promotedMessage" select="'Card has been promoted to the next box!'" />
  <xsl:param name="demotedMessage" select="'Card has been demoted to box one.'" />
  <xsl:param name="selfassesmentPromotedMessage" select="'Do you like to promote the card?'" />
  <xsl:param name="selfassesmentDemotedMessage" select="'Do you like to demote the card?'" />
  
  <xsl:param name="mode" select="'wordmode'" />
  <xsl:param name="side" select="'answer'" />
  
  <xsl:param name="nothing" select="'&lt;Nothing&gt;'" />
  
  <xsl:param name="displayImages" select="'true'" />
  <xsl:param name="question2answer" select="'true'" />
  <xsl:param name="autoPlaySound" select="'true'" />
  <xsl:param name="userAnswer" select="'&lt;Nothing&gt;'"/>
  <xsl:param name="correct" select="'true'" />
  <xsl:param name="promoted" select="'true'" />

  <xsl:param name="baseURL" />
  <xsl:param name="stylePath" />

  <xsl:include href="answerLogic.xsl"/>
  <xsl:include href="style.xsl"/>
  <xsl:include href="scripts.xsl"/>
  <xsl:include href="jquery.xsl"/>
  <xsl:include href="jquery-ui.xsl"/>
  <xsl:include href="jquery-timer.xsl"/>
  <xsl:include href="animatedFeedback.xsl"/>

  <xsl:template match="/" priority="0">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>MemoryLifter - XSL stylesheet for ML program answer side</title>
        <base href="{$baseURL}" />
        <style type="text/css">
          <xsl:call-template name="defaultstyle"/>
          <xsl:call-template name="style"/>
          <xsl:value-of select="CardObject:GetStyle()"/>
        </style>
		<script type="text/javascript">
			<xsl:call-template name="jquery"/>
		</script>
		<script type="text/javascript">
			<xsl:call-template name="jquery-ui"/>
		</script>
		<script type="text/javascript">
			<xsl:call-template name="jquery-timer"/>
		</script>
        <script type="text/javascript">
          <xsl:call-template name="jscript"/>
        </script>
		<script type="text/javascript">
          <xsl:text disable-output-escaping="yes">
          var stylePath = "</xsl:text><xsl:value-of select="$stylePath"/><xsl:text disable-output-escaping="yes">";
          </xsl:text>
		</script>
		<script type="text/javascript">
			<xsl:call-template name="animatedFeedback"/>
		</script>
      </head>
      <xsl:choose>
        <xsl:when test="$slideshowMode = 'true'">
          <xsl:call-template name="card">
            <xsl:with-param name="class" select="'answerSlideshow'"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="$correct = 'true'">
          <xsl:call-template name="card">
            <xsl:with-param name="class" select="'answerCorrect'"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="card">
            <xsl:with-param name="class" select="'answerWrong'"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </html>
  </xsl:template>
</xsl:stylesheet>