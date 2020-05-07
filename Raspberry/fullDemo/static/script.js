const serverUrl = window.location.origin;

async function getStatus() {
  const res = await fetch(serverUrl + "/status");
  return res.json();
}

async function onButtonChanged(element) {
  if (element.id === "red") {
    await fetch(serverUrl + "/10/" + element.checked ? "1" : "0");
  }
  console.log(element.id, element.checked);
}

function onLoaded() {
  document.querySelectorAll(".button input[type=checkbox]").forEach((x) => {
    x.addEventListener("click", (y) => onButtonChanged(x));
  });
}
