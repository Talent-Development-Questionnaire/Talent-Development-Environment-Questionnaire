
import flask
from flask_mysqldb import MySQL
from flask import Flask
from coach.routes import coach_blueprint
from player.routes import player_blueprint
from admin.routes import admin_blueprint
from config import app_config
from app.extensions import mysql, mail


def create_app(config_name):

	app = flask.Flask(__name__, template_folder='templates', instance_relative_config=False)

	app.config['MYSQL_HOST'] = 'localhost'  # sql connection details
	app.config['MYSQL_USER'] = 'root'
	app.config['MYSQL_PASSWORD'] = 'Planetoftheapes12'
	app.config['MYSQL_DB'] = 'talent_development_questionnaire'
	app.config['MYSQL_CURSORCLASS'] = 'DictCursor'

	app.config['MAIL_SERVER'] = 'smtp.gmail.com'  # mail server connection details
	app.config['MAIL_PORT'] = 465
	app.config['MAIL_USERNAME'] = 'tdq.noreply@gmail.com'
	app.config['MAIL_PASSWORD'] = 'Planetoftheapes12'
	app.config['MAIL_USE_TLS'] = False
	app.config['MAIL_USE_SSL'] = True
	
	mysql.init_app(app)
	mail.init_app(app)

	app.register_blueprint(coach_blueprint)  # registering blueprints to the app
	app.register_blueprint(player_blueprint, url_prefix='/player')
	app.register_blueprint(admin_blueprint)

	return app


