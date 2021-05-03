from flask import Blueprint, Flask, jsonify, render_template, current_app, request
from flask_mysqldb import MySQL

import io
import csv

from werkzeug.wrappers import Response
from pathlib import Path
from app.extensions import mysql

admin_blueprint = Blueprint('admin_blueprint', __name__)


@admin_blueprint.route('/admin')
def index_page():
    return render_template('index.html')


@admin_blueprint.route('/admin/adminInfo/<email>/<password>')
def adminInfo(email, password):
    if verifyAdmin(email, password) == 'True':
        cursor = mysql.connection.cursor()
        cursor.execute("SELECT * FROM coaches")
        result = cursor.fetchall()
        info = str(result)
        mysql.connection.commit()
        cursor.close()
        return info

    return 'Incorrect Details'


@admin_blueprint.route('/admin/main')
def main_page():
    return render_template('main.html')


@admin_blueprint.route('/admin/login/verifyLogin', methods=['GET', 'POST'])
def verify_admin():
    
    email = request.args.get('email')
    password = request.args.get('password')
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM admin WHERE email = '%s' AND password = '%s'" % (email, password))
    
    if cursor.rowcount == 1:
        return render_template('main.html')
    else:
        cursor.close()
        return str(email)


def deleteRecord(id):
    cursor = mysql.connection.cursor()
    cursor.execute("DELETE FROM coaches  WHERE coach_id = '%s'" % id)
    mysql.connection.commit()
    cursor.close()
    return 'True'


def editRecord(id, field, value):
    cursor = mysql.connection.cursor()
    cursor.execute("UPDATE coaches SET '%s' = '%s' WHERE coach_id = '%s'" % (field, value, id))
    mysql.connection.commit()
    cursor.close()
    return 'True'


@admin_blueprint.route('/download/athlete_info')
def download_athlete_report():
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM athlete_info")
    result = cursor.fetchall()
    output = io.StringIO()
    writer = csv.writer(output)
                   
    line = ['athlete_id, sport_academy, sport, name, age, gender']
    writer.writerow(line)

    for row in result:
        line = [
            str(row['athlete_id']) + ',' + row['sport_academy'] + ',' + row['sport'] + ',' + row[
                'name'] + ',' + str(row['age']) + ',' + row['gender']]
        writer.writerow(line)

    output.seek(0)

    return Response(output, mimetype="text/csv",
                    headers={"Content-Disposition": "attachment;filename=athletes_report.csv"})


@admin_blueprint.route('/download/coaches')
def download_coaches_report():
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM coaches")
    result = cursor.fetchall()
    output = io.StringIO()
    writer = csv.writer(output)

    line = ['coach_id, name, gender, dob, email']
    writer.writerow(line)

    for row in result:
        line = [
            str(row['coach_id']) + ',' + row['name'] + ',' + row['gender'] + ',' + row['dob'] + ','
            + str(row['email'])]
        writer.writerow(line)

    output.seek(0)

    return Response(output, mimetype="text/csv",
                    headers={"Content-Disposition": "attachment;filename=coaches_report.csv"})


@admin_blueprint.route('/download/questionnaire')
def download_questionnaire_report():
    cursor = mysql.connection.cursor()
    cursor.execute("SELECT * FROM questionnaire")
    result = cursor.fetchall()
    output = io.StringIO()
    writer = csv.writer(output)

    line = ['questionnaire_id, coach_id, name, type']
    writer.writerow(line)

    for row in result:
        line = [
            str(row['questionnaire_id']) + ',' + str(row['coach_id']) + ',' + row['name'] + ',' + str(row['type'])]
        writer.writerow(line)

    cursor.execute("SELECT * FROM question")
    question_result = cursor.fetchall()
    question_line = ['questionnaire_id, question_no, score']
    writer.writerow(question_line)

    for row in question_result:
        line = [
            str(row['questionnaire_id']) + ',' + str(row['question_no']) + ',' + str(row['score'])]
        writer.writerow(line)

    output.seek(0)

    return Response(output, mimetype="text/csv",
                    headers={"Content-Disposition": "attachment;filename=questionnaire_report.csv"})
