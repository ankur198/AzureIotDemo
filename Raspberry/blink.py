import RPi.GPIO as GPIO    # Import Raspberry Pi GPIO library
from time import sleep     # Import the sleep function from the time module

# Initilize
GPIO.setwarnings(False)    # Ignore warning for now
GPIO.setmode(GPIO.BOARD)   # Use physical pin numbering
# Set pin 8 to be an output pin and set initial value to low (off)
GPIO.setup(8, GPIO.OUT, initial=GPIO.LOW)


while True:  # Run forever
    print('turning on')
    GPIO.output(8, GPIO.HIGH)  # Turn on
    sleep(1)  # Sleep for 1 second
    print('turning off')
    GPIO.output(8, GPIO.LOW)  # Turn off
    sleep(1)  # Sleep for 1 second
