from flask import Blueprint, Flask, jsonify 
from flask_mysqldb import MySQL

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
               " Kind regards,\n" \
               " The TDQ Team" % otp
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
    cursor.execute("INSERT INTO questionnaire (name, type, coach_id) VALUES ('%s','%s', (SELECT coach_id FROM coaches WHERE email = '%s'))" % (name, qType, email))
    mysql.connection.commit()
    cursor.close()
    return addAthletes(list, email, otp, name)


def addAthletes(email_list, email, otp, quiz_name):
    cursor =  mysql.connection.cursor()
    for single_email in email_list:
        cursor.execute("INSERT INTO emails_verify (questionnaire_id, email, code) VALUES ((SELECT questionnaire_id FROM questionnaire WHERE name = '%s' and coach_id = (SELECT coach_id FROM coaches WHERE email = '%s')), '%s', '%s')" % (quiz_name, email, single_email, otp))
    mysql.connection.commit()
    return 'true'


@coach_blueprint.route('/coach/deleteCoach/<email>/<password>')
def deleteCoach(email, password):
    if checkAccountExists(email, password) == 'false':
        cursor = mysql.connection.cursor()
        cursor.execute("DELETE FROM coaches WHERE email = '%s'" % email)
        mysql.connection.commit()
        cursor.close()
        return 'true'
    else:
        return 'false'



