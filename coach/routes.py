from flask import Blueprint, Flask, current_app
from flask_mysqldb import MySQL

from ..extensions import mysql

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


@coach_blueprint.route('/coach/addCoach/<email>/<password>/<name>')
def addcoach(name, email, password):
    cursor = mysql.connection.cursor()
    cursor.execute("INSERT INTO coaches (name, email, password) VALUES ('%s','%s','%s')" % (name, email, password))
    mysql.connection.commit()
    cursor.close()
    return 'true'


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
    cursor.execute("UPDATE coaches SET email = '%s', name = '%s', gender = '%s', dob = '%s' WHERE id = '%s'" %
                   (email, name, gender, dob, id))
    mysql.connection.commit()
    cursor.execute("SELECT * FROM coaches WHERE email = '%s'" % email)

    return str(cursor.fetchall())


@coach_blueprint.route('/coach/createQuestionnaire/<name>/<qType>/<email>')
def createQuestionnaire(name, qType, email):
    cursor = mysql.connection.cursor()
    cursor.execute("INSERT INTO questionnaire (name, type, coach_id) VALUES ('%s','%s', (SELECT coach_id FROM coaches WHERE email = '%s'))" % (name, qType, email))
    mysql.connection.commit()
    cursor.close()
    return 'true'
