# Use an official Node.js runtime as the base image
FROM node:latest

# Expose the port where your React app will run
EXPOSE 8082

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run build



# Command to run the application
CMD ["npm", "start"]