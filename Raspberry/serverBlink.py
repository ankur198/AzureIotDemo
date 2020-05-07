import RPi.GPIO as GPIO
from time import sleep
from flask import Flask

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)

GPIO.setup(8, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(10, GPIO.OUT, initial=GPIO.LOW)

app = Flask(__name__)


@app.route('/')
def index():
    return 'hello world'


@app.route('/<int:pin_num>/<int:value>')
def change_pin_state(pin_num, value):
    if pin_num == 8 or pin_num == 10 and value == 1 or value == 0:
        GPIO.output(pin_num, value)
        return f'{pin_num} {value}'
    return 'bad request'


if __name__ == "__main__":
    app.run(host='0.0.0.0')
