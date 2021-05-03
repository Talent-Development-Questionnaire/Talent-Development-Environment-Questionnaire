import sys
import os

sys.path.insert(0, '/usr/local/env/tdqApp/')
os.environ['FLASK_ENV'] = 'production'

from run import app as application
