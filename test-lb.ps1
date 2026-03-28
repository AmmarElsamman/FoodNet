$URL = "http://localhost:5000/catalog/instance"
$TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImpvaG5AdGVzdC5jb20iLCJuYW1laWQiOiIxIiwibmJmIjoxNzc0NTQxMDg5LCJleHAiOjE3NzUxNDU4ODksImlhdCI6MTc3NDU0MTA4OX0.9VRIBuG2k9lw55LEWFlieX3QoIiEupXV3zVHG-LgMK0"
$REQUESTS = 100

Write-Host "Sending $REQUESTS requests to $URL"
Write-Host "-----------------------------------"

for ($i = 1; $i -le $REQUESTS; $i++) {
    $response = Invoke-RestMethod -Uri $URL -Headers @{Authorization = "Bearer $TOKEN"}
    Write-Host "Request $i`: $($response.instance)"
}