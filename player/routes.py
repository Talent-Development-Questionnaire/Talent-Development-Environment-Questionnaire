from flask import Blueprint, Flask, current_app
from flask_mysqldb import MySQL

from ..extensions import mysql

player_blueprint = Blueprint('player_blueprint', __name__)