gpio -1 mode 10 out
gpio -1 mode 8 out

while true
do:
    gpio -1 write 10 1
    gpio -1 write 8 0
    sleep 1
    gpio -1 write 10 0
    gpio -1 write 8 1
    sleep 1
done