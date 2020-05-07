import RPi.GPIO as GPIO
from time import sleep
from threading import Thread
from flask import Flask

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)

GPIO.setup(8, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(10, GPIO.OUT, initial=GPIO.LOW)

app = Flask(__name__)
discoThread = None


@app.route('/')
def index():
    return 'hello world'


@app.route('/<int:pin_num>/<int:value>')
def change_pin_state(pin_num, value):
    if pin_num == 8 or pin_num == 10 and value == 1 or value == 0:
        GPIO.output(pin_num, value)
        return f'{pin_num} {value}'
    return 'bad request'


@app.route('/disco/<int:value>')
def disco(value):
    global discoThread
    if value == 1:
        discoThread = Thread(target=blink)
        discoThread.start()
    elif discoThread != None:
        discoThread._stop()
    return 'ok'


def blink():
    while True:
        GPIO.output(8, GPIO.HIGH)  # Turn on
        GPIO.output(10, GPIO.LOW)  # Turn on
        sleep(1)  # Sleep for 1 second
        GPIO.output(8, GPIO.LOW)  # Turn off
        GPIO.output(10, GPIO.HIGH)  # Turn off
        sleep(1)  # Sleep for 1 second


if __name__ == "__main__":
    app.run(host='0.0.0.0')
