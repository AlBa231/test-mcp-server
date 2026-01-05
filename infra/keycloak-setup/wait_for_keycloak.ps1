        $maxTries = 30
        $delay = 5
        for ($i=0; $i -lt $maxTries; $i++) {
          try {
            $response = Invoke-WebRequest -Uri https://d2nn2xq50pm2hj.cloudfront.net/auth/realms/master -UseBasicParsing -ErrorAction Stop
            Write-Host "Checking the URL.... Status - HTTP status code: $($response.StatusCode)"
            if ($response.StatusCode -eq 200) { exit 0 }
          } catch {
            Write-Host "Error. $($_.Exception.Message). Retrying..."
            Start-Sleep -Seconds $delay
          }
        }    
