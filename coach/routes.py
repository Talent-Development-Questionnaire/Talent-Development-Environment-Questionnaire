from flask import Blueprint, Flask, jsonify 
from flask_mysqldb import MySQL
from pathlib import Path

from app.extensions import mysql, mail, Message

coach_blueprint = Blueprint('coach_blueprint', __name__)


@coach_blueprint.route('/coach/checkEmail/<email>')
def checkEmail(email):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s'" % email)
    row_count = cursor.rowcount
    mysql.connection.commit()
    cursor.close()
    if row_count == 0:
        return 'true'
    else:
        return 'false'


@coach_blueprint.route('/coach/addCoach/<email>/<password>/<name>/<otp>')
def addcoach(name, email, password, otp):
    cursor = mysql.connection.cursor()
    cursor.execute("INSERT INTO coaches (name, email, password, gender, dob, otp) VALUES ('%s','%s','%s', ' ', ' ', '%s')" % (name, email, password, otp))
    mysql.connection.commit()
    cursor.close()
    verify_email(email, otp)
    return 'true'


def verify_email(email, otp):
    msg = Message('TDQ Account Verification', sender='tdq.noreply@gmail.com', recipients=['%s' % email])
    msg.body = "Thank you for making an account with TDQ, the last step is to verify" \
               " your account please enter the OTP into the app: %s\n" \
               "Kind regards,\n" \
               "The TDQ Team" % otp
    mail.send(msg)

@coach_blueprint.route('/coach/verifyAccount/<email>/<otp>')
def verify_account(email, otp):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s' AND otp = '%s'" % (email, otp))
    row_count = cursor.rowcount

    if row_count == 1:
        return 'true'
    else:
        return 'false'


@coach_blueprint.route('/coach/checkAccountExists/<email>/<password>')
def checkAccountExists(email, password):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s' AND password = '%s'" % (email, password))
    row_count = cursor.rowcount
    mysql.connection.commit()

    cursor.close()

    if row_count == 0:
        return 'false'
    else:
        return 'true'


@coach_blueprint.route('/coach/getUser/<email>')
def get_user_details(email):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s'" % email)
    rv = cursor.fetchall()
    return str(rv)


@coach_blueprint.route('/coach/editUser/<id>/<email>/<name>/<gender>/<dob>')
def edit_user_details(id, email, name, gender, dob):
    cursor = mysql.connection.cursor()
    cursor.execute("UPDATE coaches SET email = '%s', name = '%s', gender = '%s', dob = '%s' WHERE coach_id = '%s'" %
                   (email, name, gender, dob, id))
    mysql.connection.commit()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s'" % email)
    return str(cursor.fetchall())



@coach_blueprint.route('/coach/createQuestionnaire/<name>/<qType>/<email>/<list>/<otp>')
def createQuestionnaire(name, qType, email, list, otp):
    cursor = mysql.connection.cursor()

    cursor.execute("SELECT * FROM questionnaire WHERE name = '%s' AND coach_id = (SELECT coach_id FROM coaches WHERE email = '%s')" % (name, email))
    
    if cursor.rowcount > 0:
     addAthletes(list, email, otp, name)
    else:
        cursor.execute("INSERT INTO questionnaire (name, type, coach_id) VALUES ('%s','%s', (SELECT coach_id FROM coaches WHERE email = '%s'))" % (name, qType, email))
        mysql.connection.commit()
        addAthletes(list, email, otp, name)
    cursor.execute("SELECT * FROM questionnaire WHERE name = '%s' AND coach_id = (SELECT coach_id FROM coaches WHERE email = '%s')" % (name, email))
    return str(cursor.fetchall())


def addAthletes(athlete, email, otp, quiz_name):
    cursor =  mysql.connection.cursor()
    cursor.execute("INSERT INTO emails_verify (questionnaire_id, email, code) VALUES ((SELECT questionnaire_id FROM questionnaire WHERE name = '%s' and coach_id = (SELECT coach_id FROM coaches WHERE email = '%s')), '%s', '%s')" % (quiz_name, email, athlete, otp))
    mysql.connection.commit()
    send_athlete_email(athlete, otp, email)
    return 'true'


def send_athlete_email(athlete, otp, email):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT name FROM coaches WHERE email = '%s'" % email)
    name = cursor.fetchall()[0]
    msg = Message('TDQ Coach Questionnaire Invite', sender='tdq.noreply@gmail.com', recipients=['%s' % athlete])
    msg.body = "Your coach, %s, has invited you to complete a questionnaire on our Talent Development Questionnaire " \
               "app.\n After downloading the app please go to the questionnaire page and use this OTP: %s\n" \
               "Kind regards,\n" \
               "The TDQ Team" % ("".join(name.values()), otp)
    mail.send(msg)


@coach_blueprint.route('/coach/deleteCoach/<email>/<password>')
def deleteCoach(email, password):
    if checkAccountExists(email, password) == 'true':
        cursor = mysql.connection.cursor()
        cursor.execute("DELETE FROM coaches WHERE email = '%s'" % email)
        mysql.connection.commit()
        cursor.close()
        return 'true'

    return 'false'



@coach_blueprint.route('/coach/getResults/<qID>/<qNumber>')
def getResults(qID, qNumber):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT completions FROM questionnaire WHERE questionnaire_id = '%s'" % qID)
    completions = cursor.fetchall()
    if completions != 0:
        count = completions[0]["completions"]
        cursor.execute("SELECT score FROM question WHERE questionnaire_id = '%s' AND question_no = '%s'" % (qID, qNumber))
        scoreTuple = cursor.fetchall()
        score = scoreTuple[0]["score"]
        dividedScore = score / count
        return str(dividedScore)
    mysql.connection.commit()
    cursor.close()
    return 'False'


@coach_blueprint.route('/coach/getQuestionnaires/<coachID>')
def get_questionnaires(coachID):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM questionnaire WHERE coach_id = '%s'" % coachID)
 
    if cursor.rowcount is not 0:
        return str(cursor.fetchall())

    return 'false'


@coach_blueprint.route('/coach/getQuestionnaireQuestions/<qID>')
def get_questions(qID):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM question WHERE questionnaire_id = '%s'" % qID)
    if cursor.rowcount is not 0:
       return str(cursor.fetchall())

    return 'false' 


@coach_blueprint.route('/coach/getQuestions/<qType>')
def getQuestions(qType):

    path = Path(__file__).parent.absolute()
    if qType == '59':
        questions = open("%s/59questions.txt" %(path), "r", encoding="utf-8")
    else:
        questions = open("%s/28questions.txt" %(path), "r", encoding="utf-8")

    return questions.read()


@coach_blueprint.route('/coach/verifyOTP/<email>/<otp>')
def verifyOTP(email, otp):
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT otp FROM coaches WHERE email = '%s'" % email)
    result = cursor.fetchall()
    serverOTP = result[0]["otp"]
    if otp == serverOTP:
        cursor.execute("UPDATE coaches SET otp = 'true' WHERE email = '%s'" % email)
        mysql.connection.commit()
        cursor.close()
        return 'True'
    elif serverOTP == 'true':
        return 'True'
    cursor.close()
    return 'False'


@coach_blueprint.route('/coach/recieveAthleteInfo')
def recieveAthleteInfo():
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM athlete_info")
    result = cursor.fetchall()
    info  = str(result)
    mysql.connection.commit()
    cursor.close()
    return info

