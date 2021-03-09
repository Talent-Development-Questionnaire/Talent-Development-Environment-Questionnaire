
import flask
from flask_mysqldb import MySQL
from flask import Flask
from .coach.routes import coach_blueprint
from .player.routes import player_blueprint
from .config import app_config
from .extensions import mysql

def create_app(config_name):

	app = flask.Flask(__name__, instance_relative_config=False)
	app.config.from_object(app_config[config_name])
	app.config.from_pyfile('config.py')

	app.config['MYSQL_HOST'] = 'localhost'
	app.config['MYSQL_USER'] = 'root'
	app.config['MYSQL_PASSWORD'] = 'Planetoftheapes12'
	app.config['MYSQL_DB'] = 'talent_development_questionnaire'
	
	mysql.init_app(app)

	app.register_blueprint(coach_blueprint)
	app.register_blueprint(player_blueprint)


	return app


