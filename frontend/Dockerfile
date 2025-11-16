# Development Dockerfile for Angular application
FROM node:18-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Install Angular CLI globally for development
RUN npm install -g @angular/cli@15

# Copy source code (will be overridden by volume in docker-compose)
COPY . .

# Expose Angular dev server port
EXPOSE 4200

# Start Angular dev server with hot reload configuration
CMD ["npm", "run", "start:docker"]