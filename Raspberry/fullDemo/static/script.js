const serverUrl = window.location.origin;

const redEl = document.getElementById("red");
const whiteEl = document.getElementById("white");
const discoEl = document.getElementById("disco");

async function getStatus() {
  const res = await fetch(serverUrl + "/status");
  return res.json();
}

async function onButtonChanged(element) {
  let pinNum = null;
  switch (element.id) {
    case "red":
      pinNum = 10;
      break;
    case "white":
      pinNum = 8;
      break;
    case "disco":
      pinNum = "disco";
      break;
  }
  const url = `${serverUrl}/${pinNum}/${element.checked ? "1" : "0"}`;
  console.log(element.id, element.checked, url);
  await fetch(url);
}

function refreshState() {
  setInterval(async () => {
    const states = await getStatus();
    console.log(states);
    redEl.checked = states[0]
  }, 500);
}

function onLoaded() {
  document.querySelectorAll(".button input[type=checkbox]").forEach((x) => {
    x.addEventListener("click", (y) => onButtonChanged(x));
  });
  refreshState();
}
