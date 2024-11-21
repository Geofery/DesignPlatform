#!/bin/bash

# Root directory of the project
root_dir=$(pwd)

# Define an array of microservices and their ports
declare -a microservices=(
  "UserManagementService/UserManagementService.Web:5001"
  "DesignRepositoryService/DesignRepositoryService.Web:5002"
  "CommunicationService/CommunicationService.Web:5003"
  "FurnitureLibraryService/FurnitureLibraryService.Web:5004"
  "PaymentService/PaymentService.Web:5005"
)

# Iterate over each microservice
for entry in "${microservices[@]}"
do
  # Split the entry into service path and port
  service=$(echo "$entry" | cut -d':' -f1)
  port=$(echo "$entry" | cut -d':' -f2)

  echo "Building and running $service on port $port..."

  # Navigate to the microservice directory
  service_path="$root_dir/$service"
  if [ -d "$service_path" ]; then
    cd "$service_path"

    # Restore dependencies
    if dotnet restore; then
      echo "Dependencies restored for $service"
    else
      echo "Failed to restore dependencies for $service. Skipping..."
      continue
    fi

    # Build the project
    if dotnet build -c Release; then
      echo "Build succeeded for $service"
    else
      echo "Build failed for $service. Skipping..."
      continue
    fi

    # Run the project in a new terminal window
    osascript <<EOF
      tell application "Terminal"
        do script "cd $service_path && dotnet run --urls=http://localhost:$port"
      end tell
EOF

    # Wait for the service to start
    echo "Waiting for $service to initialize..."
    sleep 7

    # Open the service in Safari
    open -a Safari "http://localhost:$port/swagger"

    # Navigate back to the root directory
    cd "$root_dir"
  else
    echo "Directory $service not found. Skipping..."
  fi
done

echo "All microservices have been processed and opened in new terminals and Safari!"
