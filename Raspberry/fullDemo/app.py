import RPi.GPIO as GPIO
from time import sleep
from threading import Thread
from flask import Flask, render_template
import json


class LED():
    def __init__(self, pin):
        self.pin = pin
        self.value = 0
        GPIO.setup(self.pin, GPIO.OUT, initial=self.value)

    def set_pin(self, value):
        self.value = value
        GPIO.output(self.pin, self.value)

    def __str__(self):
        return self.value


GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)

pins = {
    8: LED(8),
    10: LED(10)
}

app = Flask(__name__, static_url_path='', static_folder='static')

discoThread = None
discoFlag = False


# @app.route('/')
# def index():
#     return render_template('index.html')


@app.route('/status')
def status():
    res = []
    for pin in pins:
        res.append(pins[pin].__dict__)
    res.append({'disco': discoFlag})
    return json.dumps(res)


@app.route('/<int:pin_num>/<int:value>')
def change_pin_state(pin_num, value):
    if pin_num == 8 or pin_num == 10 and value == 1 or value == 0:
        pins[pin_num].set_pin(value)
        return f'{pin_num} {value}'
    return 'bad request'


@app.route('/disco/<int:value>')
def disco(value):
    global discoThread, discoFlag
    if value == 1:
        discoThread = Thread(target=blink)
        discoFlag = True
        discoThread.start()
    elif discoThread != None:
        discoFlag = False
        discoThread = None
    return 'ok'


def blink():
    global discoFlag
    while discoFlag:
        pins[8].set_pin(1)
        sleep(1)  # Sleep for 1 second
        pins[8].set_pin(0)
        sleep(1)  # Sleep for 1 second


if __name__ == "__main__":
    app.run(host='0.0.0.0')
