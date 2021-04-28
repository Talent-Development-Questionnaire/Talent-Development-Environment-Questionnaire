from flask import Blueprint, Flask, current_app
from flask_mysqldb import MySQL
from pathlib import Path
from app.extensions import mysql

player_blueprint = Blueprint('player_blueprint', __name__)


@player_blueprint.route('/getQuestions/<qType>')
def getQuestions(qType):

    path = Path(__file__).parent.absolute()
    if qType == '59':
        questions = open("%s/59questions.txt" %(path), "r", encoding="utf-8")
    else:
        questions = open("%s/28questions.txt" %(path), "r", encoding="utf-8")

    return questions.read()


@player_blueprint.route('/submitQuestion/<qID>/<qNumber>/<qScore>')
def submitQuestion(qID,qNumber,qScore):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM question WHERE questionnaire_id = '%s' AND question_no = '%s'" % (qID, qNumber))

    if cursor.rowcount is 0:
        cursor.execute("INSERT INTO question (questionnaire_id, question_no, score) VALUES ('%s','%s','%s')" % (qID, qNumber, qScore))
        mysql.connection.commit()
        return'True'

    cursor.execute("SELECT score FROM question WHERE questionnaire_id = '%s' AND question_no = '%s'" % (qID, qNumber))
    currentScore = cursor.fetchall()
    newScore = currentScore[0]["score"] + int(qScore)
    cursor.execute("UPDATE question SET score = '%s' WHERE questionnaire_id = '%s' AND question_no = '%s'" % (newScore, qID, qNumber))

    mysql.connection.commit()
    cursor.close()
    return 'True'


@player_blueprint.route('/updateCompletionCount/<qID>')
def updateCount(qID):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT completions FROM questionnaire WHERE questionnaire_id = '%s'" % qID)
    countExists = cursor.fetchall()
    if cursor.rowcount is 0:
        cursor.execute("UPDATE questionnaire SET completions = 1 WHERE questionnaire_id = '%s'" % qID)
    else:
        newCount = countExists[0]["completions"] + 1
        cursor.execute("UPDATE questionnaire SET completions = '%s' WHERE questionnaire_id = '%s'" % (newCount, qID))

    mysql.connection.commit()
    cursor.close()
    return 'true'


@player_blueprint.route('/verifyAthlete/<email>/<otp>')
def verify_athlete(email, otp):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM questionnaire WHERE questionnaire_id = (SELECT questionnaire_id FROM emails_verify WHERE email = '%s' AND code = '%s')" % (email, otp))
    if(cursor.rowcount is not 0):
        return str(cursor.fetchall())
 
    return 'false'


@player_blueprint.route('/sendAthleteInfo/<sportAcademy>/<sport>/<name>/<age>/<gender>')
def sendAthleteInfo(sportAcademy, sport, name, age, gender):
    cursor = mysql.connection.cursor()
    cursor.execute("INSERT INTO athlete_info (sport_academy, sport, name, age, gender) VALUES ('%s','%s','%s','%s','%s')" % (sportAcademy, sport, name, age, gender))
    mysql.connection.commit()
    cursor.close()
    return 'true'

