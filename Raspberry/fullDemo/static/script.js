const serverUrl = window.location.origin;

async function getStatus() {
  const res = await fetch(serverUrl + "/status");
  return res.json();
}
