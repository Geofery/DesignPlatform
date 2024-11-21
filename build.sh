#!/bin/bash

# Root directory of the project
root_dir=$(pwd)

# Define an array of microservices
microservices=(
  "UserManagementService/UserManagementService.Web"
  "DesignRepositoryService/DesignRepositoryService.Web"
  "CommunicationService/CommunicationService.Web"
  "FurnitureLibraryService/FurnitureLibraryService.Web"
  "PaymentService/PaymentService.Web"
)

# Iterate over each microservice
for service in "${microservices[@]}"
do
  echo "Building and running $service..."

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

    # Run the project in the background
    if dotnet run &; then
      echo "$service is running in the background"
    else
      echo "Failed to run $service"
    fi

    # Navigate back to the root directory
    cd "$root_dir"
  else
    echo "Directory $service not found. Skipping..."
  fi
done

echo "All microservices have been processed!"
