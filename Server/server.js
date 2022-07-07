const http = require('http');

// Set IP and port, that the server should listen on
const hostIPAddress = '0.0.0.0';
const port = 42660; // REPLACE with the port you opened in the previous step

// Define what the server should return to a client upon connection request
const server = http.createServer((request, result) => {
  result.statusCode = 200;
  // In this case it returns a simple text message
  result.setHeader('Content-Type', 'text/plain');
  result.end('Welcome to my NodeJS server');
});

// Start listening for requests on the defined port and IP
server.listen(port, hostIPAddress, () => {
  // Receive a confirmation message when server starts listening
  console.log(`Server running at http://${hostIPAddress}:${port}/`);
});
