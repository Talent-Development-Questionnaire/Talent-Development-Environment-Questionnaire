from flask import Blueprint, Flask, current_app
from flask_mysqldb import MySQL

from app.extensions import mysql

player_blueprint = Blueprint('player_blueprint', __name__)


@player_blueprint.route('/player/getQuestions/<qType>')
def getQuestions(qType):

    if qType == '59':
        questions = open("59questions.txt").readlines()
    else:
        questions = open("28questions.txt").readlines()

    return str(questions)



@player_blueprint.route('/submitQuestion/<qID>/<qNumber>/<qScore>')
def submitQuestion(qID,qNumber,qScore):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM question WHERE questionnaire_id = '%s' AND question_no = '%s'" % (qID, qNumber))
    qExists = cursor.fetchall()
    if all(qExists) == True:
        cursor.execute("INSERT INTO question (questionnaire_id, question_no, score) VALUES ('%s','%s','%s')" % (qID, qNumber, qScore))
    else:
        cursor.execute("SELECT score FROM question WHERE questionnaire_id = '%s' AND question_no = '%s'" % (qID, qNumber))
        currentScore = cursor.fetchall()
        newScore = currentScore[0]["score"] + int(qScore)
        cursor.execute("UPDATE question SET score = '%s' WHERE questionnaire_id = '%s' AND question_no = '%s'" % (newScore, qID, qNumber))

    mysql.connection.commit()
    cursor.close()
    updateCount(qID)
    return 'True'

@player_blueprint.route('/updateCount/<qID>')
def updateCount(qID):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT completions FROM questionnaire WHERE questionnaire_id = '%s'" % qID)
    countExists = cursor.fetchall()
    if countExists == 0:
        cursor.execute("UPDATE questionnaire SET completions = 1 WHERE questionnaire_id = '%s'" % qID)
    else:
        newCount = countExists[0]["completions"] + 1
        cursor.execute("UPDATE questionnaire SET completions = '%s' WHERE questionnaire_id = '%s'" % (newCount, qID))

    mysql.connection.commit()
    cursor.close()








