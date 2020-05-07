# How to Run?

- Make sure you are running on raspberry pi with DEBIAN derivative
- give permission to execute to installDependencies.sh (`chmod u=rwx,g=rw,o=r installDependencies.sh`)
- run `installDependencies.sh`
- run `blink.py`

# Control from Terminal

- `gpio -1 mode 8 out`
- `gpio -1 write 8 1`
- `gpio -1 write 8 0`
