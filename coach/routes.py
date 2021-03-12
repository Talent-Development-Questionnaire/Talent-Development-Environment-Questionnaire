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

@coach_blueprint.route('/coach/createQuestionnaire/<name>/<type>/<email>')
def createQuestionnaire(name, type, email):
	cursor = mysql.connection.cursor()
	cursor.execute("SELECT coach_id FROM coaches WHERE email = '%s'" % (email))
	coach_id = cursor.fetchall
	cursor.execute("INSERT INTO questionnaire (coach_id, name, type) VALUES ('%s','%s','%i')" % (coach_id, name, type))
	mysql.connection.commit()
	cursor.close()
	return 'true'

