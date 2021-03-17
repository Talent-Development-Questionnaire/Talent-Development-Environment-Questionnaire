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


